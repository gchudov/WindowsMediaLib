using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    [ComVisible(true)]
    public class IWMHeaderInfo3Test
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        private IWMHeaderInfo3 m_head;

        public void DoTests()
        {
            Config();

            TestAttr();
            TestIndices();
            TestCodec();
        }

        private void TestAttr()
        {
            short w1, w2, w3;
            short iIndex;
            const string v = "ItzaString";

            m_head.GetAttributeCountEx(0, out w1);

            byte[] b = Encoding.Unicode.GetBytes(v);
            short iSizeWithNull = (short)(b.Length + 2);
            IntPtr ip = Marshal.AllocCoTaskMem(iSizeWithNull);
            Marshal.Copy(b, 0, ip, b.Length);
            Marshal.WriteInt16(ip, b.Length, 0);

            m_head.AddAttribute(0, "asdf", out iIndex, AttrDataType.STRING, 0, ip, iSizeWithNull);

            m_head.GetAttributeCountEx(0, out w2);

            Debug.Assert(w2 == w1 + 1);

            short pNLen = 0;
            int pDLen = 0;
            AttrDataType pType;
            short pLang;

            m_head.GetAttributeByIndexEx(0, w1, null, ref pNLen, out pType, out pLang, IntPtr.Zero, ref pDLen);
            StringBuilder sbName = new StringBuilder(pNLen);
            IntPtr ip2 = Marshal.AllocCoTaskMem(pDLen);
            m_head.GetAttributeByIndexEx(0, w1, sbName, ref pNLen, out pType, out pLang, ip2, ref pDLen);

            Debug.Assert(sbName.ToString() == "asdf" && pType == AttrDataType.STRING && pLang == 0 && v == Marshal.PtrToStringUni(ip2));

            IntPtr ip3 = Marshal.AllocCoTaskMem(4);
            IntPtr ip4 = Marshal.AllocCoTaskMem(4);
            Marshal.WriteInt32(ip3, 4144);
            m_head.ModifyAttribute(0, w1, AttrDataType.DWORD, 0, ip3, 4);

            m_head.GetAttributeByIndexEx(0, w1, null, ref pNLen, out pType, out pLang, ip4, ref pDLen);
            Debug.Assert(Marshal.ReadInt32(ip4) == 4144);

            m_head.DeleteAttribute(0, w1);

            m_head.GetAttributeCountEx(0, out w3);
            Debug.Assert(w3 == w1);
        }

        private void TestIndices()
        {
            short w1;

            m_head.GetAttributeCountEx(0, out w1);

            IntPtr ip = Marshal.AllocCoTaskMem(4);
            Marshal.WriteInt32(ip, 4292);

            short pIndex1, pIndex2;

            m_head.AddAttribute(0, "tester", out pIndex1, AttrDataType.DWORD, 0, ip, 4);
            m_head.AddAttribute(0, "tester", out pIndex2, AttrDataType.DWORD, 0, ip, 4);

            short pCount = 0;
            WmShort ws = new WmShort();
            m_head.GetAttributeIndices(0, "tester", ws, null, ref pCount);
            short [] sa = new short[pCount];
            m_head.GetAttributeIndices(0, "tester", ws, sa, ref pCount);

            Debug.Assert(sa[0] == pIndex1 && sa[1] == pIndex2);
        }

        private void TestCodec()
        {
            int c1, c2;
            m_head.GetCodecInfoCount(out c1);
            m_head.AddCodecInfo("moo", "fooit", CodecInfoType.Unknown, 0, IntPtr.Zero);
            m_head.GetCodecInfoCount(out c2);

            Debug.Assert(c2 == c1 + 1);
        }

        private void Config()
        {
            IWMMetadataEditor pEditor;

            WMUtils.WMCreateEditor(out pEditor);
            pEditor.Open(sFileName);
            m_head = (IWMHeaderInfo3)pEditor;
        }
    }
}
