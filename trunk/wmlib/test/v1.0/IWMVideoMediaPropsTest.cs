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
    public class IWMVideoMediaPropsTest
    {
        private IWMVideoMediaProps m_Props;

        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

        public void DoTests()
        {
            Config();

            TestQual();
            TestSpacing();
        }

        private void TestSpacing()
        {
            long l;

            m_Props.SetMaxKeyFrameSpacing(1);
            m_Props.GetMaxKeyFrameSpacing(out l);

            Debug.Assert(l == 1);
        }

        private void TestQual()
        {
            int i;

            m_Props.SetQuality(10);
            m_Props.GetQuality(out i);

            Debug.Assert(i == 10);
        }

        private void Config()
        {
            IWMWriter m_Writer;
            IWMInputMediaProps pProps;

            WMUtils.WMCreateWriter(IntPtr.Zero, out m_Writer);
            m_Writer.SetProfileByID(g);
            m_Writer.GetInputProps(0, out pProps);

            m_Props = pProps as IWMVideoMediaProps;
        }
    }
}
