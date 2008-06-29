using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    [ComVisible(true)]
    public class IWMHeaderInfoTest
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        private IWMHeaderInfo m_head;

        public void DoTests()
        {
            Config();

            TestAttr();
            TestMark();
            TestScript();
        }

        private void TestMark()
        {
            short w1, w2, w3;

            m_head.GetMarkerCount(out w1);

            m_head.AddMarker("mark1", 12340000);

            m_head.GetMarkerCount(out w2);
            Debug.Assert(w2 == w1 + 1);

            long l;
            short pLen = 0;

            m_head.GetMarker((short)(w2 - 1), null, ref pLen, out l);
            StringBuilder sb = new StringBuilder(pLen);
            m_head.GetMarker((short)(w2 - 1), sb, ref pLen, out l);

            Debug.Assert(sb.ToString() == "mark1" && l == 12340000);

            m_head.RemoveMarker((short)(w2 - 1));

            m_head.GetMarkerCount(out w3);
            Debug.Assert(w3 == w1);
        }

        private void TestScript()
        {
            short w1, w2, w3;

            m_head.GetScriptCount(out w1);

            m_head.AddScript("foo", "barism", 0);

            m_head.GetScriptCount(out w2);
            Debug.Assert(w2 == w1 + 1);

            long l;
            short pLen = 0, pCLen = 0;
            m_head.GetScript(w1, null, ref pLen, null, ref pCLen, out l);

            StringBuilder sbType = new StringBuilder(pLen);
            StringBuilder sbCom = new StringBuilder(pCLen);
            m_head.GetScript(w1, sbType, ref pLen, sbCom, ref pCLen, out l);

            m_head.RemoveScript(w1);

            m_head.GetScriptCount(out w3);
            Debug.Assert(w3 == w1);
        }

        private void TestAttr()
        {
            short i1, i2;
            short wStream = 0;

            m_head.GetAttributeCount(wStream, out i1);

            IntPtr ip = Marshal.AllocCoTaskMem(4);

            Marshal.WriteInt32(ip, 1234);
            m_head.SetAttribute(wStream, "asdf", AttrDataType.DWORD, ip, 4);

            Marshal.WriteInt32(ip, 4321);
            m_head.SetAttribute(wStream, "fdsa", AttrDataType.DWORD, ip, 4);

            m_head.GetAttributeCount(wStream, out i2);
            Debug.Assert(i2 - i1 == 2);

            short pLen = 0;
            AttrDataType pType;

            m_head.GetAttributeByName(ref wStream, "asdf", out pType, IntPtr.Zero, ref pLen);
            IntPtr ip2 = Marshal.AllocCoTaskMem(pLen);
            m_head.GetAttributeByName(ref wStream, "asdf", out pType, ip2, ref pLen);
            Debug.Assert(Marshal.ReadInt32(ip2) == 1234);

            short sNameLen = 0;
            m_head.GetAttributeByIndex((short)(i1 + 1), ref wStream, null, ref sNameLen, out pType, IntPtr.Zero, ref pLen);
            StringBuilder sb = new StringBuilder(sNameLen);
            ip2 = Marshal.AllocCoTaskMem(pLen);
            m_head.GetAttributeByIndex((short)(i1 + 1), ref wStream, sb, ref sNameLen, out pType, ip2, ref pLen);

            Debug.Assert(sb.ToString() == "fdsa" && pType == AttrDataType.DWORD && Marshal.ReadInt32(ip) == 4321);
        }

        private void Config()
        {
            IWMMetadataEditor pEditor;

            WMUtils.WMCreateEditor(out pEditor);
            pEditor.Open(sFileName);
            m_head = (IWMHeaderInfo)pEditor;
        }
    }
}
