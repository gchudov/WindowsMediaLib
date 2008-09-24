using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.IO;

using WindowsMediaLib;

namespace v1._0
{
    public class IWMWriterPushSinkTest
    {
        IWMWriterPushSink m_pSink;

        public void DoTests()
        {
            Config();

            m_pSink.Connect("http://192.168.1.4/pub", null, false);
            m_pSink.EndSession();

            m_pSink.Connect("http://192.168.1.4/pub2", "http://192.168.1.4/pub", false);
            m_pSink.Disconnect();

        }

        private void Config()
        {
            WMUtils.WMCreateWriterPushSink(out m_pSink);
        }
    }
}
