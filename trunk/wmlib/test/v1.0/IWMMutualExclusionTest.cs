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
    public class IWMMutualExclusionTest
    {
        private IWMMutualExclusion m_pME;

        public void DoTests()
        {
            Config();

            TestType();
        }

        private void TestType()
        {
            Guid g;

            m_pME.SetType(MutexType.Bitrate);
            m_pME.GetType(out g);

            Debug.Assert(g == MutexType.Bitrate);
        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager = null;
            IWMProfile pWMProfile = null;

            // Open the profile manager
            WMUtils.WMCreateProfileManager(out pWMProfileManager);

            pWMProfileManager.CreateEmptyProfile(WMVersion.V9_0, out pWMProfile);
            pWMProfile.CreateNewMutualExclusion(out m_pME);
        }
    }
}
