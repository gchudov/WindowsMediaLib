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
    public class IWMWriterPreprocessTest
    {
        private const string sFileName = @"c:\WmTestOut.wmv";
        private IWMWriterPreprocess m_preproc;
        IWMWriter m_pWriter;

        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);

        public void DoTests()
        {
            Config();

            TestPreProc();

        }

        private void TestPreProc()
        {
            int i;
            const int iUseSize = 126720;
            TempBuff t = new TempBuff(iUseSize);

            m_preproc.GetMaxPreprocessingPasses(0, 0, out i);
            m_preproc.SetNumPreprocessingPasses(0, 0, i);
            m_preproc.BeginPreprocessingPass(0, 0);

            t.SetLength(iUseSize);
            m_preproc.PreprocessSample(0, 0, 0, t);
            m_preproc.EndPreprocessingPass(0, 0);
        }

        private void Config()
        {

            WMUtils.WMCreateWriter(IntPtr.Zero, out m_pWriter);
            m_pWriter.SetProfileByID(g);
            m_pWriter.BeginWriting();

            m_preproc = m_pWriter as IWMWriterPreprocess;
        }
    }
}
