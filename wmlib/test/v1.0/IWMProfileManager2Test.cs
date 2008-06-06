using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    public class IWMProfileManager2Test
    {
        private IWMProfileManager2 m_pWMProfileManager2;

        public IWMProfileManager2Test()
        {
        }

        public void DoTests()
        {
            Config();

            TestVersion();
        }

        private void TestVersion()
        {
            WMVersion v;

            m_pWMProfileManager2.SetSystemProfileVersion(WMVersion.V7_0);
            m_pWMProfileManager2.GetSystemProfileVersion(out v);

            Debug.Assert(v == WMVersion.V7_0);
        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager;

            WMUtils.WMCreateProfileManager(out pWMProfileManager);

            m_pWMProfileManager2 = (IWMProfileManager2)pWMProfileManager;
        }
    }
}
