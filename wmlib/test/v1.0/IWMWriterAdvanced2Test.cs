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
    public class IWMWriterAdvanced2Test
    {
        private IWMWriterAdvanced2 m_Writer;

        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

        public void DoTests()
        {
            Config();

            TestSetting();
        }

        private void TestSetting()
        {
            short iLen = 4;
            AttrDataType pType;
            byte[] b = null;

            m_Writer.SetInputSetting(0, Constants.g_wszDeinterlaceMode, AttrDataType.DWORD, BitConverter.GetBytes(2), 4);
            m_Writer.GetInputSetting(0, Constants.g_wszDeinterlaceMode, out pType, b, ref iLen);
            Debug.Assert(iLen == 4);

            b = new byte[iLen];
            m_Writer.GetInputSetting(0, Constants.g_wszDeinterlaceMode, out pType, b, ref iLen);
            Debug.Assert(2 == BitConverter.ToInt32(b, 0));
        }

        private void Config()
        {
            IWMWriter writer;

            WMUtils.WMCreateWriter(IntPtr.Zero, out writer);
            writer.SetProfileByID(g);
            m_Writer = writer as IWMWriterAdvanced2;
        }
    }
}
