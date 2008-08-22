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
    public class IWMStreamConfigTest
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        IWMStreamConfig m_sc;

        public void DoTests()
        {
            Config();

            TestBitRate();
            TestWin();
            TestCName();
            TestSName();
            TestStreamNum();
            TestStreamType();
        }

        private void TestBitRate()
        {
            int br;

            m_sc.SetBitrate(123);
            m_sc.GetBitrate(out br);

            Debug.Assert(br == 123);
        }

        private void TestWin()
        {
            int bw;

            m_sc.SetBufferWindow(3000);
            m_sc.GetBufferWindow(out bw);

            Debug.Assert(bw == 3000);
        }

        private void TestCName()
        {
            StringBuilder sb = null;
            short i = 0;

            m_sc.SetConnectionName("foo bar");

            m_sc.GetConnectionName(sb, ref i);
            sb = new StringBuilder(i);
            m_sc.GetConnectionName(sb, ref i);

            Debug.Assert(sb.ToString() == "foo bar");
        }

        private void TestSName()
        {
            StringBuilder sb = null;
            short i = 0;

            m_sc.SetStreamName("foo1 bar");

            m_sc.GetStreamName(sb, ref i);
            sb = new StringBuilder(i);
            m_sc.GetStreamName(sb, ref i);

            Debug.Assert(sb.ToString() == "foo1 bar");
        }

        private void TestStreamNum()
        {
            short sn;

            m_sc.SetStreamNumber(22);
            m_sc.GetStreamNumber(out sn);

            Debug.Assert(sn == 22);
        }

        private void TestStreamType()
        {
            Guid g;

            m_sc.GetStreamType(out g);
            Debug.Assert(g == MediaType.Video);
        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager;
            IWMProfile pProfile;

            WMUtils.WMCreateProfileManager(out pWMProfileManager);
            IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;

            pProfileManager2.CreateEmptyProfile(WMVersion.V8_0, out pProfile);

            pProfile.CreateNewStream(MediaType.Video, out m_sc);
        }
    }
}
