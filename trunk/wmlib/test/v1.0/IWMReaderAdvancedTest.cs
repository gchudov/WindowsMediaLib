using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;

namespace v1._0
{
    public class IWMReaderAdvancedTest : IWMReaderCallback, IWMReaderCallbackAdvanced
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        IWMReaderAdvanced m_read;
        private bool m_Opened;

        public void DoTests()
        {
            Config();

            TestClock();
            TestManualStreamSelection();
            TestReceiveSelectionCallbacks();
            TestReceiveStreamSamples();
            TestAllocateForOutput();
            TestAllocateForStream();
            TestMisc();
            TestStreamsSelected();
        }

        private void TestClock()
        {
            bool b;

            m_read.SetUserProvidedClock(true);
            m_read.GetUserProvidedClock(out b);

            Debug.Assert(b);
        }

        private void TestManualStreamSelection()
        {
            bool b;

            m_read.SetManualStreamSelection(true);
            m_read.GetManualStreamSelection(out b);

            Debug.Assert(b);
        }

        private void TestReceiveSelectionCallbacks()
        {
            bool b;

            m_read.SetReceiveSelectionCallbacks(true);
            m_read.GetReceiveSelectionCallbacks(out b);

            Debug.Assert(b);
        }

        private void TestReceiveStreamSamples()
        {
            bool b;

            m_read.SetReceiveStreamSamples(1, true);
            m_read.GetReceiveStreamSamples(1, out b);

            Debug.Assert(b);
        }

        private void TestAllocateForOutput()
        {
            bool b;

            m_read.SetAllocateForOutput(1, true);
            m_read.GetAllocateForOutput(1, out b);

            Debug.Assert(b);
        }

        private void TestAllocateForStream()
        {
            bool b;

            m_read.SetAllocateForStream(1, true);
            m_read.GetAllocateForStream(1, out b);

            Debug.Assert(b);
        }

        private void TestMisc()
        {
            WMReaderStatistics ps = new WMReaderStatistics();
            ps.cbSize = Marshal.SizeOf(typeof(WMReaderStatistics));

            m_read.GetStatistics(ps);
            Debug.Assert(ps.wQuality == 100);

            m_read.NotifyLateDelivery(1000);
            m_read.DeliverTime(1000);

            int i;
            m_read.GetMaxOutputSampleSize(1, out i);

            Debug.Assert(i != 0);

            m_read.GetMaxStreamSampleSize(1, out i);
            Debug.Assert(i != 0);

            WMReaderClientInfo w = new WMReaderClientInfo();
            w.cbSize = Marshal.SizeOf(typeof(WMReaderClientInfo));
            m_read.SetClientInfo(w);
        }

        private void TestStreamsSelected()
        {
            short[] psn = new short[2];
            StreamSelection[] pss = new StreamSelection[2];

            psn[0] = 1;
            psn[1] = 2;

            pss[0] = StreamSelection.CleanPointOnly;
            pss[1] = StreamSelection.Off;

            m_read.SetStreamsSelected(2, psn, pss);

            StreamSelection ps;
            m_read.GetStreamSelected(1, out ps);
            Debug.Assert(ps == StreamSelection.CleanPointOnly);

            m_read.GetStreamSelected(2, out ps);
            Debug.Assert(ps == StreamSelection.Off);
        }

        private void Config()
        {
            IWMReader read;

            WMUtils.WMCreateReader(IntPtr.Zero, 0, out read);

            m_Opened = false;
            read.Open(sFileName, this, new IntPtr(123));
            m_read = read as IWMReaderAdvanced;

            while (!m_Opened)
            {
                System.Threading.Thread.Sleep(0);
            }
        }

        #region IWMReaderCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            Debug.Write(string.Format("Status: {0} 0x{1:x} {2} {3} {4} {5} ", iStatus, hr, WMError.GetErrorText(hr), dwType, pValue.ToInt32(), pvContext.ToInt32()));

            if (iStatus == Status.Opened)
            {
                m_Opened = true;
            }

            switch (dwType)
            {
                case AttrDataType.STRING:
                    Debug.WriteLine(Marshal.PtrToStringUni(pValue));
                    break;
                case AttrDataType.WORD:
                    Debug.WriteLine(Marshal.ReadInt16(pValue));
                    break;
                case AttrDataType.DWORD:
                case AttrDataType.BOOL:
                    Debug.WriteLine(Marshal.ReadInt32(pValue));
                    break;
                case AttrDataType.QWORD:
                    Debug.WriteLine(Marshal.ReadInt64(pValue));
                    break;
                default:
                    Debug.WriteLine("???");
                    break;
            }

        }

        public void OnSample(int dwOutputNum, long cnsSampleTime, long cnsSampleDuration, WriteFlags dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
            //throw new Exception("The method or operation is not implemented.");
            if (pvContext.ToInt32() == 567)
            {
                //m_CallbackCalled = true;
            }
        }

        #endregion


        #region IWMReaderCallbackAdvanced Members

        public void OnStreamSample(short wStreamNum, long cnsSampleTime, long cnsSampleDuration, int dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OnTime(long cnsCurrentTime, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OnStreamSelection(short wStreamCount, short[] pStreamNumbers, StreamSelection[] pSelections, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OnOutputPropsChanged(int dwOutputNum, WindowsMediaLib.Defs.AMMediaType pMediaType, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AllocateForStream(short wStreamNum, int cbBuffer, out INSSBuffer ppBuffer, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AllocateForOutput(int dwOutputNum, int cbBuffer, out INSSBuffer ppBuffer, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
