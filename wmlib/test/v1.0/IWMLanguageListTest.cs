using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;
using WindowsMediaLib.Defs;

namespace v1._0
{
    class IWMLanguageListTest
    {
        IWMLanguageList m_pLang;

        public void DoTests()
        {
            Config();

            Test();
        }

        private void Test()
        {
            short i, iIndex, iLen = 0;
            StringBuilder sb = null;

            m_pLang.GetLanguageCount(out i);

            Debug.Assert(i > 0);
            m_pLang.GetLanguageDetails(0, sb, ref iLen);
            sb = new StringBuilder(iLen);
            m_pLang.GetLanguageDetails(0, sb, ref iLen);

            Debug.Assert(sb.ToString() == "en-us");

            m_pLang.AddLanguageByRFC1766String("foo", out iIndex);
            Debug.Assert(iIndex == i);
        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager;
            IWMProfile pProfile;

            WMUtils.WMCreateProfileManager(out pWMProfileManager);
            IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;
            pProfileManager2.SetSystemProfileVersion(WMVersion.V8_0);
            pProfileManager2.CreateEmptyProfile(WMVersion.V8_0, out pProfile);

            m_pLang = pProfile as IWMLanguageList;
        }

    }
}
