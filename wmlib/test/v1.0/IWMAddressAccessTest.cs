using System;
using System.Collections.Generic;
using System.Text;
using WindowsMediaLib;
using System.Net;
using System.Diagnostics;

namespace v1._0
{
    public class IWMAddressAccessTest
    {
        private IWMAddressAccess m_pAddressAccess;

        public void DoTests()
        {
            Config();

            TestAdd(AEType.Include);
            TestAdd(AEType.Exclude);
        }

        private void TestAdd(AEType aeType)
        {
            // 192.168.0.1 (0xC0A80001) in host int form
            const int hostip = -1062731775;
            // 255.255.255.255 (0xFFFFFFFF) in int form
            const int mask = -1;
            int initialEntries = 0;
            int numEntries = 0;

            m_pAddressAccess.GetAccessEntryCount(aeType, out initialEntries);
            for (int i = initialEntries - 1; i >= 0; i--)
            {
                m_pAddressAccess.RemoveAccessEntry(aeType, i);
            }
            
            int netip = IPAddress.HostToNetworkOrder((int)hostip);
            
            WMAddressAccessEntry accessEntry = new WMAddressAccessEntry();
            accessEntry.dwIPAddress = netip;
            accessEntry.dwMask = mask;

            m_pAddressAccess.AddAccessEntry(aeType, ref accessEntry);
            m_pAddressAccess.GetAccessEntryCount(aeType, out numEntries);
            Debug.Assert(numEntries == 1);

            WMAddressAccessEntry outEntry;
            m_pAddressAccess.GetAccessEntry(aeType, numEntries - 1, out outEntry);
            Debug.Assert(outEntry.dwIPAddress == accessEntry.dwIPAddress);
            Debug.Assert(outEntry.dwMask == accessEntry.dwMask);

            m_pAddressAccess.RemoveAccessEntry(aeType, numEntries - 1);
            m_pAddressAccess.GetAccessEntryCount(aeType, out numEntries);
            Debug.Assert(numEntries == 0);
        }


        private void Config()
        {
            IWMWriterNetworkSink pWMWriterNetworkSink = null;

            WMUtils.WMCreateWriterNetworkSink(out pWMWriterNetworkSink);
            m_pAddressAccess = (IWMAddressAccess)pWMWriterNetworkSink;
        }
    }
}
