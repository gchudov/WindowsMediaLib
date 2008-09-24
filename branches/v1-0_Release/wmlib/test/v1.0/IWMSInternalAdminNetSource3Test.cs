// Per the docs, GetNetSourceCreator2, IsUsingIE2 & RegisterProxyFailure2 are unimplemented

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

using WindowsMediaLib;
using System.Runtime.InteropServices.ComTypes;

namespace v1._0
{
    [ComVisible(true)]
    public class IWMSInternalAdminNetSource3Test
    {
        private const string sFileName = @"c:\so_lesson3c.wmv";
        private IWMSInternalAdminNetSource3 m_ian;

        public void DoTests()
        {
            Config();

            Test();
        }

        private void Test()
        {
            string pName, pw;
            bool b;
            NetSourceURLCredPolicySettings ps;

            m_ian.SetCredentialsEx2("foo", "bar", false, "david", "moo", true, false, true);
            m_ian.GetCredentialsEx2("foo", "bar", false, true, out ps, out pName, out pw, out b);

            Debug.Assert(pName == "david" && pw == "moo" && b == false);

            string s;
            long context = 0;
            int p;
            int hr = m_ian.FindProxyForURLEx2("http", "www.limegreensocks.com", "http://www.limegreensocks.com/DShow", out b, out s, out p, ref context);
            WMError.ThrowExceptionForHR(hr);

            m_ian.ShutdownProxyContext2(context);
        }

        private void Config()
        {
            object o;
            INSNetSourceCreator nsc;
            ClientNetManager cnm = new ClientNetManager();

            nsc = cnm as INSNetSourceCreator;
            nsc.Initialize();
            nsc.GetNetSourceAdminInterface("http", out o);

            m_ian = o as IWMSInternalAdminNetSource3;

            nsc.Shutdown();
        }
    }
}
