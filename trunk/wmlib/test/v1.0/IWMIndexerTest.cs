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
    public class IWMIndexerTest : IWMStatusCallback
    {
        private const string sFileName = @"c:\so_lesson3c.wmv";
        private bool m_IndexComplete;
        private int m_IndexError;
        private int m_MaxIndex;

        public void DoTests()
        {
            DoIndex(IndexerType.FrameNumbers, false);
            Debug.Assert(m_MaxIndex == 100);
            DoIndex(IndexerType.FrameNumbers, true);
            Debug.Assert(m_MaxIndex < 100);
        }

        private void DoIndex(IndexerType it, bool b)
        {
            IWMIndexer pIndex;
            WMUtils.WMCreateIndexer(out pIndex);
            IWMIndexer2 pIndex2 = pIndex as IWMIndexer2;
            pIndex2.Configure(0, it, null, null);

            m_IndexComplete = false;
            m_MaxIndex = 0;
            m_IndexError = 0;
            pIndex2.StartIndexing(sFileName, this, IntPtr.Zero);
            while (!m_IndexComplete)
            {
                if (b)
                {
                    pIndex.Cancel();
                }
                else
                {
                    System.Threading.Thread.Sleep(0);
                }
            }
            if (m_IndexError != 0)
                throw new COMException("Indexing error", m_IndexError);
        }

        #region IWMStatusCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            Debug.WriteLine(string.Format("{0} {1} {2} {3}", iStatus, hr, dwType, Marshal.ReadInt32(pValue)));
            switch (iStatus)
            {
                case Status.Closed:
                    {
                        m_IndexComplete = true;
                        break;
                    }
                case Status.Error:
                    {
                        m_IndexError = hr;
                        break;
                    }
                case Status.IndexProgress:
                    m_MaxIndex = Marshal.ReadInt32(pValue);
                    break;
            }
        }

        #endregion

    }
}
