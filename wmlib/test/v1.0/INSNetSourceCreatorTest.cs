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
    public class INSNetSourceCreatorTest
    {
        private const string sFileName = @"c:\so_lesson3c.wmv";
        private INSNetSourceCreator m_nsc;

        public void DoTests()
        {
            object o;

            Config();

            m_nsc.Initialize();
            m_nsc.GetNetSourceAdminInterface("http", out o);

            IWMSInternalAdminNetSource ians = o as IWMSInternalAdminNetSource;
            Debug.Assert(ians != null);

            m_nsc.Shutdown();
        }

        private void Config()
        {
            ClientNetManager cnm = new ClientNetManager();
            m_nsc = cnm as INSNetSourceCreator;
        }
    }
}
