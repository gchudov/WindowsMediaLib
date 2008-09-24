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
    public class IWMSyncReaderTest : IWMStatusCallback, IStream
    {
        const string sFileName = @"c:\so_lesson3c.wmv";

        const long Sec = 10000000;
        private IWMSyncReader m_read;
        bool m_IndexComplete;
        BinaryReader m_br;

        public void DoTests()
        {
            m_br = new BinaryReader(new FileStream(sFileName, FileMode.Open, FileAccess.Read));

            m_IndexComplete = false;
            Config();

            TestOpen();
            TestSelected();
            TestReadSamples();
            TestProps();
            TestSetting();
            TestFormat();
            TestSample();
            TestMisc();
            TestClose();

            m_br.Close();
        }

        private void TestFormat()
        {
            int i;
            IWMOutputMediaProps iProp;

            m_read.GetOutputFormatCount(1, out i);
            Debug.Assert(i > 0);

            m_read.GetOutputFormat(1, 1, out iProp);
            Debug.Assert(iProp != null);
        }

        private void TestMisc()
        {
            IWMIndexer2 wi2 = m_read as IWMIndexer2;
            m_read.SetRange(100, 200);
            m_read.SetRangeByFrame(1, 1, 0);

            int on;
            short sn;
            m_read.GetOutputNumberForStream(2, out on);
            m_read.GetStreamNumberForOutput(on, out sn);

            Debug.Assert(sn == 2);

            m_read.OpenStream(this);
            Debug.WriteLine("here");
        }

        private void TestSample()
        {
            int iSize;

            m_read.GetMaxOutputSampleSize(0, out iSize);
            Debug.Assert(iSize > 0);

            INSSBuffer pSamp;
            long l;
            long d;
            SampleFlag f;
            int i;
            short s;
            m_read.GetNextSample(1, out pSamp, out l, out d, out f, out i, out s);

            Debug.Assert(pSamp != null);
            Debug.Assert(d > 0);
            Debug.Assert(s == 1);

            int iMax;
            m_read.GetMaxStreamSampleSize(1, out iMax);
            Debug.Assert(iMax > 0);
        }

        private void TestSetting()
        {
            AttrDataType pType;
            int v = 12345;
            short w = 0;
            byte[] b1 = BitConverter.GetBytes(v);

            m_read.SetOutputSetting(1, Constants.g_wszEarlyDataDelivery, AttrDataType.DWORD, b1, (short)b1.Length);
            m_read.GetOutputSetting(1, Constants.g_wszEarlyDataDelivery, out pType, null, ref w);

            byte[] b = new byte[w];
            m_read.GetOutputSetting(1, Constants.g_wszEarlyDataDelivery, out pType, b, ref w);

            Debug.Assert(v == BitConverter.ToInt32(b, 0));
        }

        private void TestOpen()
        {
            m_read.Open(sFileName);
        }

        private void TestClose()
        {
            m_read.Close();
        }

        private void TestSelected()
        {
            int i;

            m_read.GetOutputCount(out i);

            short[] pn = new short[2];
            StreamSelection[] ps1 = new StreamSelection[2];
            StreamSelection[] ps2 = new StreamSelection[2];

            ps1[0] = StreamSelection.CleanPointOnly;
            ps1[1] = StreamSelection.On;

            pn[0] = 1;
            pn[1] = 2;

            m_read.SetStreamsSelected(2, pn, ps1);

            m_read.GetStreamSelected(1, out ps2[0]);
            m_read.GetStreamSelected(2, out ps2[1]);
        }

        private void TestReadSamples()
        {
            bool b;

            m_read.SetReadStreamSamples(1, true);
            m_read.GetReadStreamSamples(1, out b);

            Debug.Assert(b);
        }

        private void TestProps()
        {
            IWMOutputMediaProps p;

            m_read.GetOutputProps(1, out p);
            Debug.Assert(p != null);
            m_read.SetOutputProps(1, p);
        }

        private void Config()
        {
            IWMIndexer pIndex;
            WMUtils.WMCreateIndexer(out pIndex);
            IWMIndexer2 pIndex2 = pIndex as IWMIndexer2;

            pIndex2.Configure(0, IndexerType.FrameNumbers, null, null);

            pIndex2.StartIndexing(sFileName, this, IntPtr.Zero);
            while (!m_IndexComplete)
            {
                System.Threading.Thread.Sleep(0);
            }

            WMUtils.WMCreateSyncReader(IntPtr.Zero, Rights.Playback, out m_read);
        }

        #region IWMStatusCallback Members

        public void OnStatus(Status Status, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            Debug.WriteLine(Status);
            if (Status == Status.Closed)
            {
                m_IndexComplete = true;
            }
        }

        #endregion

        #region IStream Members

        public void Clone(out IStream ppstm)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Commit(int grfCommitFlags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void LockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            int i = m_br.Read(pv, 0, cb);
            if (pcbRead != IntPtr.Zero)
            {
                Marshal.WriteInt32(pcbRead, i);
            }
        }

        public void Revert()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            long l = m_br.BaseStream.Seek(dlibMove, (SeekOrigin)dwOrigin);
            if (plibNewPosition != IntPtr.Zero)
            {
                Marshal.WriteInt64(plibNewPosition, l);
            }
        }

        public void SetSize(long libNewSize)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new System.Runtime.InteropServices.ComTypes.STATSTG();
            pstatstg.cbSize = m_br.BaseStream.Length;
            //throw new Exception("The method or operation is not implemented.");
        }

        public void UnlockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
