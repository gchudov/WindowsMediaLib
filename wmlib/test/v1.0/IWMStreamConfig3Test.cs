using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.IO;

using WindowsMediaLib;
using WindowsMediaLib.Defs;

namespace v1._0
{
    public class IWMStreamConfig3Test
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        IWMStreamConfig3 m_sc;

        public void DoTests()
        {
            Config();

            Test();
        }

        private void Test()
        {
            short iLen = 0;
            StringBuilder sb = null;

            m_sc.SetLanguage("foobar");

            m_sc.GetLanguage(sb, ref iLen);
            Debug.Assert(iLen > 0);

            sb = new StringBuilder(iLen);
            m_sc.GetLanguage(sb, ref iLen);

            Debug.Assert(sb.ToString() == "foobar");
        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager;
            IWMProfile pProfile;
            IWMStreamConfig sc;

            WMUtils.WMCreateProfileManager(out pWMProfileManager);
            IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;

            pProfileManager2.CreateEmptyProfile(WMVersion.V8_0, out pProfile);

            pProfile.CreateNewStream(MediaType.Video, out sc);

            m_sc = sc as IWMStreamConfig3;
        }
    }
}
