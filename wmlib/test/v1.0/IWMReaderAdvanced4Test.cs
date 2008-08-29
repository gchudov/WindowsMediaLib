using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.IO;

using WindowsMediaLib;

namespace v1._0
{
    public class IWMReaderAdvanced4Test : IWMReaderCallback
    {
        //const string sFileName = @"http://85.25.144.234:3030";
        const string sFileName = @"c:\so_lesson3c.wmv";
        IWMReaderAdvanced4 m_read;
        private bool m_Opened;
        private bool m_CallbackCalled;

        public void DoTests()
        {
            Config();

            TestMisc();
            DoOpen(m_read as IWMReader);
            //DoStart(m_read);

            TestLanguage();
            TestSaveAs();
            TestLog();
        }

        private void TestLanguage()
        {
            short i, len;
            m_read.GetLanguageCount(0, out i);

            Debug.Assert(i > 0);

            len = 0;
            StringBuilder sb = null;

            m_read.GetLanguage(0, 0, sb, ref len);
            sb = new StringBuilder(len);
            m_read.GetLanguage(0, 0, sb, ref len);
            Debug.Assert(sb.ToString() == "en-us");
        }

        private void TestLog()
        {
            m_read.SendLogParams();
            m_read.AddLogParam("foo", "bar", "moo");
        }

        private void TestSaveAs()
        {
            StringBuilder sb = null;
            bool b;
            int i = 0;

            m_read.CanSaveFileAs(out b);
            m_read.CancelSaveFileAs();

            m_read.GetURL(sb, ref i);
            sb = new StringBuilder(i);
            m_read.GetURL(sb, ref i);
            Debug.Assert(sb.ToString() == sFileName);
        }

        private void TestMisc()
        {
            double d;
            bool b;

            m_read.GetMaxSpeedFactor(out d);
            Debug.Assert(d == 0);

            m_read.IsUsingFastCache(out b);
        }

        private void Config()
        {
            IWMReader read;

            WMUtils.WMCreateReader(IntPtr.Zero, 0, out read);

            m_read = read as IWMReaderAdvanced4;
        }

        private void DoOpen(IWMReader read)
        {
            m_Opened = false;
            read.Open(sFileName, this, new IntPtr(123));

            while (!m_Opened)
            {
                System.Threading.Thread.Sleep(0);
            }
        }

        private void DoStart(IWMReader read)
        {
            read.Start(0, 0, 1.0f, IntPtr.Zero);
            m_CallbackCalled = false;
            while (!m_CallbackCalled)
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

        public void OnSample(int dwOutputNum, long cnsSampleTime, long cnsSampleDuration, WM_SF dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
            m_CallbackCalled = true;
        }

        #endregion
    }
}
