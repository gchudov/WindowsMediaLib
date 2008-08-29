using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    [ComVisible(true)]
    public class IWMReaderStreamClockTest : IWMReaderCallback
    {
        const string sFileName = @"c:\so_lesson3c.wmv";

        const long Sec = 10000000;
        private IWMReaderStreamClock m_pIWMReaderStreamClock;
        private bool m_Triggered = false;
        private int m_CheckTimer = 0;
        IWMReader m_read;

        public void DoTests()
        {
            Config();

            TestTimer();
            //TestKill();
        }

        private void TestKill()
        {
            long j;
            int i2;

            m_pIWMReaderStreamClock.GetTime(out j);
            m_pIWMReaderStreamClock.SetTimer(j + 50 * Sec, new IntPtr(456), out i2);

            // KillTimer behaves the same way here that it does in c++: It causes
            // an access violation.
            m_pIWMReaderStreamClock.KillTimer(i2);
        }

        private void TestTimer()
        {
            int i;
            long j;

            m_pIWMReaderStreamClock.GetTime(out j);

            m_pIWMReaderStreamClock.SetTimer(1, new IntPtr(234), out i);
            m_pIWMReaderStreamClock.SetTimer(j + Sec, new IntPtr(345), out m_CheckTimer);

            System.Threading.Thread.Sleep(3000);

            Debug.Assert(m_Triggered, "Triggered");
        }

        private void Config()
        {
            WMUtils.WMCreateReader(IntPtr.Zero, 0, out m_read);
            m_read.Open(sFileName, this, new IntPtr(123));
            System.Threading.Thread.Sleep(500);
            m_pIWMReaderStreamClock = (IWMReaderStreamClock)m_read;
        }

        #region IWMReaderCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            Debug.Write(string.Format("{0} 0x{1:x} {2} {3} {4} {5} ", iStatus, hr, WMError.GetErrorText(hr), dwType, pValue.ToInt32(), pvContext.ToInt32()));

            if ((iStatus == Status.Timer) && (dwType == AttrDataType.DWORD) && (Marshal.ReadInt32(pValue) == m_CheckTimer) && (pvContext.ToInt32() == 345))
            {
                m_Triggered = true;
            }

            switch (dwType)
            {
                case AttrDataType.STRING:
                    Debug.WriteLine(Marshal.PtrToStringUni(pValue));
                    break;
                case AttrDataType.WORD:
                    Debug.WriteLine(Marshal.ReadInt16(pValue));
                    break;
                case AttrDataType.DWORD:
                case AttrDataType.BOOL:
                    Debug.WriteLine(Marshal.ReadInt32(pValue));
                    break;
                case AttrDataType.QWORD:
                    Debug.WriteLine(Marshal.ReadInt64(pValue));
                    break;
                default:
                    Debug.WriteLine("???");
                    break;
            }

        }

        public void OnSample(int dwOutputNum, long cnsSampleTime, long cnsSampleDuration, WM_SF dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
