using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    public class IWMProfileManagerTest
    {
        private IWMProfileManager m_pWMProfileManager;

        public IWMProfileManagerTest()
        {
        }

        public void DoTests()
        {
            Config();

            TestCreate();
            TestLoadID();
            TestSystem();
        }

        private void TestCreate()
        {
            IWMProfile pProfile;

            m_pWMProfileManager.CreateEmptyProfile(WMVersion.V9_0, out pProfile);
            Debug.Assert(pProfile != null);
        }

        private void TestLoadID()
        {
            int i = 3;
            IWMProfile pProfile, pProfile2;
            // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
            Guid guidProfileID = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

            m_pWMProfileManager.LoadProfileByID(guidProfileID, out pProfile);

            m_pWMProfileManager.SaveProfile(pProfile, null, ref i);
            Debug.Assert(i > 0);
            StringBuilder sb = new StringBuilder(i);
            m_pWMProfileManager.SaveProfile(pProfile, sb, ref i);
            Debug.Assert(sb.Length > 0);

            m_pWMProfileManager.LoadProfileByData(sb.ToString(), out pProfile2);
            Debug.Assert(pProfile2 != null);
        }

        private void TestSystem()
        {
            // GetSystemProfileCount & LoadSystemProfile only work if the profile version
            // is set to v8.  But the version can only be set by using IWMProfileManager2.  Go figure.
            int i;
            IWMProfile pProfile;

            IWMProfileManager2 pWMProfileManager2 = (IWMProfileManager2)m_pWMProfileManager;
            pWMProfileManager2.SetSystemProfileVersion(WMVersion.V8_0);

            m_pWMProfileManager.GetSystemProfileCount(out i);
            Debug.Assert(i > 0);

            m_pWMProfileManager.LoadSystemProfile(13, out pProfile);
            Debug.Assert(pProfile != null);
        }

        private void Config()
        {
            WMUtils.WMCreateProfileManager(out m_pWMProfileManager);
        }
    }
}
