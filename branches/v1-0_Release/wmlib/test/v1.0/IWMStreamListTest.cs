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
    public class IWMStreamListTest
    {
        private IWMStreamList m_sl;

        public void DoTests()
        {
            Config();

            TestStreams();
        }

        private void TestStreams()
        {
            short [] sa = null;
            short iCount = 0;

            m_sl.AddStream(13);

            m_sl.GetStreams(sa, ref iCount);
            Debug.Assert(iCount == 1);

            sa = new short[iCount];
            m_sl.GetStreams(sa, ref iCount);
            Debug.Assert(sa[0] == 13);

            m_sl.RemoveStream(13);
            m_sl.GetStreams(null, ref iCount);
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

            m_sl = pME as IWMStreamList;

        }
    }
}
