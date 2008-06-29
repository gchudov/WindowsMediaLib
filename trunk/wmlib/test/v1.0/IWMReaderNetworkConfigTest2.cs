using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    public class IWMReaderNetworkConfig2Test
    {
        private IWMReaderNetworkConfig2 m_pIWMReaderNetworkConfig2;

        public void DoTests()
        {
            Config();

            TestContent();
            TestFast();
            TestResend();
            TestThin();
            TestAcc();
            TestRec();
            TestPack();
        }

        private void TestContent()
        {
            bool b;

            m_pIWMReaderNetworkConfig2.SetEnableContentCaching(false);
            m_pIWMReaderNetworkConfig2.GetEnableContentCaching(out b);

            Debug.Assert(b == false);
        }

        private void TestFast()
        {
            bool b;

            m_pIWMReaderNetworkConfig2.SetEnableFastCache(false);
            m_pIWMReaderNetworkConfig2.GetEnableFastCache(out b);

            Debug.Assert(b == false);
        }

        private void TestResend()
        {
            bool b;

            m_pIWMReaderNetworkConfig2.SetEnableResends(false);
            m_pIWMReaderNetworkConfig2.GetEnableResends(out b);

            Debug.Assert(b == false);
        }

        private void TestThin()
        {
            bool b;

            m_pIWMReaderNetworkConfig2.SetEnableThinning(false);
            m_pIWMReaderNetworkConfig2.GetEnableThinning(out b);

            Debug.Assert(b == false);
        }

        private void TestAcc()
        {
            long l;
            long v = 1200000000;

            m_pIWMReaderNetworkConfig2.SetAcceleratedStreamingDuration(v);
            m_pIWMReaderNetworkConfig2.GetAcceleratedStreamingDuration(out l);

            Debug.Assert(l == v);
        }

        private void TestRec()
        {
            int i;

            m_pIWMReaderNetworkConfig2.SetAutoReconnectLimit(12);
            m_pIWMReaderNetworkConfig2.GetAutoReconnectLimit(out i);

            Debug.Assert(i == 12);
        }

        private void TestPack()
        {
            int i;

            m_pIWMReaderNetworkConfig2.GetMaxNetPacketSize(out i);

            Debug.Assert(i == 65535);
        }

        private void Config()
        {
            IWMReader read;

            WMUtils.WMCreateReader(IntPtr.Zero, Rights.Playback, out read);
            m_pIWMReaderNetworkConfig2 = (IWMReaderNetworkConfig2)read;
        }
    }
}
