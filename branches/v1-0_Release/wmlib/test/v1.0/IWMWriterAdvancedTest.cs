// Two of the methods here (GetStatistics & GetStatistics) are actually tested over in IWMWriter, since
// they require a fully configured file, and I didn't want to dupe it all here.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

using WindowsMediaLib;
using WindowsMediaLib.Defs;
using System.Runtime.InteropServices.ComTypes;

namespace v1._0
{
    public class IWMWriterAdvancedTest
    {
        private const string sFileName = @"c:\WmTestOut.wmv";
        private IWMWriterAdvanced m_Writer;

        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

        public void DoTests()
        {
            Config();

            TestSink();
            TestLive();
            TestTolerance();
            TestMisc();
        }

        private void TestMisc()
        {
            long l;

            m_Writer.GetWriterTime(out l);
            Debug.Assert(l == 0);
        }

        private void TestTolerance()
        {
            int i;

            m_Writer.SetSyncTolerance(1211);
            m_Writer.GetSyncTolerance(out i);
            Debug.Assert(i == 1211);
        }

        private void TestLive()
        {
            bool b;
            m_Writer.IsRealTime(out b);

            Debug.Assert(b == false);

            m_Writer.SetLiveSource(true);
        }

        private void TestSink()
        {
            int i;
            IWMWriterFileSink pSink;
            IWMWriterSink pSink2;

            WMUtils.WMCreateWriterFileSink(out pSink);

            m_Writer.AddSink(pSink);
            m_Writer.GetSinkCount(out i);
            Debug.Assert(i == 1);

            m_Writer.GetSink(0, out pSink2);
            Debug.Assert(pSink2 != null);

            m_Writer.RemoveSink(pSink2);
            m_Writer.GetSinkCount(out i);
            Debug.Assert(i == 0);
        }

        private void Config()
        {
            IWMWriter pWriter;
            WMUtils.WMCreateWriter(IntPtr.Zero, out pWriter);

            m_Writer = pWriter as IWMWriterAdvanced;
        }
    }
}
