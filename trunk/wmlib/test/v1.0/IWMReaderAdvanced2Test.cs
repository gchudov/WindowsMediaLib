using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.IO;

using WindowsMediaLib;

namespace v1._0
{
    public class IWMReaderAdvanced2Test : IWMReaderCallback, IStream
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        IWMReaderAdvanced2 m_read;
        private bool m_Opened;
        BinaryReader m_br;

        public void DoTests()
        {
            Config();

            TestPlayMode();
            TestMarker();
            TestOutputSetting();
            TestLogClientID();
            TestMisc();
            TestSaveAs();
            TestStream();

        }

        private void TestStream()
        {
            m_br = new BinaryReader(new FileStream(sFileName, FileMode.Open, FileAccess.Read));

            IWMReader read = m_read as IWMReader;
            read.Close();
            m_read.OpenStream(this, this, IntPtr.Zero);
            m_br.Close();
        }

        private void TestPlayMode()
        {
            PlayMode m;

            m_read.SetPlayMode(PlayMode.Download);
            m_read.GetPlayMode(out m);

            Debug.Assert(m == PlayMode.Download);

            m_read.SetPlayMode(PlayMode.Local);
        }

        private void TestOutputSetting()
        {
            short l = 0;
            AttrDataType pt;
            byte[] pv = null;

            byte[] bb = new byte[4];
            bb[0] = 123;
            m_read.SetOutputSetting(1, Constants.g_wszEarlyDataDelivery, AttrDataType.DWORD, bb, 4);

            m_read.GetOutputSetting(1, Constants.g_wszEarlyDataDelivery, out pt, pv, ref l);
            pv = new byte[l];
            m_read.GetOutputSetting(1, Constants.g_wszEarlyDataDelivery, out pt, pv, ref l);

            Debug.Assert(pv[0] == 123);
        }

        private void TestLogClientID()
        {
            bool b;

            m_read.SetLogClientID(true);
            m_read.GetLogClientID(out b);

            Debug.Assert(b);
        }

        private void TestMisc()
        {
            int p;
            long l, l2;

            m_read.GetBufferProgress(out p, out l);
            m_read.GetDownloadProgress(out p, out l, out l2);

            int j = 0;
            m_read.GetProtocolName(null, ref j);
            StringBuilder sb = new StringBuilder(j);
            m_read.GetProtocolName(sb, ref j);
            Debug.Assert(sb.ToString() == "file");

            m_read.Preroll(123, 456, 1.0f);
            m_read.StopBuffering();
        }

        private void TestMarker()
        {
            IWMHeaderInfo head;
            IWMMetadataEditor pEditor;

            WMUtils.WMCreateEditor(out pEditor);
            pEditor.Open(sFileName);
            head = (IWMHeaderInfo)pEditor;
            head.AddMarker("bar", 12340000);
            pEditor.Flush();
            pEditor.Close();
            DoOpen();
            m_read.StartAtMarker(0, 2000, 1.0f, IntPtr.Zero);
        }

        private void TestSaveAs()
        {
            int p;

            try
            {
                // This method only works with downloads.  I don't know of a file I can
                // use for testing.  Still, seems likely the def is correct
                m_read.SaveFileAs(@"c:\fooit.wmv");
            }
            catch (COMException ce)
            {
                Debug.Assert(Marshal.GetHRForException(ce) == NSResults.E_INVALID_REQUEST);
            }
            m_read.GetSaveAsProgress(out p);
        }

        private void Config()
        {
            IWMReader read;

            WMUtils.WMCreateReader(IntPtr.Zero, 0, out read);

            m_read = read as IWMReaderAdvanced2;
        }

        private void DoOpen()
        {
            IWMReader read = m_read as IWMReader;

            m_Opened = false;
            read.Open(sFileName, this, new IntPtr(123));

            while (!m_Opened)
            {
                System.Threading.Thread.Sleep(0);
            }
        }

        #region IWMReaderCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            Debug.Write(string.Format("Status: {0} 0x{1:x} {2} {3} {4} {5} ", iStatus, hr, WMError.GetErrorText(hr), dwType, pValue.ToInt32(), pvContext.ToInt32()));

            if (iStatus == Status.Opened)
            {
                m_Opened = true;
            }

            switch (dwType)
            {
                case AttrDataType.STRING:
                    Debug.WriteLine(Marshal.PtrToStringUni(pValue));
                    break;
                case AttrDataType.WORD:
                    Debug.WriteLine(Marshal.ReadInt16(pValue));
                    break;
                case AttrDataType.DWORD:
                case AttrDataType.BOOL:
                    Debug.WriteLine(Marshal.ReadInt32(pValue));
                    break;
                case AttrDataType.QWORD:
                    Debug.WriteLine(Marshal.ReadInt64(pValue));
                    break;
                default:
                    Debug.WriteLine("???");
                    break;
            }

        }

        public void OnSample(int dwOutputNum, long cnsSampleTime, long cnsSampleDuration, WM_SF dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
            //throw new Exception("The method or operation is not implemented.");
            if (pvContext.ToInt32() == 567)
            {
                //m_CallbackCalled = true;
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
