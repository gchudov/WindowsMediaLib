using System;
using System.Collections.Generic;
using System.Text;
using WindowsMediaLib;
using System.Diagnostics;
using System.Threading;

namespace v1._0
{
    public class IWMClientConnections2Test : IWMReaderCallback
    {
        int port = 9999;
        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);
        IWMClientConnections2 m_clientConns;
        private readonly object m_openLock = new object();
        IWMReader reader;
        IWMWriterNetworkSink sink;
        StringBuilder sbUrl;
        IWMWriter writer;

        public IWMClientConnections2Test()
        {
        }

        public void DoTests()
        {
            Config();

            Test();

            CloseConfig();
        }

        private void Test()
        {
            int numClients = -1;
            StringBuilder networkAddr;
            int netAddrLen = 0;
            StringBuilder networkPort;
            int portLen = 0;
            StringBuilder dnsName;
            int dnsLen = 0;

            m_clientConns.GetClientCount(out numClients);
            Debug.Assert(numClients > 0);

            m_clientConns.GetClientInfo(0, null, ref netAddrLen, null, ref portLen, null, ref dnsLen);
            networkAddr = new StringBuilder(netAddrLen);
            networkPort = new StringBuilder(portLen);
            dnsName = new StringBuilder(dnsLen);
            m_clientConns.GetClientInfo(0, networkAddr, ref netAddrLen, networkPort, ref portLen, dnsName, ref dnsLen);
            Debug.Assert(networkAddr.Length > 0);
            Debug.Assert(networkPort.Length > 0);
            Debug.Assert(dnsName.Length > 0);
        }

        private void Config()
        {   
            WMUtils.WMCreateWriter(IntPtr.Zero, out writer);
            writer.SetProfileByID(g);
            
            WMUtils.WMCreateWriterNetworkSink(out sink);
            m_clientConns = (IWMClientConnections2)sink;
            
            IWMWriterAdvanced advWriter = (IWMWriterAdvanced)writer;
            advWriter.AddSink(sink);
            sink.Open(ref port);

            int urlLen = 0;
            sink.GetHostURL(null, ref urlLen);
            sbUrl = new StringBuilder(urlLen);
            sink.GetHostURL(sbUrl, ref urlLen);

            writer.BeginWriting();

            WMUtils.WMCreateReader(IntPtr.Zero, Rights.Playback, out reader);
            reader.Open(sbUrl.ToString(), this, new IntPtr(123));

            lock (m_openLock)
            {
                Monitor.Wait(m_openLock);
            }

            reader.Start(0, 0, 1.0f, new IntPtr(321));

            lock (m_openLock)
            {
                Monitor.Wait(m_openLock);
            }
        }

        private void CloseConfig()
        {
            reader.Stop();
            reader.Close();
            sink.Disconnect();
            writer.EndWriting();
        }

        #region IWMReaderCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            if (hr != 0)
            {
                string s = WMError.GetErrorText(hr);
            }

            if (iStatus == Status.Opened || iStatus == Status.BufferingStart)
            {
                lock (m_openLock)
                {
                    Monitor.PulseAll(m_openLock);
                }
            }
        }

        public void OnSample(int dwOutputNum, long cnsSampleTime, long cnsSampleDuration, WriteFlags dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
            
        }

        #endregion
    }
}
