using System;
using System.Collections.Generic;
using System.Text;
using WindowsMediaLib;
using System.Diagnostics;
using WindowsMediaLib.Defs;

namespace v1._0
{
    public class IWMCodecInfo2Test
    {
        private IWMCodecInfo2 m_pCodecInfo;

        public void DoTests()
        {
            Config();

            Test();
        }

        private void Test()
        {
            int numCodecs = -1;
            m_pCodecInfo.GetCodecInfoCount(MediaType.Video, out numCodecs);
            Debug.Assert(numCodecs >= 0);

            if (numCodecs > 0)
            {
                int numFormats = -1;
                m_pCodecInfo.GetCodecFormatCount(MediaType.Video, 0, out numFormats);
                Debug.Assert(numFormats >= 0);

                if (numFormats > 0)
                {
                    // Start testing IWMCodecInfo2
                    int nameLen = -1;
                    m_pCodecInfo.GetCodecName(MediaType.Video, 0, null, ref nameLen);
                    Debug.Assert(nameLen >= 0);

                    StringBuilder sb = new StringBuilder(nameLen);
                    m_pCodecInfo.GetCodecName(MediaType.Video, 0, sb, ref nameLen);
                    Debug.Assert(sb.ToString().Length > 0);
                    
                    int descLen = -1;
                    IWMStreamConfig streamConfig;
                    m_pCodecInfo.GetCodecFormatDesc(MediaType.Video, 0, 0, out streamConfig, null, ref descLen);
                    Debug.Assert(descLen >= 0);

                    StringBuilder description = new StringBuilder(descLen);                    
                    m_pCodecInfo.GetCodecFormatDesc(MediaType.Video, 0, 0, out streamConfig, description, ref descLen);
                    Debug.Assert(description.ToString().Length >= 0);
                }
            }
        }

        private void Config()
        {
            IWMProfileManager profileManager;
            WMUtils.WMCreateProfileManager(out profileManager);

            m_pCodecInfo = (IWMCodecInfo2)profileManager;
        }
    }
}
