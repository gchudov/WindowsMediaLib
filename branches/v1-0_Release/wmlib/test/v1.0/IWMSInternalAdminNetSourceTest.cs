// Per the docs, GetNetSourceCreator, Initialize, & IsUsingIE are unimplemented

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
    public class IWMSInternalAdminNetSourceTest
    {
        private const string sFileName = @"c:\so_lesson3c.wmv";
        private IWMSInternalAdminNetSource m_ian;

        public void DoTests()
        {
            Config();

            TestCredFlag();
            TestCred();
            TestProxy();
        }

        private void TestCredFlag()
        {
            CredentialFlag c;
            m_ian.SetCredentialFlags(CredentialFlag.AutoSavePW);
            m_ian.GetCredentialFlags(out c);

            Debug.Assert(c == CredentialFlag.AutoSavePW);
        }

        private void TestCred()
        {
            string b, c;
            bool d;

            m_ian.SetCredentials("a", "b", "c", true, false);
            m_ian.GetCredentials("a", out b, out c, out d);

            Debug.Assert(b == "b" && c == "c" && d == false);

            m_ian.DeleteCredentials("a");
        }

        private void TestProxy()
        {
            string s;
            bool b;
            int i;
            int c = 0;

            int hr = m_ian.FindProxyForURL("http", "limegreensocks.com", out b, out s, out i, ref c);
            WMError.ThrowExceptionForHR(hr);

            Debug.Assert(b == false);

            m_ian.RegisterProxyFailure(-1, c);
            m_ian.ShutdownProxyContext(c);
        }

        private void Config()
        {
            object o;
            INSNetSourceCreator nsc;
            ClientNetManager cnm = new ClientNetManager();

            nsc = cnm as INSNetSourceCreator;
            nsc.Initialize();
            nsc.GetNetSourceAdminInterface("http", out o);

            m_ian = o as IWMSInternalAdminNetSource;

            nsc.Shutdown();
        }
    }
}
