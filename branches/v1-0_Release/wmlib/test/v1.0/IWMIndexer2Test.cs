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
    public class IWMIndexer2Test : IWMStatusCallback
    {
        private const string sFileName = @"c:\so_lesson3c.wmv";
        private bool m_IndexComplete;
        private int m_IndexError;
        private int m_MaxIndex;

        public void DoTests()
        {
            DoIndex(IndexerType.FrameNumbers, null, null);
            DoIndex(IndexerType.PresentationTime, new WmInt(4000), new WmShort((short)IndexType.NearestDataUnit));

            // Checked it with ASFView.  Looks right to me.
        }

        private void DoIndex(IndexerType it, WmInt w1, WmShort w2)
        {
            IWMIndexer pIndex;
            WMUtils.WMCreateIndexer(out pIndex);
            IWMIndexer2 pIndex2 = pIndex as IWMIndexer2;
            pIndex2.Configure(0, it, w1, w2);

            m_IndexComplete = false;
            m_MaxIndex = 0;
            m_IndexError = 0;
            pIndex2.StartIndexing(sFileName, this, IntPtr.Zero);
            while (!m_IndexComplete)
            {
                System.Threading.Thread.Sleep(0);
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
