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
    public class IWMReaderAllocatorExTest : IWMReaderAllocatorEx
    {
        private const string sFileName = @"c:\so_lesson3c.wmv";
        private int m_Status;

        private IWMSyncReader2 m_read2;

        public void DoTests()
        {
            Config();

            Test1();
            Test2();

            Debug.Assert(m_Status == 3);
        }

        private void Test1()
        {
            m_read2.Open(sFileName);

            m_read2.SetAllocateForOutput(0, this);

            INSSBuffer pSamp;
            long l;
            long d;
            WriteFlags f;
            int i;
            short s;

            m_read2.GetNextSample(1, out pSamp, out l, out d, out f, out i, out s);
            m_read2.Close();
        }

        private void Test2()
        {
            m_read2.Open(sFileName);

            m_read2.SetReadStreamSamples(1, true);
            m_read2.SetAllocateForStream(1, this);

            INSSBuffer pSamp;
            long l;
            long d;
            WriteFlags f;
            int i;
            short s;

            m_read2.GetNextSample(1, out pSamp, out l, out d, out f, out i, out s);
            m_read2.Close();

        }

        private void Config()
        {
            IWMSyncReader read;
            m_Status = 0;

            WMUtils.WMCreateSyncReader(IntPtr.Zero, Rights.Playback, out read);
            m_read2 = read as IWMSyncReader2;
        }

        #region IWMReaderAllocatorEx Members

        public void AllocateForStreamEx(short wStreamNum, int cbBuffer, out INSSBuffer ppBuffer, WM_SFEX dwFlags, long cnsSampleTime, long cnsSampleDuration, IntPtr pvContext)
        {
            m_Status |= 1;
            TempBuff t = new TempBuff(cbBuffer);
            ppBuffer = t as INSSBuffer;
        }

        public void AllocateForOutputEx(int dwOutputNum, int cbBuffer, out INSSBuffer ppBuffer, WM_SFEX dwFlags, long cnsSampleTime, long cnsSampleDuration, IntPtr pvContext)
        {
            m_Status |= 2;
            TempBuff t = new TempBuff(cbBuffer);
            ppBuffer = t as INSSBuffer;
        }

        #endregion
    }
}
