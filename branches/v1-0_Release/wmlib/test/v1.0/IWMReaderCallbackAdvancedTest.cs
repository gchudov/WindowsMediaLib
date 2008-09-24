using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using WindowsMediaLib;
using WindowsMediaLib.Defs;

namespace v1._0
{
    public class IWMReaderCallbackAdvancedTest : IWMReaderCallback, IWMReaderCallbackAdvanced
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        private bool m_Opened;
        private bool m_AllocateForStream;
        private bool m_OutputPropsChanged;
        private bool m_StreamSelection;
        private int m_loop;

        public void DoTests()
        {
            m_AllocateForStream = false;
            m_OutputPropsChanged = false;

            TestOutputPropsChanged();
            TestAllocateForStream();
            TestOnStreamSelection();
        }

        private void TestOutputPropsChanged()
        {
            IWMReader read;

            m_Opened = false;

            WMUtils.WMCreateReader(IntPtr.Zero, Rights.Playback, out read);

            read.Open(sFileName, this, new IntPtr(123));
            while (!m_Opened)
            {
                System.Threading.Thread.Sleep(0);
            }

            read.Start(0, 0, 1.0f, IntPtr.Zero);
            ChangeProps(read);
            while (!m_OutputPropsChanged)
            {
                System.Threading.Thread.Sleep(0);
            }
            read.Stop();
            read.Close();
        }

        private void TestAllocateForStream()
        {
            IWMReader read;

            m_AllocateForStream = false;
            m_Opened = false;

            WMUtils.WMCreateReader(IntPtr.Zero, Rights.Playback, out read);

            read.Open(sFileName, this, new IntPtr(123));
            while (!m_Opened)
            {
                System.Threading.Thread.Sleep(0);
            }

            IWMReaderAdvanced ra = read as IWMReaderAdvanced;
            ra.SetAllocateForStream(1, true);
            ra.SetReceiveStreamSamples(1, true);

            read.Start(0, 0, 1.0f, IntPtr.Zero);
            while (!m_AllocateForStream)
            {
                System.Threading.Thread.Sleep(0);
            }

            read.Stop();
            read.Close();

        }

        private void TestOnStreamSelection()
        {
            IWMReader read;

            m_StreamSelection = false;
            m_Opened = false;

            WMUtils.WMCreateReader(IntPtr.Zero, Rights.Playback, out read);
            IWMReaderAdvanced ra = read as IWMReaderAdvanced;
            ra.SetReceiveSelectionCallbacks(true);

            read.Open(sFileName, this, new IntPtr(123));
            while (!m_Opened)
            {
                System.Threading.Thread.Sleep(0);
            }

            ChangeSelected(ra);
            while (!m_StreamSelection)
            {
                System.Threading.Thread.Sleep(1);
            }
            read.Stop();
            read.Close();

        }

        private void ChangeSelected(IWMReaderAdvanced ra)
        {
            short[] ss = new short[2];
            ss[0] = 1;
            ss[1] = 2;

            StreamSelection[] sss = new StreamSelection[2];

            if (m_loop == 0)
            {
                sss[0] = StreamSelection.Off;
                sss[1] = StreamSelection.On;
                m_loop = 1;
            }
            else
            {
                sss[1] = StreamSelection.Off;
                sss[0] = StreamSelection.On;
                m_loop = 0;
            }
            ra.SetStreamsSelected(2, ss, sss);

        }

        private void ChangeProps(IWMReader read)
        {
            IWMOutputMediaProps pProps;

            read.GetOutputProps(0, out pProps);
            int iLen = 0;
            AMMediaType pType = null;
            pProps.GetMediaType(pType, ref iLen);
            pType = new AMMediaType();
            pType.formatSize = iLen;
            pProps.GetMediaType(pType, ref iLen);

            pType.temporalCompression = true;
            pType.formatSize = 20;
            //pType.subType = MediaSubType.WAVE;
            pProps.SetMediaType(pType);
            read.SetOutputProps(0, pProps);
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
                    int i = Marshal.ReadInt32(pValue);
                    Debug.WriteLine(i);
                    break;
                case AttrDataType.QWORD:
                    Debug.WriteLine(Marshal.ReadInt64(pValue));
                    break;
                default:
                    Debug.WriteLine("???");
                    break;
            }

        }

        public void OnSample(int dwOutputNum, long cnsSampleTime, long cnsSampleDuration, SampleFlag dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
        }

        #endregion

        #region IWMReaderCallbackAdvanced Members

        public void OnStreamSample(short wStreamNum, long cnsSampleTime, long cnsSampleDuration, SampleFlag dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
            Debug.WriteLine("OnStreamSample");
            // wmvcopy
        }

        public void OnTime(long cnsCurrentTime, IntPtr pvContext)
        {
            Debug.WriteLine("OnStreamSample");
            // wmvcopy
        }

        public void OnStreamSelection(short wStreamCount, short[] pStreamNumbers, StreamSelection[] pSelections, IntPtr pvContext)
        {
            m_StreamSelection = true;
            Debug.WriteLine("OnStreamSample");
        }

        public void OnOutputPropsChanged(int dwOutputNum, AMMediaType pMediaType, IntPtr pvContext)
        {
            m_OutputPropsChanged = true;
            Debug.WriteLine("OnOutputPropsChanged");
        }

        public void AllocateForStream(short wStreamNum, int cbBuffer, out INSSBuffer ppBuffer, IntPtr pvContext)
        {
            m_AllocateForStream = true;
            Debug.WriteLine("OnStreamSample");
            TempBuff t = new TempBuff(cbBuffer);
            ppBuffer = t as INSSBuffer;
        }

        public void AllocateForOutput(int dwOutputNum, int cbBuffer, out INSSBuffer ppBuffer, IntPtr pvContext)
        {
            Debug.WriteLine("OnStreamSample");
            ppBuffer = null;
            // audioplayer
        }

        #endregion
    }
}
