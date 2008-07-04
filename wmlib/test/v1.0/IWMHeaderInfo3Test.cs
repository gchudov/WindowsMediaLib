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
            m_head.AddAttribute(0, "asdf", out iIndex, AttrDataType.STRING, 0, b, b.Length);

            m_head.GetAttributeCountEx(0, out w2);

            Debug.Assert(w2 == w1 + 1);

            short pNLen = 0;
            int pDLen = 0;
            AttrDataType pType;
            short pLang;

            m_head.GetAttributeByIndexEx(0, w1, null, ref pNLen, out pType, out pLang, null, ref pDLen);
            byte[] bb = new byte[pDLen];
            m_head.GetAttributeByIndexEx(0, w1, null, ref pNLen, out pType, out pLang, bb, ref pDLen);
            string ss = Encoding.Unicode.GetString(bb, 0, pDLen - 2);
            StringBuilder sbName = new StringBuilder(pNLen);
            byte[] bb2 = new byte[pDLen];
            m_head.GetAttributeByIndexEx(0, w1, sbName, ref pNLen, out pType, out pLang, bb2, ref pDLen);

            Debug.Assert(sbName.ToString() == "asdf" && pType == AttrDataType.STRING && pLang == 0 && v == Encoding.Unicode.GetString(bb2, 0, v.Length * 2));

            byte[] wm3 = BitConverter.GetBytes(4144); ;
            m_head.ModifyAttribute(0, w1, AttrDataType.DWORD, 0, wm3, 4);

            byte[] bb4 = new byte[pDLen];
            m_head.GetAttributeByIndexEx(0, w1, null, ref pNLen, out pType, out pLang, bb4, ref pDLen);
            Debug.Assert(BitConverter.ToInt32(bb4, 0) == 4144);

            m_head.DeleteAttribute(0, w1);

            m_head.GetAttributeCountEx(0, out w3);
            Debug.Assert(w3 == w1);
        }

        private void TestIndices()
        {
            short w1;

            m_head.GetAttributeCountEx(0, out w1);

            short pIndex1, pIndex2;
            byte[] wm = BitConverter.GetBytes(4292);

            m_head.AddAttribute(0, "tester", out pIndex1, AttrDataType.DWORD, 0, wm, 4);
            m_head.AddAttribute(0, "tester", out pIndex2, AttrDataType.DWORD, 0, wm, 4);

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
