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
    public class IWMWriterFileSink2Test
    {
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);
        private const string sFileName = @"c:\WmTestOut.wmv";
        IWMWriterFileSink2 m_pSink;

        public void DoTests()
        {
            Config();

            TestOpen();
            Test();
        }

        private void TestOpen()
        {
            IWMWriterAdvanced wa;
            IWMWriter pw;

            WMUtils.WMCreateWriter(IntPtr.Zero, out pw);
            wa = pw as IWMWriterAdvanced;

            wa.AddSink(m_pSink);

            m_pSink.Open(sFileName);

            pw.SetProfileByID(g);

            pw.BeginWriting();
        }

        private void Test()
        {
            bool b;

            m_pSink.IsStopped(out b);
            Debug.Assert(!b);

            m_pSink.Stop(0);

            m_pSink.IsStopped(out b);
            Debug.Assert(b);

            m_pSink.Start(0);

            long lDur, lSize;
            m_pSink.GetFileDuration(out lDur);
            m_pSink.GetFileSize(out lSize);

            Debug.Assert(lDur == 0); // no sample written
            Debug.Assert(lSize > 0); // But we did write a header

            m_pSink.IsClosed(out b);
            Debug.Assert(!b);

            m_pSink.Close();

            m_pSink.IsClosed(out b);
            Debug.Assert(b);
        }

        private void Config()
        {
            IWMWriterFileSink pSink;
            WMUtils.WMCreateWriterFileSink(out pSink);

            m_pSink = pSink as IWMWriterFileSink2;
        }
    }
}
