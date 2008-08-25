using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;
using WindowsMediaLib.Defs;

namespace v1._0
{
    class IWMPacketSize2Test
    {
        IWMPacketSize2 m_ps;

        public void DoTests()
        {
            Config();

            Test();
        }

        private void Test()
        {
            int i;

            m_ps.SetMinPacketSize(100);
            m_ps.GetMinPacketSize(out i);

            Debug.Assert(i == 100);
        }


        private void Config()
        {
            IWMProfile pProfile;
            IWMProfileManager pWMProfileManager;

            WMUtils.WMCreateProfileManager(out pWMProfileManager);
            IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;
            pWMProfileManager.CreateEmptyProfile(WMVersion.V9_0, out pProfile);

            m_ps = pProfile as IWMPacketSize2;
        }

    }

}
