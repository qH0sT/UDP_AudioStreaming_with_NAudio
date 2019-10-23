using NAudio.Wave;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace MicRecieveUDP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Recorder rc;
        static  Thread play;
        static IPEndPoint ipep;
        static UdpClient newsock;
        private void button1_Click(object sender, EventArgs e)
        {
            if (ipep == null && newsock == null)
            {
                ipep = new IPEndPoint(IPAddress.Any, 9999);
                newsock = new UdpClient(ipep);
                
            }
            rc = new Recorder();
            button1.Enabled = false;
            button2.Enabled = true;
        }
        class Recorder
        {
            public Recorder()
            {
                play  = new Thread(new ThreadStart(Play));
                play.Start();
              
            }
            private void Play()
            {
                WaveOutEvent output = new WaveOutEvent();
                BufferedWaveProvider buffer = new BufferedWaveProvider(new WaveFormat(44100, 16, 2));
                output.Init(buffer);
                output.Play();
                for (;;)
                {
                    IPEndPoint remoteEP = null;
                    byte[] data = newsock.Receive(ref remoteEP);
                    buffer.AddSamples(data, 0, data.Length);
                }
            }
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            play.Abort();
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            play.Abort();
        }
    }
}
