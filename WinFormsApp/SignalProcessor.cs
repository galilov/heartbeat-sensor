using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace WinFormsApp
{
    class SignalProcessor
    {
        private const int SampleRate = 48000; // Bin/s
        private const int ToneGenerator = 2400; // Hz
        public const int FilterWindowMilliseconds = 10;
        private const int WaveLengthInSamples = SampleRate / ToneGenerator;
        private const int BufferSizeInWaves = SampleRate;
        private const int FilterWindowInSamples = SampleRate * FilterWindowMilliseconds / 1000;

        private readonly BlockingCollection<float[]> _dataFromAsioCollection = new BlockingCollection<float[]>();
        private CancellationTokenSource _cancellationTokenSource;
        private AsioOut _asioOut;
        private Task _recordTask;
        private readonly byte[] _rawSamples = new byte[WaveLengthInSamples * sizeof(float)];
        private readonly BufferedWaveProvider _waveProvider;
        private EnvelopeFilter _envelopeFilter;
        private Action<long, double> _action;

        public SignalProcessor()
        {
            _waveProvider = new BufferedWaveProvider(WaveFormat.CreateIeeeFloatWaveFormat(SampleRate, 1));
            _waveProvider.BufferLength = WaveLengthInSamples * BufferSizeInWaves * sizeof(float);
            var samples = new float[WaveLengthInSamples];
            for (var i = 0; i < WaveLengthInSamples; i++)
            {
                samples[i] = (float) Math.Sin(2 * Math.PI * i / WaveLengthInSamples);
            }

            Buffer.BlockCopy(samples, 0, _rawSamples, 0, _rawSamples.Length);
        }

        public void StartRecord(Action<long, double> action)
        {
            if (_asioOut != null) return;
            _action = action;
            _cancellationTokenSource = new CancellationTokenSource();
            _asioOut = PrepareAsioOut();
            _asioOut.AudioAvailable += OnAsioOutAudioAvailable;
            _recordTask = Task.Run(ProcessAudioDataTask, _cancellationTokenSource.Token)
                .ContinueWith(t => { t.Exception?.Handle(e => true); }, TaskContinuationOptions.OnlyOnCanceled);
            _envelopeFilter = new EnvelopeFilter(FilterWindowInSamples,  OnFilterDataReady);
            PostTestSignalData();
            _asioOut.Play();
        }

        private void PostTestSignalData()
        {
            while (_waveProvider.BufferLength - _waveProvider.BufferedBytes >= _rawSamples.Length)
            {
                _waveProvider.AddSamples(_rawSamples, 0, _rawSamples.Length);
            }
        }

        private void OnAsioOutAudioAvailable(object sender, AsioAudioAvailableEventArgs e)
        {
            var samples = new float[e.SamplesPerBuffer * e.InputBuffers.Length];
            var nSamples = e.GetAsInterleavedSamples(samples);
            if (nSamples == 0)
            {
                return;
            }

            if (nSamples < samples.Length)
            {
                var realSamples = new float[nSamples];
                Array.Copy(samples, realSamples, nSamples);
                _dataFromAsioCollection.Add(realSamples);
            }
            else
            {
                _dataFromAsioCollection.Add(samples);
            }

            PostTestSignalData();
        }

        private void OnFilterDataReady(EnvelopeFilter filter)
        {
            _action(filter.WindowCounter, filter.Value);
        }

        public void StopRecord()
        {
            if (_asioOut == null)
            {
                return;
            }

            _asioOut.Stop();
            _cancellationTokenSource.Cancel();
            while (!_recordTask.IsCompleted)
            {
                _recordTask.Wait();
            }

            _asioOut.Dispose();
            _asioOut = null;
            _action = null;
        }

        private void ProcessAudioDataTask()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                var samples = _dataFromAsioCollection.Take(_cancellationTokenSource.Token);
                foreach (var sample in samples)
                {
                    _envelopeFilter.ProcessSample(sample);
                }
            }

            _cancellationTokenSource.Token.ThrowIfCancellationRequested();
        }

        private AsioOut PrepareAsioOut()
        {
            var names = AsioOut.GetDriverNames();

            var asioDriverName = names[0];
            var asioOut = new AsioOut(asioDriverName) {InputChannelOffset = 0};
            asioOut.InitRecordAndPlayback(_waveProvider, 1, SampleRate);
            return asioOut;
        }
    }
}