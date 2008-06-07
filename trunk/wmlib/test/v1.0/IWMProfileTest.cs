using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;
using WindowsMediaLib.Defs;

namespace v1._0
{
    class IWMProfileTest
    {
        IWMProfile m_pProfile;

        public void DoTests()
        {
            Config();

            TestVer();
            TestName();
            TestDescription();
            TestStream();
            TestExclusion();
        }

        private void TestVer()
        {
            WMVersion v;

            m_pProfile.GetVersion(out v);
            Debug.Assert(v == WMVersion.V8_0);
        }

        private void TestName()
        {
            StringBuilder sb = null;
            int i = 0;

            m_pProfile.SetName("moo");
            m_pProfile.GetName(sb, ref i);

            sb = new StringBuilder(i);
            m_pProfile.GetName(sb, ref i);
            Debug.Assert(sb.ToString() == "moo");
        }

        private void TestDescription()
        {
            StringBuilder sb = null;
            int i = 0;

            m_pProfile.SetDescription("moo2");
            m_pProfile.GetDescription(sb, ref i);

            sb = new StringBuilder(i);
            m_pProfile.GetDescription(sb, ref i);
            Debug.Assert(sb.ToString() == "moo2");
        }

        private void TestStream()
        {
            int i;
            IWMStreamConfig sc, sc2, sc3;

            m_pProfile.CreateNewStream(MediaType.Video, out sc);
            Debug.Assert(sc != null);

            sc.SetStreamNumber(12);

            m_pProfile.AddStream(sc);
            m_pProfile.GetStreamCount(out i);
            Debug.Assert(i == 1);

            m_pProfile.GetStream(0, out sc2);
            Debug.Assert(sc2 != null);

            m_pProfile.ReconfigStream(sc2);

            m_pProfile.GetStreamByNumber(12, out sc3);
            Debug.Assert(sc3 != null);

            m_pProfile.RemoveStream(sc3);
            m_pProfile.GetStreamCount(out i);
            Debug.Assert(i == 0);

            m_pProfile.AddStream(sc);
            m_pProfile.GetStreamCount(out i);
            Debug.Assert(i == 1);

            m_pProfile.RemoveStreamByNumber(12);
            m_pProfile.GetStreamCount(out i);
            Debug.Assert(i == 0);
        }

        private void TestExclusion()
        {
            int i;
            IWMMutualExclusion pme, pme2;

            m_pProfile.CreateNewMutualExclusion(out pme);
            Debug.Assert(pme != null);

            m_pProfile.AddMutualExclusion(pme);
            m_pProfile.GetMutualExclusionCount(out i);
            Debug.Assert(i == 1);

            m_pProfile.GetMutualExclusion(0, out pme2);
            Debug.Assert(pme2 != null);

            m_pProfile.RemoveMutualExclusion(pme2);
            m_pProfile.GetMutualExclusionCount(out i);
            Debug.Assert(i == 0);
        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager;

            WMUtils.WMCreateProfileManager(out pWMProfileManager);
            IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;
            pProfileManager2.SetSystemProfileVersion(WMVersion.V8_0);
            pProfileManager2.CreateEmptyProfile(WMVersion.V8_0, out m_pProfile);
        }

    }

#if false
        void RemoveMutualExclusion(
            [In] IWMMutualExclusion pME
            );

#endif
}
