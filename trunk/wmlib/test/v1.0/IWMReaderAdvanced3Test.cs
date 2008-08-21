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
    public class IWMReaderAdvanced3Test : IWMReaderCallback
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        IWMReaderAdvanced3 m_read;
        private bool m_Opened;
        private bool m_CallbackCalled;

        public void DoTests()
        {
            Config();
            TestStart();
            TestStop();
        }

        private void TestStart()
        {
            IWMReader read = m_read as IWMReader;
            read.Pause();

            TimeCodeExtensionData tce1 = new TimeCodeExtensionData();
            TimeCodeExtensionData tce2 = new TimeCodeExtensionData();

            RA3Union l1 = new RA3Union(0L);
            RA3Union l2 = new RA3Union(123456789L);

            RA3Union l3 = new RA3Union(0);
            RA3Union l4 = new RA3Union(1234567);

            RA3Union l5 = new RA3Union(tce1);
            RA3Union l6 = new RA3Union(tce2);

            m_read.StartAtPosition(1, l1, l2, OffsetFormat.HundredNS, 1.0f, IntPtr.Zero);
            m_read.StartAtPosition(1, l3, l4, OffsetFormat.PlaylistOffset, 1.0f, IntPtr.Zero);

            // apparently SMPTE functionality is broken in WMF.  Apparently the indices aren't
            // created correctly.
            // m_read.StartAtPosition(1, l5, l6, OffsetFormat.Timecode, 1.0f, IntPtr.Zero);
        }

        private void TestStop()
        {
            m_read.StopNetStreaming();
        }

        private void Config()
        {
            IWMReader read;

            WMUtils.WMCreateReader(IntPtr.Zero, 0, out read);

            m_read = read as IWMReaderAdvanced3;
            DoOpen(read);
            DoStart(read);
        }

        private void DoOpen(IWMReader read)
        {
            m_Opened = false;
            read.Open(sFileName, this, new IntPtr(123));

            while (!m_Opened)
            {
                System.Threading.Thread.Sleep(0);
            }
        }

        private void DoStart(IWMReader read)
        {
            read.Start(0, 0, 1.0f, IntPtr.Zero);
            m_CallbackCalled = false;
            while (!m_CallbackCalled)
            {
                System.Threading.Thread.Sleep(0);
            }
        }

        #region IWMReaderCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            Debug.Write(string.Format("Status: {0} 0x{1:x} {2} {3} {4} {5} ", iStatus, hr, WMError.GetErrorText(hr), dwType, pValue.ToInt32(), pvContext.ToInt32()));

            if (iStatus == Status.Opened)
            {
                m_Opened = true;
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

        public void OnSample(int dwOutputNum, long cnsSampleTime, long cnsSampleDuration, WriteFlags dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
            m_CallbackCalled = true;
        }

        #endregion
    }
}
