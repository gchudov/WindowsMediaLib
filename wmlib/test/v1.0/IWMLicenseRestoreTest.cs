using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    [ComVisible(true)]
    public class IWMLicenseRestoreTest : IWMStatusCallback
    {
        const string wzBackupDir = @"c:\test";
        private IWMLicenseRestore m_pIWMLicenseRestore;
        private int m_LastResult;
        private const int iAborted = (unchecked((int)0xc00d2768));

        public void DoTests()
        {
            Config();

            TestRestore();
            TestRestore2();
        }

        private void TestRestore()
        {
            m_pIWMLicenseRestore.RestoreLicenses(BackupRestoreFlags.None, this);
            System.Threading.Thread.Sleep(10000);
            Debug.Assert(m_LastResult == 0);
        }

        private void TestRestore2()
        {
            m_pIWMLicenseRestore.RestoreLicenses(BackupRestoreFlags.None, this);
            m_pIWMLicenseRestore.CancelLicenseRestore();
            System.Threading.Thread.Sleep(10000);
            Debug.Assert(m_LastResult == iAborted);
        }

        private void Config()
        {
            IWMBackupRestoreProps pIWMBackupRestoreProps;
            IWMLicenseBackup pIWMLicenseBackup;

            WMUtils.WMCreateBackupRestorer(this, out pIWMLicenseBackup);
            m_pIWMLicenseRestore = (IWMLicenseRestore)pIWMLicenseBackup;

            pIWMBackupRestoreProps = (IWMBackupRestoreProps)pIWMLicenseBackup;

            byte[] b = Encoding.Unicode.GetBytes(wzBackupDir);
            Array.Resize(ref b, b.Length + 2);

            pIWMBackupRestoreProps.SetProp("RestorePath", AttrDataType.STRING, b, (short)b.Length);
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
