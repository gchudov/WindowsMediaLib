using System;
using System.Collections.Generic;
using System.Text;
using WindowsMediaLib;
using WindowsMediaLib.Defs;
using System.Diagnostics;

using System.Runtime.InteropServices;

namespace v1._0
{
    public class IWMCodecInfo3Test
    {
        private IWMCodecInfo3 m_pCodecInfo;

        public void DoTests()
        {
            Config();

            Test();
        }

        private void Test()
        {
            int numCodecs = -1;
            m_pCodecInfo.GetCodecInfoCount(MediaType.Audio, out numCodecs);
            Debug.Assert(numCodecs >= 0);

            if (numCodecs > 0)
            {
                for (int i = 0; i < numCodecs; i++)
                {
                    // Start testing IWMCodecInfo3
                    AttrDataType dataType;
                    byte[] value = new byte[] { 2, 0, 0, 0 };
                    byte[] outvalue;
                    int size = 4;

                    // Start testing IWMCodecInfo2
                    int nameLen = -1;
                    m_pCodecInfo.GetCodecName(MediaType.Audio, i, null, ref nameLen);
                    Debug.Assert(nameLen >= 0);

                    StringBuilder sb = new StringBuilder(nameLen);
                    m_pCodecInfo.GetCodecName(MediaType.Audio, i, sb, ref nameLen);
                    Debug.Assert(sb.ToString().Length > 0);

                    Guid mt;
                    int fCount;

                    mt = MediaType.Video;
                    m_pCodecInfo.GetCodecFormatCount(mt, i, out fCount);
                    for (int jjj = 0; jjj < fCount; jjj++)
                    {
                        TryOne(mt, jjj, Constants.g_wszComplexityMax);
                    }

                    if (sb.ToString().Equals("Windows Media Audio 9.2"))
                    {
                        m_pCodecInfo.SetCodecEnumerationSetting(MediaType.Audio, i, Constants.g_wszNumPasses, AttrDataType.DWORD, value, size);
                        size = -1;
                        m_pCodecInfo.GetCodecEnumerationSetting(MediaType.Audio, i, Constants.g_wszNumPasses, out dataType, null, ref size);
                        Debug.Assert(size == 4);

                        outvalue = new byte[size];
                        m_pCodecInfo.GetCodecEnumerationSetting(MediaType.Audio, 0, Constants.g_wszNumPasses, out dataType, outvalue, ref size);
                        Debug.Assert(BitConverter.ToInt32(value, 0) == BitConverter.ToInt32(outvalue, 0));
                    }
                    else if (sb.ToString().Equals("Windows Media Audio Voice 9"))
                    {
                        m_pCodecInfo.GetCodecFormatProp(MediaType.Audio, i, 0, Constants.g_wszSpeechCaps, out dataType, null, ref size);
                        Debug.Assert(size == 4);
                        outvalue = new byte[size];

                        m_pCodecInfo.GetCodecFormatProp(MediaType.Audio, i, 0, Constants.g_wszSpeechCaps, out dataType, outvalue, ref size);

                        int val = BitConverter.ToInt32(outvalue, 0);
                        Debug.Assert(size == 4);
                        Debug.Assert(dataType == AttrDataType.DWORD);
                        Debug.Assert(val > 0);
                    }
                }
            }
        }

        private void TryOne(Guid g, int jjj, string s)
        {
            AttrDataType pType;
            int iSize = 4;
            byte[] b = null;
            try
            {
                m_pCodecInfo.GetCodecProp(g, jjj, s, out pType, b, ref iSize);
                b = new byte[iSize];
                m_pCodecInfo.GetCodecProp(g, jjj, s, out pType, b, ref iSize);
            }
            catch (Exception e)
            {
                int hr = Marshal.GetHRForException(e);
                if (hr != -2147024809)
                {
                    Debug.WriteLine(hr);
                }
            }
        }

        private void Config()
        {
            IWMProfileManager profileManager;
            WMUtils.WMCreateProfileManager(out profileManager);

            IWMProfileManager2 profMan2 = (IWMProfileManager2)profileManager;
            profMan2.SetSystemProfileVersion(WMVersion.V9_0);

            m_pCodecInfo = (IWMCodecInfo3)profileManager;
        }
    }
}
