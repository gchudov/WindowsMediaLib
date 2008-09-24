using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    public class ExternTest
    {
        const string sFileName = @"c:\so_lesson3c.wmv";

        public void DoTests()
        {
            Config();

            TestExt();
            TestScheme();
            TestOffline();
            TestMisc();
            TestProtected();
            TestValid();
        }

        private void TestExt()
        {
            WMUtils.WMCheckURLExtension(@"c:\foo.wmv");
            try
            {
                WMUtils.WMCheckURLExtension(@".fjfjjjfjjfjfjfjjjfjjf");
                Debug.Assert(false);
            }
            catch(Exception e)
            {
                int hr = Marshal.GetHRForException(e);
                Debug.Assert(hr == NSResults.E_INVALID_NAME);
            }
        }

        private void TestScheme()
        {
            WMUtils.WMCheckURLScheme("http");
            try
            {
                WMUtils.WMCheckURLScheme("ptth");
                Debug.Assert(false);
            }
            catch (Exception e)
            {
                int hr = Marshal.GetHRForException(e);
                Debug.Assert(hr == NSResults.E_INVALID_NAME);
            }
        }

        private void TestOffline()
        {
            bool b;

            WMUtils.WMIsAvailableOffline(sFileName, null, out b);
            Debug.Assert(b);
            WMUtils.WMIsAvailableOffline("http://85.25.144.234:3030", null, out b);
            Debug.Assert(!b);
        }

        private void TestMisc()
        {
            IWMWriterPushSink pSink;

            WMUtils.WMCreateWriterPushSink(out pSink);
            Debug.Assert(pSink != null);
        }

        private void TestProtected()
        {
            bool b;

            WMUtils.WMIsContentProtected(sFileName, out b);
            Debug.Assert(!b);
            WMUtils.WMIsContentProtected(@"c:\STC3_623kbps_550kbpsVideo_64kbpsAudio_30fps.wmv", out b);
            Debug.Assert(b);
            try
            {
                WMUtils.WMIsContentProtected(@"c:\notfound.wmv", out b);
                Debug.Assert(false);
            }
            catch (Exception e)
            {
                int hr = Marshal.GetHRForException(e);
                Debug.Assert(hr == NSResults.E_INVALID_NAME);
            }
        }

        private void TestValid()
        {
            int i = 0;
            byte[] b = null;

            BinaryReader m_br;
            m_br = new BinaryReader(new FileStream(sFileName, FileMode.Open, FileAccess.Read));

            WMUtils.WMValidateData(b, ref i);
            b = new byte[i];
            m_br.Read(b, 0, i);
            WMUtils.WMValidateData(b, ref i);

            try
            {
                b[0] = 55;
                WMUtils.WMValidateData(b, ref i);
                Debug.Assert(false);
            }
            catch (Exception e)
            {
                int hr = Marshal.GetHRForException(e);
                Debug.Assert(hr == NSResults.E_INVALID_NAME);
            }
        }

        private void Config()
        {
        }
    }
}
