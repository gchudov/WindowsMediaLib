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
    public class IWMWriterFileSinkTest
    {
        private const string sFileName = @"c:\WmTestOut.wmv";
        IWMWriterFileSink m_pSink;

        public void DoTests()
        {
            Config();

            m_pSink.Open(sFileName);
        }

        private void Config()
        {
            WMUtils.WMCreateWriterFileSink(out m_pSink);
        }
    }
}
