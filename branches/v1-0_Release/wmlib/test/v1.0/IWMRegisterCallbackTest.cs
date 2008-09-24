// Two of the methods here (GetStatistics & GetStatistics) are actually tested over in IWMWriter, since
// they require a fully configured file, and I didn't want to dupe it all here.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

using WindowsMediaLib;
using WindowsMediaLib.Defs;
using System.Runtime.InteropServices.ComTypes;

namespace v1._0
{
    public class IWMRegisterCallbackTest : IWMStatusCallback
    {
        private const string sFileName = @"c:\WmTestOut.wmv";
        private IWMRegisterCallback m_rcb;
        private IWMWriterFileSink m_pSink;
        private bool m_bPassed;

        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

        public void DoTests()
        {
            Config();

            Test();
        }

        private void Test()
        {
            m_bPassed = false;

            m_rcb.Advise(this, new IntPtr(3));
            m_pSink.Open(sFileName);
            while (!m_bPassed)
                ;
            m_rcb.Unadvise(this, IntPtr.Zero);
        }

        private void Config()
        {
            IWMWriter pWriter;
            IWMWriterAdvanced pWriterA;

            WMUtils.WMCreateWriter(IntPtr.Zero, out pWriter);
            pWriterA = pWriter as IWMWriterAdvanced;

            WMUtils.WMCreateWriterFileSink(out m_pSink);

            pWriterA.AddSink(m_pSink);

            m_rcb = m_pSink as IWMRegisterCallback;
        }

        #region IWMStatusCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            if (pvContext == new IntPtr(3))
            {
                m_bPassed = true;
            }

            Debug.WriteLine(string.Format("{0} {1} {2} {3}", iStatus, hr, dwType, Marshal.ReadInt32(pValue)));
            switch (iStatus)
            {
                case Status.Closed:
                    {
                        break;
                    }
                case Status.Error:
                    {
                        break;
                    }
                case Status.IndexProgress:
                    break;
            }
        }

        #endregion
    }
}
