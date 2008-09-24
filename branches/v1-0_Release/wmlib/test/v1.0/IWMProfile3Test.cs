using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;
using WindowsMediaLib.Defs;

namespace v1._0
{
    class IWMProfile3Test
    {
        IWMProfile3 m_pProfile3;
        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"

        public void DoTests()
        {
            Config();

            TestStorage();
            TestBandwidth();
            TestPrioritization();
            TestMisc();
        }

        private void TestMisc()
        {
            long p;

            m_pProfile3.GetExpectedPacketCount(1000, out p);

            Debug.Assert(p > 0);
        }

        private void TestStorage()
        {
            StorageFormat sf;

            try
            {
                m_pProfile3.GetStorageFormat(out sf);
            }
            catch
            {
                // per the docs, this method is not implemented
            }
            try
            {
                sf = new StorageFormat();
                m_pProfile3.SetStorageFormat(sf);
            }
            catch
            {
                // per the docs, this method is not implemented
            }
        }

        private void TestBandwidth()
        {
            int i1, i2, i3;
            IWMBandwidthSharing bs, bs2;

            m_pProfile3.GetBandwidthSharingCount(out i1);

            m_pProfile3.CreateNewBandwidthSharing(out bs);
            bs.AddStream(1);
            bs.SetBandwidth(333, 1234);
            m_pProfile3.AddBandwidthSharing(bs);

            m_pProfile3.GetBandwidthSharingCount(out i2);
            Debug.Assert(i2 == i1 + 1);

            int br, bw;
            m_pProfile3.GetBandwidthSharing(0, out bs2);
            bs2.GetBandwidth(out br, out bw);
            Debug.Assert(bw == 1234);

            m_pProfile3.RemoveBandwidthSharing(bs2);

            m_pProfile3.GetBandwidthSharingCount(out i3);
            Debug.Assert(i1 == i3);

        }

        private void TestPrioritization()
        {
            IWMStreamPrioritization sp, sp2;
            WMStreamPrioritizationRecord []spr = new WMStreamPrioritizationRecord[2];

            m_pProfile3.CreateNewStreamPrioritization(out sp);

            spr[0] = new WMStreamPrioritizationRecord();
            spr[1] = new WMStreamPrioritizationRecord();
            spr[0].wStreamNumber = 2;
            spr[0].fMandatory = true;
            spr[1].wStreamNumber = 1;
            spr[1].fMandatory = false;

            sp.SetPriorityRecords(spr, (short)2);
            short pc = 0;
            WMStreamPrioritizationRecord[] pa = null;
            sp.GetPriorityRecords(pa, ref pc);
            pa = new WMStreamPrioritizationRecord[2];
            sp.GetPriorityRecords(pa, ref pc);

            Debug.Assert(pa[1].wStreamNumber == 1);

            m_pProfile3.SetStreamPrioritization(sp);

            m_pProfile3.GetStreamPrioritization(out sp2);
            Debug.Assert(sp2 != null);

            m_pProfile3.RemoveStreamPrioritization();
            m_pProfile3.GetStreamPrioritization(out sp2);
            Debug.Assert(sp2 == null);
        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager;
            IWMProfile pProfile;
            Guid guidProfileID = new Guid("{5B16E74B-4068-45B5-B80E-7BF8C80D2C2F}");

            WMUtils.WMCreateProfileManager(out pWMProfileManager);
            IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;
            pProfileManager2.SetSystemProfileVersion(WMVersion.V8_0);
            pProfileManager2.LoadProfileByID(guidProfileID, out pProfile);

            m_pProfile3 = (IWMProfile3)pProfile;
        }

    }
}
