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
    public class IWMBandwidthSharingTest
    {
        private IWMBandwidthSharing m_pBS;

        public void DoTests()
        {
            Config();

            TestType();
            TestBand();
        }

        private void TestBand()
        {
            int br, bw;

            m_pBS.SetBandwidth(101001, 202002);
            m_pBS.GetBandwidth(out br, out bw);

            Debug.Assert(br == 101001 && bw == 202002);
        }

        private void TestType()
        {
            Guid g;

            m_pBS.SetType(BandwidthSharingType.Exclusive);
            m_pBS.GetType(out g);

            Debug.Assert(g == BandwidthSharingType.Exclusive);
        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager = null;
            IWMProfile pWMProfile = null;
            IWMProfile3 pWMProfile3 = null;

            // Open the profile manager
            WMUtils.WMCreateProfileManager(out pWMProfileManager);

            pWMProfileManager.CreateEmptyProfile(WMVersion.V9_0, out pWMProfile);
            pWMProfile3 = pWMProfile as IWMProfile3;
            pWMProfile3.CreateNewBandwidthSharing(out m_pBS);
        }
    }
}
