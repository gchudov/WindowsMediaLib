using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.IO;

using WindowsMediaLib;

namespace v1._0
{
    public class IWMReaderTest : IWMReaderCallback
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        IWMReader m_read;
        private Status m_LastStatus;
        private int m_SampleCount;

        public void DoTests()
        {
            m_SampleCount = 0;
            Config();

            TestOpen();
            TestControl();
            TestOutput();
            TestClose();
        }

        private void TestOutput()
        {
            int iCount, IFCount;
            IWMOutputMediaProps pProp;

            m_read.GetOutputCount(out iCount);
            Debug.Assert(iCount != 0);

            m_read.GetOutputProps(0, out pProp);
            Debug.Assert(pProp != null);
            m_read.SetOutputProps(0, pProp);

            m_read.GetOutputFormatCount(0, out IFCount);
            Debug.Assert(IFCount != 0);

            pProp = null;
            m_read.GetOutputFormat(0, 0, out pProp);
            Debug.Assert(pProp != null);
        }

        private void TestControl()
        {
            m_read.Start(0, 50000000, 1.0f, IntPtr.Zero);
            while (m_SampleCount == 0)
            {
                System.Threading.Thread.Sleep(1);
            }

            m_read.Pause();
            System.Threading.Thread.Sleep(100);
            int cur = m_SampleCount;
            System.Threading.Thread.Sleep(500);
            Debug.Assert(cur == m_SampleCount);

            m_read.Resume();
            System.Threading.Thread.Sleep(100);
            Debug.Assert(m_SampleCount != cur);

            m_read.Stop();
            WaitForStatus(Status.Stopped);
        }

#if false
        void GetOutputProps(
            [In] int dwOutputNum,
            out IWMOutputMediaProps ppOutput
            );

        void SetOutputProps(
            [In] int dwOutputNum,
            [In] IWMOutputMediaProps pOutput
            );

#endif

        private void TestClose()
        {
            m_read.Close();
            WaitForStatus(Status.Closed);
        }

        private void TestOpen()
        {
            m_read.Open(sFileName, this, new IntPtr(123));

            WaitForStatus(Status.Opened);

        }

        private void WaitForStatus(Status st)
        {
            while (m_LastStatus != st)
            {
                System.Threading.Thread.Sleep(1);
            }
        }

        private void Config()
        {
            WMUtils.WMCreateReader(IntPtr.Zero, 0, out m_read);
        }

        #region IWMReaderCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            Debug.Write(string.Format("Status: {0} 0x{1:x} {2} {3} {4} {5} ", iStatus, hr, WMError.GetErrorText(hr), dwType, pValue.ToInt32(), pvContext.ToInt32()));

            m_LastStatus = iStatus;

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

        public void OnSample(int dwOutputNum, long cnsSampleTime, long cnsSampleDuration, WriteFlags dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
            m_SampleCount++;
        }

        #endregion
    }
}
