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
    public class IWMReaderTypeNegotiationTest : IWMReaderCallback
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        IWMReaderTypeNegotiation m_type;
        IWMOutputMediaProps m_pProp;
        Status m_LastStatus;

        public void DoTests()
        {
            Config();

            m_type.TryOutputProps(0, m_pProp);

        }

        private void Config()
        {
            IWMReader read;

            WMUtils.WMCreateReader(IntPtr.Zero, 0, out read);
            read.Open(sFileName, this, new IntPtr(123));
            while (m_LastStatus != Status.Opened)
                ;
            read.GetOutputProps(0, out m_pProp);

            m_type = read as IWMReaderTypeNegotiation;
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

        public void OnSample(int dwOutputNum, long cnsSampleTime, long cnsSampleDuration, WM_SF dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
        }

        #endregion
    }
}
