using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

using WindowsMediaLib;

namespace v1._0
{
    public class INSSBuffer2Test
    {
        const int MAXLENGTH = 921600;
        private INSSBuffer2 m_pSample;

        public INSSBuffer2Test()
        {
        }

        public void DoTests()
        {
            Config();

            TestProperties();
        }

        private void TestProperties()
        {
            byte [] p = new byte[100];

            try
            {
                m_pSample.GetSampleProperties(1, out p);
                Debug.Assert(false);  // According to the docs, this method is unimplemented
            }
            catch (NotImplementedException) { }

            try
            {
                m_pSample.SetSampleProperties(1, p);
                Debug.Assert(false);  // According to the docs, this method is unimplemented
            }
            catch (NotImplementedException) { }

        }

        private void Config()
        {
            IWMWriter pWMWriter;
            IWMProfileManager pWMProfileManager = null;
            IWMProfile pWMProfile = null;
            INSSBuffer pSample;

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
            pWMWriter.AllocateSample(MAXLENGTH, out pSample);
            m_pSample = (INSSBuffer2)pSample;
        }
    }
}
