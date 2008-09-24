using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    public class IWMReaderNetworkConfigTest
    {
        private IWMReaderNetworkConfig m_pIWMReaderNetworkConfig;

        public void DoTests()
        {
            Config();

            TestBufferTime();
            TestUDPPort();
            TestProxy();
            TestProxyHost();
            TestProxyPort();
            TestProxyExc();
            TestProxyBy();
            TestProxyRerun();
            TestMulti();
            TestHTTP();
            TestUDP();
            TestTCP();
            TestBand();
            TestProt();
            TestLogging();
            TestReset();
        }

        private void TestBufferTime()
        {
            const long v = 12345678;
            long q;

            m_pIWMReaderNetworkConfig.SetBufferingTime(v);
            m_pIWMReaderNetworkConfig.GetBufferingTime(out q);

            Debug.Assert(q == v);
        }

        private void TestUDPPort()
        {
            WMPortNumberRange[] nr = new WMPortNumberRange[2];
            int iPorts = 0;

            nr[0] = new WMPortNumberRange();
            nr[1] = new WMPortNumberRange();

            nr[0].wPortBegin = 33;
            nr[0].wPortEnd = 35;
            nr[1].wPortBegin = 37;
            nr[1].wPortEnd = 39;

            m_pIWMReaderNetworkConfig.SetUDPPortRanges(nr, nr.Length);

            m_pIWMReaderNetworkConfig.GetUDPPortRanges(null, ref iPorts);
            WMPortNumberRange[] nr2 = new WMPortNumberRange[iPorts];
            m_pIWMReaderNetworkConfig.GetUDPPortRanges(nr2, ref iPorts);

            Debug.Assert(nr[0].wPortBegin == nr2[0].wPortBegin && nr[1].wPortEnd == nr2[1].wPortEnd);
        }

        private void TestProxy()
        {
            ProxySettings p;

            m_pIWMReaderNetworkConfig.SetProxySettings("HTTP", ProxySettings.Auto);
            m_pIWMReaderNetworkConfig.GetProxySettings("HTTP", out p);

            Debug.Assert(p == ProxySettings.Auto);
        }

        private void TestProxyHost()
        {
            StringBuilder p;
            int pLen = 0;

            m_pIWMReaderNetworkConfig.SetProxyHostName("HTTP", "foo");

            m_pIWMReaderNetworkConfig.GetProxyHostName("HTTP", null, ref pLen);
            p = new StringBuilder(pLen);
            m_pIWMReaderNetworkConfig.GetProxyHostName("HTTP", p, ref pLen);

            Debug.Assert(p.ToString() == "foo");
        }

        private void TestProxyPort()
        {
            const int v = 222;
            int q;

            m_pIWMReaderNetworkConfig.SetProxyPort("HTTP", v);
            m_pIWMReaderNetworkConfig.GetProxyPort("HTTP", out q);

            Debug.Assert(q == v);
        }

        private void TestProxyExc()
        {
            int iLen = 0;
            StringBuilder p;

            m_pIWMReaderNetworkConfig.SetProxyExceptionList("HTTP", "moo");

            m_pIWMReaderNetworkConfig.GetProxyExceptionList("HTTP", null, ref iLen);
            p = new StringBuilder(iLen);
            m_pIWMReaderNetworkConfig.GetProxyExceptionList("HTTP", p, ref iLen);

            Debug.Assert(p.ToString() == "moo");
        }

        private void TestProxyBy()
        {
            bool b;

            m_pIWMReaderNetworkConfig.SetProxyBypassForLocal("HTTP", true);
            m_pIWMReaderNetworkConfig.GetProxyBypassForLocal("HTTP", out b);

            Debug.Assert(b == true);
        }

        private void TestProxyRerun()
        {
            bool b;

            m_pIWMReaderNetworkConfig.SetForceRerunAutoProxyDetection(true);
            m_pIWMReaderNetworkConfig.GetForceRerunAutoProxyDetection(out b);

            Debug.Assert(b == true);
        }

        private void TestMulti()
        {
            bool b;

            m_pIWMReaderNetworkConfig.SetEnableMulticast(false);
            m_pIWMReaderNetworkConfig.GetEnableMulticast(out b);

            Debug.Assert(b == false);
        }

        private void TestHTTP()
        {
            bool b;

            m_pIWMReaderNetworkConfig.SetEnableHTTP(false);
            m_pIWMReaderNetworkConfig.GetEnableHTTP(out b);

            Debug.Assert(b == false);
        }

        private void TestUDP()
        {
            bool b;

            m_pIWMReaderNetworkConfig.SetEnableUDP(false);
            m_pIWMReaderNetworkConfig.GetEnableUDP(out b);

            Debug.Assert(b == false);
        }

        private void TestTCP()
        {
            bool b;

            m_pIWMReaderNetworkConfig.SetEnableTCP(false);
            m_pIWMReaderNetworkConfig.GetEnableTCP(out b);

            Debug.Assert(b == false);
        }

        private void TestBand()
        {
            int i;

            m_pIWMReaderNetworkConfig.SetConnectionBandwidth(1011);
            m_pIWMReaderNetworkConfig.GetConnectionBandwidth(out i);

            Debug.Assert(i == 1011);
        }

        private void TestProt()
        {
            int i;

            m_pIWMReaderNetworkConfig.GetNumProtocolsSupported(out i);

            for (int x = 0; x < i; x++)
            {
                int iLen = 0;

                m_pIWMReaderNetworkConfig.GetSupportedProtocolName(x, null, ref iLen);
                StringBuilder p = new StringBuilder(iLen);
                m_pIWMReaderNetworkConfig.GetSupportedProtocolName(x, p, ref iLen);

                if (x == 0)
                {
                    Debug.Assert(p.ToString() == "HTTP");
                }
            }
        }

        private void TestLogging()
        {
            int i;
            int iLen = 0;

            m_pIWMReaderNetworkConfig.AddLoggingUrl(@"http://foo");
            m_pIWMReaderNetworkConfig.AddLoggingUrl(@"http://bar");

            m_pIWMReaderNetworkConfig.GetLoggingUrlCount(out i);
            Debug.Assert(i == 2);

            m_pIWMReaderNetworkConfig.GetLoggingUrl(1, null, ref iLen);
            StringBuilder p = new StringBuilder(iLen);
            m_pIWMReaderNetworkConfig.GetLoggingUrl(1, p, ref iLen);

            Debug.Assert(p.ToString() == "http://bar");

            m_pIWMReaderNetworkConfig.ResetLoggingUrlList();
            m_pIWMReaderNetworkConfig.GetLoggingUrlCount(out i);
            Debug.Assert(i == 0);
        }

        private void TestReset()
        {
            m_pIWMReaderNetworkConfig.ResetProtocolRollover();
        }

        private void Config()
        {
            IWMReader read;

            WMUtils.WMCreateReader(IntPtr.Zero, Rights.Playback, out read);
            m_pIWMReaderNetworkConfig = (IWMReaderNetworkConfig)read;
        }
    }
}
