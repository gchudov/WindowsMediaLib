using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

using WindowsMediaLib;

namespace v1._0
{
    public class INSSBuffer3Test
    {
        const int MAXLENGTH = 921600;
        private INSSBuffer3 m_pSample;

        public INSSBuffer3Test()
        {
        }

        public void DoTests()
        {
            Config();

            TestProperties();
        }

        private void TestProperties()
        {
            string s = "foo.asf";
            int i = (s.Length + 1) * 2;
            int j = 0;
            IntPtr p = Marshal.StringToBSTR(s);

            m_pSample.SetProperty(WM_SampleExtensionGUID.FileName, p, i);

            m_pSample.GetProperty(WM_SampleExtensionGUID.FileName, IntPtr.Zero, ref j);
            Debug.Assert(j == i);

            IntPtr p2 = Marshal.AllocCoTaskMem(j);
            m_pSample.GetProperty(WM_SampleExtensionGUID.FileName, p2, ref j);

            Debug.Assert(s == Marshal.PtrToStringUni(p2));
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
            m_pSample = (INSSBuffer3)pSample;
        }
    }
}
