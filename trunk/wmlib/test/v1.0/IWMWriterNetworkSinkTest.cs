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
    public class IWMWriterNetworkSinkTest
    {
        private const string sFileName = @"c:\so_lesson3c.wmv";
        private IWMWriterNetworkSink m_pSink;

        public void DoTests()
        {
            Config();

            Clients();
            TestMisc();
        }

        private void Clients()
        {
            int i;

            m_pSink.SetMaximumClients(13);
            m_pSink.GetMaximumClients(out i);

            Debug.Assert(i == 13);
        }

        private void TestProt()
        {
            NetProtocol pP;

            m_pSink.SetNetworkProtocol(NetProtocol.HTTP);
            m_pSink.GetNetworkProtocol(out pP);

            Debug.Assert(pP == NetProtocol.HTTP);
        }

        private void TestMisc()
        {
            StringBuilder sb = null;
            int iLen = 0;
            int iPort = 1011;

            m_pSink.Open(ref iPort);

            m_pSink.GetHostURL(sb, ref iLen);
            sb = new StringBuilder(iLen);
            m_pSink.GetHostURL(sb, ref iLen);

            Debug.Assert(sb.ToString().Contains(iPort.ToString()));

            m_pSink.Disconnect();
            m_pSink.Close();
        }

#if false
        void Disconnect();

        void Close();

#endif

        private void Config()
        {
            WMUtils.WMCreateWriterNetworkSink(out m_pSink);
        }
    }
}
