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
    public class IWMWriterAdvanced3Test
    {
        private const string sFileName = @"c:\WmTestOut.wmv";
        private IWMWriter m_Writer;
        private IWMWriterAdvanced3 m_wa3;

        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);

        public void DoTests()
        {
            Config();

            Test();
        }

        private void Test()
        {
            WMWriterStatisticsEx w = new WMWriterStatisticsEx();

            m_wa3.SetNonBlocking();
            m_Writer.BeginWriting();
            m_wa3.GetStatisticsEx(0, out w);
        }

        private void Config()
        {
            WMUtils.WMCreateWriter(IntPtr.Zero, out m_Writer);
            m_Writer.SetProfileByID(g);
            m_Writer.SetOutputFilename(sFileName);
            m_wa3 = m_Writer as IWMWriterAdvanced3;
        }
    }
}
