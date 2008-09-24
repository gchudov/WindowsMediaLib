using System;
using System.Collections.Generic;
using System.Text;
using WindowsMediaLib;
using System.Diagnostics;

namespace v1._0
{
    public class IWMAddressAccess2Test
    {
        private IWMAddressAccess2 m_pAddressAccess;

        public void DoTests()
        {
            Config();

            TestAdd(AEType.Include);
            TestAdd(AEType.Exclude);
        }

        private void TestAdd(AEType aeType)
        {
            const string ipv4 = "192.168.0.1";
            const string maskv4 = "255.255.255.255";
            const string localipv6 = "::1";
            const string maskv6 = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";

            string outip, outmask;
            int initialEntries = 0;
            int numEntries = 0;

            // remove all existing entries first
            m_pAddressAccess.GetAccessEntryCount(aeType, out initialEntries);
            for (int i = initialEntries - 1; i >= 0; i--)
            {
                m_pAddressAccess.RemoveAccessEntry(aeType, i);
            }

            // add IPv4 entry
            m_pAddressAccess.AddAccessEntryEx(aeType, ipv4, maskv4);
            m_pAddressAccess.GetAccessEntryCount(aeType, out numEntries);
            Debug.Assert(numEntries == 1);

            m_pAddressAccess.GetAccessEntryEx(aeType, numEntries - 1, out outip, out outmask);
            Debug.Assert(ipv4 == outip);
            Debug.Assert(maskv4 == outmask);

            // remove IPv4 entry
            m_pAddressAccess.RemoveAccessEntry(aeType, numEntries - 1);
            m_pAddressAccess.GetAccessEntryCount(aeType, out numEntries);
            Debug.Assert(numEntries == 0);

            // add IPv6 entry
            m_pAddressAccess.AddAccessEntryEx(aeType, localipv6, maskv6);
            m_pAddressAccess.GetAccessEntryCount(aeType, out numEntries);
            Debug.Assert(numEntries == 1);

            m_pAddressAccess.GetAccessEntryEx(aeType, numEntries - 1, out outip, out outmask);
            Debug.Assert(localipv6 == outip);
            Debug.Assert(maskv6 == outmask);
        }

        private void Config()
        {
            IWMWriterNetworkSink pWMWriterNetworkSink = null;

            WMUtils.WMCreateWriterNetworkSink(out pWMWriterNetworkSink);
            m_pAddressAccess = (IWMAddressAccess2)pWMWriterNetworkSink;
        }
    }
}
