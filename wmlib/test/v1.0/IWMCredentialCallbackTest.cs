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
    public class IWMCredentialCallbackTest : IWMReaderCallback, IWMCredentialCallback
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        IWMReader m_read;
        private Status m_LastStatus;
        private int m_RequestCount;

        public void DoTests()
        {
            m_RequestCount = 0;

            Config();
        }

        private void Config()
        {
            WMUtils.WMCreateReader(IntPtr.Zero, 0, out m_read);
            m_read.Open("http://192.168.1.4/league/so_lesson3c.wmv", this, IntPtr.Zero);

            while (m_RequestCount < 2 || m_LastStatus != Status.Opened)
                System.Threading.Thread.Sleep(1);
        }

        #region IWMReaderCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            Debug.Write(string.Format("Status: {0} 0x{1:x} {2} {3} {4} {5} ", iStatus, hr, WMError.GetErrorText(hr), dwType, pValue.ToInt32(), pvContext.ToInt32()));

            m_LastStatus = iStatus;

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

        public void OnSample(int dwOutputNum, long cnsSampleTime, long cnsSampleDuration, SampleFlag dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
        }

        #endregion

        #region IWMCredentialCallback Members

        public void AcquireCredentials(string pwszRealm, string pwszSite, char[] pwszUser, int cchUser, char[] pwszPassword, int cchPassword, int hrStatus, ref CredentialFlags pdwFlags)
        {
            const string user = "david\0";
            string[] pw = { "foo\0", "Moo\0" };

            // If we are being called a second time (ie the first uid & pw were wrong), the 
            // old values might be sent back to us.

            int t = Array.IndexOf(pwszUser, '\0'); // Look for the trailing null since
            string u = new string(pwszUser, 0, t); // the string construct is too dumb to look for it.

            t = Array.IndexOf(pwszPassword, '\0');
            string p = new string(pwszPassword, 0, t);

            Debug.WriteLine(string.Format("uid: {0}  pw: {1}", u, p));

            char[] uc = user.ToCharArray(); // Doesn't include a trailing null by default
            Array.Copy(uc, pwszUser, uc.Length);  // MUST use copy!  Don't just assign into pwszUser

            char[] pc = pw[m_RequestCount].ToCharArray();
            Array.Copy(pc, pwszPassword, pc.Length);

            m_RequestCount++;
            pdwFlags = CredentialFlags.ClearText; // Tell the caller what we're sending back

        }

        #endregion
    }
}
