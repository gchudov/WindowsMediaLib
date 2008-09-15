using System;
using System.Collections.Generic;
using System.Text;
using WindowsMediaLib;
using System.Threading;
using System.Diagnostics;

namespace v1._0
{
    public class IWMReaderPlaylistBurnTest : IWMStatusCallback
    {
        IWMReaderPlaylistBurn burn;
        private readonly object m_resultLock = new object();
        private const int iAborted = (unchecked((int)0xc00d2768));
        

        public void DoTests()
        {
            Config();

            TestInit();

            TestCancel();
        }

        private void TestInit()
        {
            string[] playlist = new string[1];
            playlist[0] = @"C:\so_lesson3c.wmv";
            burn.InitPlaylistBurn(1, playlist, this, new IntPtr(123));

            lock (m_resultLock)
            {
                Monitor.Wait(m_resultLock);
            }

            int[] results = new int[1];
            burn.GetInitResults(1, results);
            Debug.Assert(results[0] == 0);

            burn.EndPlaylistBurn(iAborted);
        }

        private void TestCancel()
        {
            string[] playlist = new string[1];
            playlist[0] = @"C:\so_lesson3c.wmv";
            burn.InitPlaylistBurn(1, playlist, this, new IntPtr(123));
            burn.Cancel();
        }

        private void Config()
        {
            IWMReader reader;            
            WMUtils.WMCreateReader(IntPtr.Zero, Rights.Playback, out reader);
            burn = (IWMReaderPlaylistBurn)reader;
        }

        #region IWMStatusCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            if (iStatus == Status.InitPlaylistBurn)
            {
                lock (m_resultLock)
                {
                    Monitor.PulseAll(m_resultLock);
                }
            }
        }

        #endregion
    }
}
