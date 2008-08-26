// Per the docs, FindProxyForURLEx is unimplemented

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
    public class IWMSInternalAdminNetSource2Test
    {
        private const string sFileName = @"c:\so_lesson3c.wmv";
        private IWMSInternalAdminNetSource2 m_ian;

        public void DoTests()
        {
            Config();

            TestCred();
        }

        private void TestCred()
        {
            string sName, sPW;
            bool b;
            NetSourceURLCredPolicySettings ps;

            m_ian.SetCredentialsEx("foo", "bar", false, "david", "moo", true, false);

            m_ian.GetCredentialsEx("foo", "bar", false, out ps, out sName, out sPW, out b);
            Debug.Assert(sName == "david" && sPW == "moo" && b == false);

            m_ian.DeleteCredentialsEx("foo", "bar", false);
        }

        private void Config()
        {
            object o;
            INSNetSourceCreator nsc;
            ClientNetManager cnm = new ClientNetManager();

            nsc = cnm as INSNetSourceCreator;
            nsc.Initialize();
            nsc.GetNetSourceAdminInterface("http", out o);

            m_ian = o as IWMSInternalAdminNetSource2;

            nsc.Shutdown();
        }
    }
}
