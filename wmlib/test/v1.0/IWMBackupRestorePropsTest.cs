using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    public class IWMBackupRestorePropsTest : IWMStatusCallback
    {
        private IWMBackupRestoreProps m_pIWMBackupRestoreProps;

       public void DoTests()
        {
            Config();

            TestUnimpl();
            TestProps();
        }

        private void TestUnimpl()
        {
            AttrDataType pType;
            bool bPassed = true;
            short i = 4;
            byte[] pValue;
            short pcbLength = 0;
            short pcProps;

            try
            {
                m_pIWMBackupRestoreProps.GetPropByIndex(0, "asdf", ref i, out pType, out pValue, ref pcbLength);
                bPassed = false;
            }
            catch (System.NotImplementedException)
            {
            }
            catch
            {
                Debug.Assert(false);
            }

            try
            {
                m_pIWMBackupRestoreProps.GetPropByName("asdf", out pType, out pValue, ref pcbLength);
                bPassed = false;
            }
            catch (System.NotImplementedException)
            {
            }
            catch
            {
                Debug.Assert(false);
            }

            try
            {
                m_pIWMBackupRestoreProps.GetPropCount(out pcProps);
                bPassed = false;
            }
            catch (System.NotImplementedException)
            {
            }
            catch
            {
                Debug.Assert(false);
            }

            Debug.Assert(bPassed);

        }

        private void TestProps()
        {
            byte [] b = Encoding.Unicode.GetBytes(@"c:\");
            Array.Resize(ref b, b.Length + 2);

            m_pIWMBackupRestoreProps.SetProp("BackupPath", AttrDataType.STRING, b, (short)b.Length);
            m_pIWMBackupRestoreProps.RemoveProp("BackupPath");
            m_pIWMBackupRestoreProps.RemoveAllProps();
        }

        private void Config()
        {
            IWMLicenseBackup pLB = null;

            WMUtils.WMCreateBackupRestorer(this, out pLB);
            m_pIWMBackupRestoreProps = (IWMBackupRestoreProps)pLB;
        }

        #region IWMStatusCallback Members

        void IWMStatusCallback.OnStatus(Status Status, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
