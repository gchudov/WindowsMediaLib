using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.IO;

using WindowsMediaLib;
using WindowsMediaLib.Defs;

namespace v1._0
{
    public class IWMStreamConfig2Test
    {
        const string sFileName = @"c:\so_lesson3c.wmv";
        IWMStreamConfig2 m_sc;

        public void DoTests()
        {
            Config();

            TestTransp();
            TestDU();
        }

        private void TestTransp()
        {
            TransportType t;

            m_sc.SetTransportType(TransportType.Reliable);
            m_sc.GetTransportType(out t);

            Debug.Assert(t == TransportType.Reliable);
        }

        private void TestDU()
        {
            short i;
            Guid g = Guid.NewGuid();

            m_sc.GetDataUnitExtensionCount(out i);
            Debug.Assert(i == 0);

            m_sc.AddDataUnitExtension(g, -1, BitConverter.GetBytes(4244), 4);
            m_sc.GetDataUnitExtensionCount(out i);
            Debug.Assert(i == 1);

            Guid g2;
            short s;
            int iLen = 0;
            byte[] b = null;
            m_sc.GetDataUnitExtension(0, out g2, out s, b, ref iLen);

            Debug.Assert(iLen == 4);
            b = new byte[iLen];
            m_sc.GetDataUnitExtension(0, out g2, out s, b, ref iLen);

            Debug.Assert(g2 == g && BitConverter.ToInt32(b, 0) == 4244);

            m_sc.RemoveAllDataUnitExtensions();

            m_sc.GetDataUnitExtensionCount(out i);
            Debug.Assert(i == 0);
        }

        private void Config()
        {
            IWMProfileManager pWMProfileManager;
            IWMProfile pProfile;
            IWMStreamConfig sc;

            WMUtils.WMCreateProfileManager(out pWMProfileManager);
            IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;

            pProfileManager2.CreateEmptyProfile(WMVersion.V8_0, out pProfile);

            pProfile.CreateNewStream(MediaType.Video, out sc);

            m_sc = sc as IWMStreamConfig2;
        }
    }
}
