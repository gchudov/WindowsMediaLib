using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;
using WindowsMediaLib.Defs;

namespace v1._0
{
    class IWMProfile2Test
    {
        IWMProfile2 m_pProfile2;
        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid guidProfileID = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

        public void DoTests()
        {
            Guid g;
            Config();

            m_pProfile2.GetProfileID(out g);
            Debug.Assert(g == guidProfileID);
        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager;
            IWMProfile pProfile;


            WMUtils.WMCreateProfileManager(out pWMProfileManager);
            IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;
            pProfileManager2.SetSystemProfileVersion(WMVersion.V8_0);
            pProfileManager2.LoadProfileByID(guidProfileID, out pProfile);

            m_pProfile2 = (IWMProfile2)pProfile;
        }

    }

#if false
        void RemoveMutualExclusion(
            [In] IWMMutualExclusion pME
            );

#endif
}
