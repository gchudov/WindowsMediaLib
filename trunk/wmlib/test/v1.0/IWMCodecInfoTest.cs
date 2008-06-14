using System;
using System.Collections.Generic;
using System.Text;
using WindowsMediaLib;
using WindowsMediaLib.Defs;
using System.Diagnostics;

namespace v1._0
{
    public class IWMCodecInfoTest
    {
        private IWMCodecInfo m_pCodecInfo;

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
                    IWMStreamConfig streamConfig;
                    m_pCodecInfo.GetCodecFormat(MediaType.Video, 0, 0, out streamConfig);

                    Guid streamType;
                    streamConfig.GetStreamType(out streamType);
                    Debug.Assert(streamType == MediaType.Video);
                }
            }
        }

        private void Config()
        {
            IWMProfileManager profileManager;
            WMUtils.WMCreateProfileManager(out profileManager);

            m_pCodecInfo = (IWMCodecInfo)profileManager;
        }
    }
}
