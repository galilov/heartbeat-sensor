using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private delegate void SafeCallDelegate(long windowCounter, double value);

        private const int Amplification = 10000;

        private readonly SignalProcessor _signalProcessor = new SignalProcessor();
        private Bitmap _dbmWave;
        private readonly LinkedList<Tuple<long, double>> _waveData = new LinkedList<Tuple<long, double>>();
        private readonly Font _drawFont = new Font("Arial", 12);

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _signalProcessor.StartRecord(
                (windowCounter, value) => Invoke(new SafeCallDelegate(UIOnAudioDataReceived), windowCounter, value));
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _signalProcessor.StopRecord();
        }


        private void UIOnAudioDataReceived(long windowCounter, double value)
        {
            if (_waveData.Count >= _dbmWave.Width)
            {
                _waveData.RemoveFirst();
            }

            _waveData.AddLast(new Tuple<long, double>(windowCounter, value));
            var avgData = _waveData.Skip(_waveData.Count - _dbmWave.Width / 20).Average(x => x.Item2);
            var bottomOffset = 20;
            long xOffset = 0;
            if (_waveData.Last().Item1 > _dbmWave.Width)
            {
                xOffset = _dbmWave.Width - _waveData.Last().Item1;
            }

            using (var g = Graphics.FromImage(_dbmWave))
            {
                g.FillRectangle(Brushes.Black, 0, 0, _dbmWave.Width, _dbmWave.Height);
                g.CompositingQuality = CompositingQuality.HighSpeed;


                g.DrawLine(Pens.Gray, 0, _dbmWave.Height / 2, _dbmWave.Width, _dbmWave.Height / 2);
                for (int i = 0; i < _dbmWave.Width; i++)
                {
                    if (i % 50 == 0)
                    {
                        g.DrawLine(Pens.Gray, i, 0, i, _dbmWave.Height - bottomOffset);
                        g.DrawString(((i * SignalProcessor.FilterWindowMilliseconds) / 1000f).ToString("F1"), _drawFont,
                            Brushes.Chartreuse, i, _dbmWave.Height - bottomOffset);
                    }
                }

                float prevX = 0;
                float prevY = 0;
                foreach (var d in _waveData)
                {
                    var windowNumber = d.Item1;
                    var detectedValue = avgData - d.Item2;
                    var y = (float) (_dbmWave.Height / 2f - (detectedValue * Amplification) + bottomOffset);
                    var x = windowNumber + xOffset;
                    g.DrawLine(Pens.LightGreen, prevX, prevY, x, y);
                    prevX = x;
                    prevY = y;
                }
            }


            pbx.Invalidate();
        }

        private void pbx_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(_dbmWave, 0, 0);
        }

        private void pbx_Resize(object sender, EventArgs e)
        {
            _dbmWave = new Bitmap(pbx.Width, pbx.Height);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //_signalProcessor.StopRecord();
        }
    }
}