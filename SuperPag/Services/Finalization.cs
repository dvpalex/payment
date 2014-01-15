using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using SuperPag.Handshake;

namespace SuperPag.Services
{
    public class Finalization
    {
        int _sleepTime;
        int _sleepNumber;
        
        public Finalization(Interval intervalType, int interval)
        {
            //calcula o tempo que irá dormir e quando irá acordar
            switch (intervalType)
            {
                case Interval.Dia:
                    _sleepNumber = interval * 24;
                    _sleepTime = 1000 * 360;
                    break;
                case Interval.Hora:
                    _sleepNumber = interval;
                    _sleepTime = 1000 * 360;
                    break;
                case Interval.Minuto:                                     
                    _sleepTime = interval * 1000 * 60;
                    break;
                case Interval.Segundo:
                    _sleepNumber = 1;
                    _sleepTime = interval * 1000;
                    break;
            }
        }

        public void Start()
        {
            Thread th = new Thread(new ThreadStart(StartThread));
            th.IsBackground = true;
            th.Start();
        }

        private long[] GetJobs()
        {
            return null;
        }

        private void SendPosts()
        {
        }

        private void StartThread()
        {
            SendPosts();

            loop:
            for (int i = 1; i <= _sleepNumber; i++)
            {
                System.Threading.Thread.Sleep(_sleepTime);
            }
            SendPosts();

            goto loop;
        }

    }
}
