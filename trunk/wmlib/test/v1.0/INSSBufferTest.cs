using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

using WindowsMediaLib;

namespace v1._0
{
    public class INSSBufferTest
    {
        const int MAXLENGTH = 921600;
        private INSSBuffer m_pSample;

        public INSSBufferTest()
        {
        }

        public void DoTests()
        {
            Config();

            TestLength();
            TestBuffer();
        }

        private void TestLength()
        {
            int l;

            m_pSample.SetLength(12);
            m_pSample.GetLength(out l);

            Debug.Assert(l == 12);

            m_pSample.GetMaxLength(out l);
            Debug.Assert(l == MAXLENGTH);
        }

        private void TestBuffer()
        {
            IntPtr ip, ip2;
            int l;

            m_pSample.GetBuffer(out ip);
            Debug.Assert(ip != IntPtr.Zero);

            m_pSample.GetBufferAndLength(out ip2, out l);

            Debug.Assert(ip == ip2);
            Debug.Assert(l == 12);
        }

        private void Config()
        {
            IWMWriter pWMWriter;
            IWMProfileManager pWMProfileManager = null;
            IWMProfile pWMProfile = null;

            // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
            Guid guidProfileID = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

            WMUtils.WMCreateProfileManager(out pWMProfileManager);
            IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;
            pProfileManager2.SetSystemProfileVersion(WMVersion.V8_0);
            pProfileManager2.LoadProfileByID(guidProfileID, out pWMProfile);

            WMUtils.WMCreateWriter(IntPtr.Zero, out pWMWriter);
            pWMWriter.SetProfile(pWMProfile);
            pWMWriter.SetOutputFilename("foo.out");

            Marshal.ReleaseComObject(pWMProfile);
            Marshal.ReleaseComObject(pWMProfileManager);

            pWMWriter.BeginWriting();
            pWMWriter.AllocateSample(MAXLENGTH, out m_pSample);
        }
    }
}
