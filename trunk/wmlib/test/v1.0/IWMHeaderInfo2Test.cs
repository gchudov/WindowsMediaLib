using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    [ComVisible(true)]
    public class IWMHeaderInfo2Test
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        private IWMHeaderInfo2 m_head;

        public void DoTests()
        {
            Config();

            TestCodec();
        }

        private void TestCodec()
        {
            int i;

            m_head.GetCodecInfoCount(out i);
            Debug.Assert(i > 0);

            CodecInfoType pType;
            short pDLen = 0, pNLen = 0, pCLen = 0;
            m_head.GetCodecInfo(0, ref pNLen, null, ref pDLen, null, out pType, ref pCLen, null);

            StringBuilder sbName = new StringBuilder(pNLen);
            StringBuilder sbDesc = new StringBuilder(pDLen);
            //IntPtr ip = Marshal.AllocCoTaskMem(pCLen);
            byte[] data = new byte[pCLen];
            m_head.GetCodecInfo(0, ref pNLen, sbName, ref pDLen, sbDesc, out pType, ref pCLen, data);

            Debug.WriteLine(string.Format("{0} {1}", sbName.ToString(), sbDesc.ToString()));
        }

        private void Config()
        {
            IWMMetadataEditor pEditor;

            WMUtils.WMCreateEditor(out pEditor);
            pEditor.Open(sFileName);
            m_head = (IWMHeaderInfo2)pEditor;
        }
    }
}
