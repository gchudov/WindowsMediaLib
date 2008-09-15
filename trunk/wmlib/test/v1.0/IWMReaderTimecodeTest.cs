using System;
using System.Collections.Generic;
using System.Text;
using WindowsMediaLib;
using System.Diagnostics;

namespace v1._0
{
    public class IWMReaderTimecodeTest
    {
        IWMReaderTimecode readerTimecode;

        public IWMReaderTimecodeTest()
        {
        }

        public void DoTests()
        {
            Config();

            short rangeCount = -1;
            readerTimecode.GetTimecodeRangeCount(1, out rangeCount);
            Debug.Assert(rangeCount >= 0);
        }

        private void Config()
        {
            IWMSyncReader reader;
            WMUtils.WMCreateSyncReader(IntPtr.Zero, Rights.Playback, out reader);
            readerTimecode = (IWMReaderTimecode)reader;

            reader.Open(@"C:\so_lesson3c.wmv");

            int outputs = -1;
            reader.GetOutputCount(out outputs);
            Debug.Assert(outputs > 0);
        }
    }
}
