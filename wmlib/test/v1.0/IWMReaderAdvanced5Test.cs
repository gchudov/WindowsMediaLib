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
    public class IWMReaderAdvanced5Test : IWMReaderCallback, IWMPlayerHook
    {
        const string sFileName = @"c:\Instancing10.wmv";
        IWMReaderAdvanced5 m_read;
        private bool m_Opened;
        private bool m_CallbackCalled;
        private bool m_Decode;

        public void DoTests()
        {
            m_Decode = false;
            Config();

            m_read.SetPlayerHook(0, this);
            (m_read as IWMReader).Start(0, 0, 1.0f, IntPtr.Zero);
            System.Threading.Thread.Sleep(5000);
            Debug.Assert(m_Decode);
        }

        private void Config()
        {
            IWMReader read;

            WMUtils.WMCreateReader(IntPtr.Zero, 0, out read);
            DoOpen(read);
            DoStart(read);

            m_read = read as IWMReaderAdvanced5;
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

        public void OnSample(int dwOutputNum, long cnsSampleTime, long cnsSampleDuration, SampleFlag dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
            m_CallbackCalled = true;
        }

        #endregion

        #region IWMPlayerHook Members

        public void PreDecode()
        {
            m_Decode = true;
        }

        #endregion
    }
}
