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
    public class IWMSyncReader2Test : IWMReaderAllocatorEx, IWMStatusCallback
    {
        private const string sFileName = @"c:\so_lesson3c.wmv";
        private bool m_IndexComplete;
        private int m_IndexError;

        private IWMSyncReader2 m_read;

        public void DoTests()
        {
            Config();
            TestAlloc();
            TestStream();
            TestFrameRange();
            TestRange();
        }

        private void TestFrameRange()
        {
            long l;
            m_read.SetRangeByFrameEx(1, 3, 0, out l);
            Debug.Assert(l != 0);
        }

        private void TestRange()
        {
            m_read.Close();
            DoIndex(IndexerType.TimeCode);
            m_read.Open(sFileName);

            TimeCodeExtensionData st, e;
            st = new TimeCodeExtensionData();
            e = new TimeCodeExtensionData();
            st.dwTimecode = 0x300;
            e.dwTimecode = 0x900;

            try
            {
                m_read.SetRangeByTimecode(2, st, e);
            }
            catch (COMException ce)
            {
                // The concensus is that SetRangeByTimecode() doesn't work, even in c++
                if (Marshal.GetHRForException(ce) != NSResults.E_INVALID_REQUEST)
                {
                    throw;
                }
            }
        }

        private void TestStream()
        {
            IWMReaderAllocatorEx pAlloc;

            m_read.SetAllocateForStream(1, this);
            m_read.GetAllocateForStream(1, out pAlloc);

            Debug.Assert(pAlloc != null);
        }

        private void TestAlloc()
        {
            IWMReaderAllocatorEx pAlloc;
            m_read.SetAllocateForOutput(1, this);
            m_read.GetAllocateForOutput(1, out pAlloc);

            Debug.Assert(pAlloc != null);
        }

        private void DoIndex(IndexerType it)
        {
            IWMIndexer pIndex;
            WMUtils.WMCreateIndexer(out pIndex);
            IWMIndexer2 pIndex2 = pIndex as IWMIndexer2;
            pIndex2.Configure(0, it, null, null);

            m_IndexComplete = false;
            m_IndexError = 0;
            pIndex2.StartIndexing(sFileName, this, IntPtr.Zero);
            while (!m_IndexComplete)
            {
                System.Threading.Thread.Sleep(0);
            }
            if (m_IndexError != 0)
                throw new COMException("Indexing error", m_IndexError);
        }

        private void Config()
        {
            DoIndex(IndexerType.FrameNumbers);

            IWMSyncReader read;

            WMUtils.WMCreateSyncReader(IntPtr.Zero, Rights.Playback, out read);
            m_read = read as IWMSyncReader2;
            m_read.Open(sFileName);
        }

        #region IWMStatusCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            Debug.WriteLine(string.Format("{0} {1} {2} {3}", iStatus, hr, dwType, Marshal.ReadInt32(pValue)));
            if (iStatus == Status.Closed)
            {
                m_IndexComplete = true;
            }
            else if (iStatus == Status.Error)
            {
                m_IndexError = hr;
            }
        }

        #endregion

        #region IWMReaderAllocatorEx Members

        public void AllocateForStreamEx(short wStreamNum, int cbBuffer, out INSSBuffer ppBuffer, SampleFlagEx dwFlags, long cnsSampleTime, long cnsSampleDuration, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AllocateForOutputEx(int dwOutputNum, int cbBuffer, out INSSBuffer ppBuffer, SampleFlagEx dwFlags, long cnsSampleTime, long cnsSampleDuration, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
