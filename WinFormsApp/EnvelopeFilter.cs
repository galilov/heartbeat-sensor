using System;
using System.Collections.Generic;
using System.Linq;

namespace WinFormsApp
{
    internal class EnvelopeFilter
    {
        private readonly int _windowWidthInSamples;
        private readonly List<double> _window;
        private readonly Action<EnvelopeFilter> _resultConsumer;
        private long _windowCounter;
        private double _value;
        
        public EnvelopeFilter(int windowWidthInSamples, Action<EnvelopeFilter> resultConsumer)
        {
            _windowWidthInSamples = windowWidthInSamples;
            _resultConsumer = resultConsumer;
            _window = new List<double>(windowWidthInSamples);
        }

        public long WindowCounter => _windowCounter;
        public double Value => _value;
        
        public void ProcessSample(double sample)
        {
            _window.Add(Math.Abs(sample));
            if (_window.Count >= _windowWidthInSamples)
            {
                _value = _window.Average();
                _resultConsumer(this);
                _window.Clear();
                _windowCounter++;
            }
        }
    }
}