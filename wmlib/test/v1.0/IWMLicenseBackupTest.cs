using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    [ComVisible(true)]
    public class IWMLicenseBackupTest : IWMStatusCallback
    {
        const string wzBackupDir = @"c:\test";
        private IWMLicenseBackup m_pIWMLicenseBackup;
        private int m_LastResult;
        private const int iAborted = (unchecked((int)0xc00d2768));

        public void DoTests()
        {
            Config();

            TestBackup();
            TestBackup2();
        }

        private void TestBackup()
        {
            m_pIWMLicenseBackup.BackupLicenses(BackupRestoreFlags.OverWrite, this);
            System.Threading.Thread.Sleep(10000);
            Debug.Assert(m_LastResult == 0);
        }

        private void TestBackup2()
        {
            m_pIWMLicenseBackup.BackupLicenses(BackupRestoreFlags.OverWrite, this);
            m_pIWMLicenseBackup.CancelLicenseBackup();
            System.Threading.Thread.Sleep(10000);
            Debug.Assert(m_LastResult == iAborted);
        }

        private void Config()
        {
            IWMBackupRestoreProps m_pIWMBackupRestoreProps;

            WMUtils.WMCreateBackupRestorer((IWMStatusCallback)this, out m_pIWMLicenseBackup);

            m_pIWMBackupRestoreProps = (IWMBackupRestoreProps)m_pIWMLicenseBackup;

            byte [] b = Encoding.Unicode.GetBytes(wzBackupDir);
            Array.Resize(ref b, b.Length + 2);

            m_pIWMBackupRestoreProps.SetProp("BackupPath", AttrDataType.STRING, b, (short)b.Length);
        }

        #region IWMStatusCallback Members

        void IWMStatusCallback.OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            m_LastResult = hr;

            Debug.Write(string.Format("{0} 0x{1:x} {2} {3} {4} {5} ", iStatus, hr, WMError.GetErrorText(hr), dwType, pValue.ToInt32(), pvContext.ToInt32()));

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

        #endregion
    }
}
