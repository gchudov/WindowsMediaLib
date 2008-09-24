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
    public class IWMInputMediaPropsTest
    {
        private IWMInputMediaProps m_Props;

        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

        public void DoTests()
        {
            Config();

            TestNames();
        }

        private void TestNames()
        {
            short iLen = 0;
            StringBuilder sb = null;

            m_Props.GetGroupName(sb, ref iLen);
            sb = new StringBuilder(iLen);
            m_Props.GetGroupName(sb, ref iLen);

            Debug.Assert(sb.ToString() == "");

            sb = null;
            iLen = 0;
            m_Props.GetConnectionName(sb, ref iLen);
            sb = new StringBuilder(iLen);
            m_Props.GetConnectionName(sb, ref iLen);

            Debug.Assert(sb.ToString() == "Video");
        }

        private void Config()
        {
            IWMWriter m_Writer;
            WMUtils.WMCreateWriter(IntPtr.Zero, out m_Writer);
            m_Writer.SetProfileByID(g);
            m_Writer.GetInputProps(0, out m_Props);
        }
    }
}
