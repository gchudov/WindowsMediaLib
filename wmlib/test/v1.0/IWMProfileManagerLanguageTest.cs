using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    public class IWMProfileManagerLanguageTest
    {
        private IWMProfileManagerLanguage m_pl;

        public void DoTests()
        {
            Config();

            TestLang();
        }

        private void TestLang()
        {
            short s;

            m_pl.SetUserLanguageID(1033);
            m_pl.GetUserLanguageID(out s);

            Debug.Assert(s == 1033);
        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager;

            WMUtils.WMCreateProfileManager(out pWMProfileManager);

            m_pl = pWMProfileManager as IWMProfileManagerLanguage;
        }
    }
}
