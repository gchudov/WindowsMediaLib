using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    [ComVisible(true)]
    public class IWMReaderCallbackTest : IWMReaderCallback
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        private bool m_CallbackCalled = false;
        IWMReader m_read;

        public void DoTests()
        {
            Config();

            TestSample();
        }

        private void TestSample()
        {
            m_read.Start(0, 1, 1, new IntPtr(567));
            System.Threading.Thread.Sleep(300);
            Debug.Assert(m_CallbackCalled);
        }

        private void Config()
        {
            WMUtils.WMCreateReader(IntPtr.Zero, 0, out m_read);
            m_read.Open(sFileName, this, new IntPtr(123));
            System.Threading.Thread.Sleep(500);
        }

        #region IWMReaderCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            Debug.Write(string.Format("{0} 0x{1:x} {2} {3} {4} {5} ", iStatus, hr, WMError.GetErrorText(hr), dwType, pValue.ToInt32(), pvContext.ToInt32()));

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
            //throw new Exception("The method or operation is not implemented.");
            if (pvContext.ToInt32() == 567)
            {
                m_CallbackCalled = true;
            }
        }

        #endregion
    }
}
