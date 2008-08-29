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
    public class IWMIStreamPropsTest : COMBase, IWMReaderCallback, IStream, IWMIStreamProps
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        IWMReaderAdvanced2 m_read;
        private bool m_Opened;
        BinaryReader m_br;
        private bool m_Called;

        public void DoTests()
        {
            Config();

            TestStream();
        }

        private void TestStream()
        {
            m_br = new BinaryReader(new FileStream(sFileName, FileMode.Open, FileAccess.Read));
            m_Called = false;

            m_read.OpenStream(this, this, IntPtr.Zero);
            System.Threading.Thread.Sleep(5000);
            Debug.Assert(m_Called);
            m_br.Close();
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

        #region IWMIStreamProps Members

        public void GetProperty(StringBuilder pszName, out AttrDataType pType, IntPtr pValue, ref int pdwSize)
        {
            Debug.WriteLine(pszName);
            m_Called = true;

            if (pszName.ToString() == Constants.g_wszReloadIndexOnSeek)
            {
                pType = AttrDataType.BOOL;
                Marshal.WriteInt32(pValue, 1);
            }
            else if (pszName.ToString() == Constants.g_wszStreamNumIndexObjects)
            {
                pType = AttrDataType.DWORD;
                Marshal.WriteInt32(pValue, 1);
            }
            else if (pszName.ToString() == Constants.g_wszFailSeekOnError)
            {
                pType = AttrDataType.BOOL;
                Marshal.WriteInt32(pValue, 1);
            }
            else if (pszName.ToString() == Constants.g_wszPermitSeeksBeyondEndOfStream)
            {
                pType = AttrDataType.BOOL;
                Marshal.WriteInt32(pValue, 1);
            }
            else if (pszName.ToString() == Constants.g_wszUsePacketAtSeekPoint)
            {
                pType = AttrDataType.BOOL;
                Marshal.WriteInt32(pValue, 1);
            }
            else if (pszName.ToString() == Constants.g_wszSourceBufferTime)
            {
                pType = AttrDataType.DWORD;
                Marshal.WriteInt32(pValue, 1);
            }
            else if (pszName.ToString() == Constants.g_wszSourceMaxBytesAtOnce)
            {
                pType = AttrDataType.DWORD;
                Marshal.WriteInt32(pValue, 10000);
            }
            else
            {
                throw new COMException("Unrecognized type passed to IWMIStreamProps", E_Fail);
            }
        }

        #endregion
    }
}
