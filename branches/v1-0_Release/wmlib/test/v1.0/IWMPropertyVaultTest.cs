using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.IO;

using WindowsMediaLib;
using WindowsMediaLib.Defs;

namespace v1._0
{
    public class IWMPropertyVaultTest
    {
        IWMPropertyVault m_pv;
        IWMProfile m_pProfile;

        public void DoTests()
        {
            Config();

            TestProp();
            TestCopy();
        }

        private void TestCopy()
        {
            IWMStreamConfig sc;
            IWMPropertyVault pv2;
            Guid g = Guid.NewGuid();
            byte[] b = null;
            int iLen = 0;
            AttrDataType pType;

            m_pProfile.CreateNewStream(MediaType.Video, out sc);
            pv2 = sc as IWMPropertyVault;

            pv2.SetProperty("moo", AttrDataType.GUID, g.ToByteArray(), 16);

            m_pv.CopyPropertiesFrom(pv2);

            m_pv.GetPropertyByName("moo", out pType, b, ref iLen);
            Debug.Assert(iLen == 16);
            Debug.Assert(pType == AttrDataType.GUID);
            b = new byte[iLen];
            m_pv.GetPropertyByName("moo", out pType, b, ref iLen);

            Debug.Assert(new Guid(b) == g);
        }

        private void TestProp()
        {
            int i, iLen = 0;
            AttrDataType pType;
            byte[] b = null;

            m_pv.GetPropertyCount(out i);
            Debug.Assert(i == 0);

            m_pv.SetProperty("foo", AttrDataType.DWORD, BitConverter.GetBytes(14), 4);

            m_pv.GetPropertyByName("foo", out pType, b, ref iLen);
            Debug.Assert(iLen == 4);
            b = new byte[iLen];
            m_pv.GetPropertyByName("foo", out pType, b, ref iLen);

            Debug.Assert(BitConverter.ToInt32(b, 0) == 14);

            int isb = 0;
            b = null;
            iLen = 0;
            StringBuilder sb = null;
            m_pv.GetPropertyByIndex(0, sb, ref isb, out pType, b, ref iLen);
            Debug.Assert(isb == 4);
            Debug.Assert(iLen == 4);

            sb = new StringBuilder(isb);
            b = new byte[iLen];
            m_pv.GetPropertyByIndex(0, sb, ref isb, out pType, b, ref iLen);

            Debug.Assert(BitConverter.ToInt32(b, 0) == 14);
            Debug.Assert(sb.ToString() == "foo");

            m_pv.Clear();

            m_pv.GetPropertyCount(out i);
            Debug.Assert(i == 0);
        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager;
            IWMStreamConfig sc;

            WMUtils.WMCreateProfileManager(out pWMProfileManager);
            IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;

            pProfileManager2.CreateEmptyProfile(WMVersion.V8_0, out m_pProfile);

            m_pProfile.CreateNewStream(MediaType.Video, out sc);

            m_pv = sc as IWMPropertyVault;
        }
    }
}
