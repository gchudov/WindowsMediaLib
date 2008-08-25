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
    public class IWMOutputMediaPropsTest : IWMReaderCallback
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        IWMOutputMediaProps m_pProp;
        Status m_LastStatus;

        public void DoTests()
        {
            Config();

            StringBuilder sb = null;
            short iLen = 0;

            m_pProp.GetConnectionName(sb, ref iLen);
            sb = new StringBuilder(iLen);
            m_pProp.GetConnectionName(sb, ref iLen);

            Debug.Assert(sb.ToString() == "0");

            sb = null;
            iLen = 0;
            m_pProp.GetStreamGroupName(sb, ref iLen);
            sb = new StringBuilder(iLen);
            m_pProp.GetStreamGroupName(sb, ref iLen);

        }

        private void Config()
        {
            IWMReader read;

            WMUtils.WMCreateReader(IntPtr.Zero, 0, out read);
            read.Open(sFileName, this, new IntPtr(123));
            while (m_LastStatus != Status.Opened)
                ;
            read.GetOutputProps(0, out m_pProp);
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
        }

        #endregion
    }
}
