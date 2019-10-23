using NAudio.Wave;
using System;
using System.Net.Sockets;
using System.Windows.Forms;

namespace MicSendUDP
{
    public partial class Form1 : Form
    {
        Recorder rc;
        public Form1()
        {
            InitializeComponent();
          
        }

        class Recorder
        {
            UdpClient client = new UdpClient();
            WaveInEvent wave = new WaveInEvent();
            public Recorder()
            {
                //Thread play = new Thread(new ThreadStart(Play));
                //play.Start();
                wave.BufferMilliseconds = 30;
                wave.DeviceNumber = 0;
                wave.WaveFormat = new WaveFormat(44100, 16, 2);
                wave.DataAvailable += Wave_DataAvailable;
                wave.StartRecording();
            }
            public void Stop()
            {
                wave.StopRecording();
                wave.Dispose();
            }
            /*
            private void Play()
            {
                WaveOutEvent output = new WaveOutEvent();
                BufferedWaveProvider buffer = new BufferedWaveProvider(new WaveFormat(8000, 16, 2));
                output.Init(buffer);
                output.Play();
                for (;;)
                {
                    IPEndPoint remoteEP = null;
                    byte[] data = client.Receive(ref remoteEP);
                    buffer.AddSamples(data, 0, data.Length);
                }
            }
            */
          
            private void Wave_DataAvailable(object sender, WaveInEventArgs e)
            {
                client.Send(e.Buffer, e.BytesRecorded, "127.0.0.1", 9999);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
             rc = new Recorder();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rc.Stop();
        }
    }
}
