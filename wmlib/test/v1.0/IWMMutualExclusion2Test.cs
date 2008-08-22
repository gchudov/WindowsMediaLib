using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

using WindowsMediaLib;
using WindowsMediaLib.Defs;
using System.Runtime.InteropServices.ComTypes;

namespace v1._0
{
    public class IWMMutualExclusion2Test
    {
        private IWMMutualExclusion2 m_pme;

        public void DoTests()
        {
            Config();

            TestName();
            TestRecord();
        }

        private void TestName()
        {
            short iLen = 0;
            StringBuilder sb = null;

            m_pme.SetName("foo");
            m_pme.GetName(sb, ref iLen);
            sb = new StringBuilder(iLen);
            m_pme.GetName(sb, ref iLen);

            Debug.Assert(sb.ToString() == "foo");
        }

        private void TestRecord()
        {
            short iCount, iLen = 0, iALen = 0;
            StringBuilder sb = null;
            short[] na = null;

            m_pme.GetRecordCount(out iCount);
            Debug.Assert(iCount == 0);

            m_pme.AddRecord();
            m_pme.GetRecordCount(out iCount);
            Debug.Assert(iCount == 1);

            m_pme.SetRecordName(0, "foorecord");
            m_pme.GetRecordName(0, sb, ref iLen);
            sb = new StringBuilder(iLen);
            m_pme.GetRecordName(0, sb, ref iLen);
            Debug.Assert(sb.ToString() == "foorecord");

            m_pme.AddStreamForRecord(0, 33);
            m_pme.AddStreamForRecord(0, 34);
            m_pme.GetStreamsForRecord(0, na, ref iALen);
            Debug.Assert(iALen == 2);
            na = new short[iALen];
            m_pme.GetStreamsForRecord(0, na, ref iALen);
            Debug.Assert(na[0] == 33 && na[1] == 34);

            m_pme.RemoveStreamForRecord(0, 34);
            m_pme.GetStreamsForRecord(0, null, ref iALen);
            Debug.Assert(iALen == 1);

            m_pme.RemoveRecord(0);
            m_pme.GetRecordCount(out iCount);
            Debug.Assert(iCount == 0);

        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager = null;
            IWMProfile pWMProfile = null;
            IWMMutualExclusion pME;

            // Open the profile manager
            WMUtils.WMCreateProfileManager(out pWMProfileManager);

            pWMProfileManager.CreateEmptyProfile(WMVersion.V9_0, out pWMProfile);
            pWMProfile.CreateNewMutualExclusion(out pME);

            m_pme = pME as IWMMutualExclusion2;
        }
    }
}
