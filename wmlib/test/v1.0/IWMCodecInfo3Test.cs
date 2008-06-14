using System;
using System.Collections.Generic;
using System.Text;
using WindowsMediaLib;
using WindowsMediaLib.Defs;
using System.Diagnostics;

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
                    byte[] value = new byte[] { 1, 0, 0, 0 };
                    byte[] outvalue;
                    int size = 4;

                    // Start testing IWMCodecInfo2
                    int nameLen = -1;
                    m_pCodecInfo.GetCodecName(MediaType.Audio, i, null, ref nameLen);
                    Debug.Assert(nameLen >= 0);

                    StringBuilder sb = new StringBuilder(nameLen);
                    m_pCodecInfo.GetCodecName(MediaType.Audio, i, sb, ref nameLen);
                    Debug.Assert(sb.ToString().Length > 0);

                    if (sb.ToString().Equals("Windows Media Audio Voice 9"))
                    {
                        //m_pCodecInfo.SetCodecEnumerationSetting(MediaType.Audio, i, Constants.g_wszVBREnabled, AttrDataType.BOOL, value, size);
                        //size = -1;
                        //m_pCodecInfo.GetCodecEnumerationSetting(MediaType.Audio, i, Constants.g_wszVBREnabled, out dataType, null, ref size);
                        //Debug.Assert(size == 4);

                        //outvalue = new byte[size];
                        //m_pCodecInfo.GetCodecEnumerationSetting(MediaType.Audio, 0, Constants.g_wszVBREnabled, out dataType, outvalue, ref size);
                        //Debug.Assert(value == outvalue);

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
