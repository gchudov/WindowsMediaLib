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
    public class IWMWriterPostViewTest : IWMWriterPostViewCallback
    {
        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

        private IWMWriterPostView m_Writer;

        public void DoTests()
        {
            Config();

            TestReceive();
            TestProps();
            TestAlloc();
            TestFormat();
            TestCB();
        }

        private void TestProps()
        {
            IWMMediaProps pProps;

            m_Writer.GetPostViewProps(1, out pProps);
            m_Writer.SetPostViewProps(1, pProps);
        }

        private void TestReceive()
        {
            bool b;

            m_Writer.SetReceivePostViewSamples(1, true);
            m_Writer.GetReceivePostViewSamples(1, out b);

            Debug.Assert(b);
            m_Writer.SetReceivePostViewSamples(1, !b);
        }

        private void TestAlloc()
        {
            bool b;

            m_Writer.SetAllocateForPostView(1, true);
            m_Writer.GetAllocateForPostView(1, out b);

            Debug.Assert(b);
        }

        private void TestFormat()
        {
            int iCount;
            IWMMediaProps pProps;

            m_Writer.GetPostViewFormatCount(1, out iCount);
            Debug.Assert(iCount > 0);

            m_Writer.GetPostViewFormat(1, 0, out pProps);
            Debug.Assert(pProps != null);
        }

        private void TestCB()
        {
            m_Writer.SetPostViewCallback(this, IntPtr.Zero);
        }

        private void Config()
        {
            IWMWriter pWriter;

            WMUtils.WMCreateWriter(IntPtr.Zero, out pWriter);
            m_Writer = pWriter as IWMWriterPostView;
            pWriter.SetProfileByID(g);
        }

        #region IWMWriterPostViewCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OnPostViewSample(short wStreamNumber, long cnsSampleTime, long cnsSampleDuration, SampleFlag dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AllocateForPostView(short wStreamNum, int cbBuffer, out INSSBuffer ppBuffer, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
