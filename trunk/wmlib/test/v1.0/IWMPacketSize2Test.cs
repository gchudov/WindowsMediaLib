using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;
using WindowsMediaLib.Defs;

namespace v1._0
{
    class IWMPacketSizeTest
    {
        IWMPacketSize m_ps;

        public void DoTests()
        {
            Config();

            Test();
        }

        private void Test()
        {
            int i;

            m_ps.SetMaxPacketSize(123);
            m_ps.GetMaxPacketSize(out i);
        }


        private void Config()
        {
            IWMProfile pProfile;
            IWMProfileManager pWMProfileManager;

            WMUtils.WMCreateProfileManager(out pWMProfileManager);
            IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;
            pWMProfileManager.CreateEmptyProfile(WMVersion.V9_0, out pProfile);

            m_ps = pProfile as IWMPacketSize;
        }

    }

}
