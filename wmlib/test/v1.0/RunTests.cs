using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

using WindowsMediaLib;
using WindowsMediaLib.Defs;  // Contains defs also found in DirectShowLib

namespace v1._0
{
    class Class1
    {
        [MTAThread]
        static void Main(string[] args)
        {
            try
            {
                //INSNetSourceCreatorTest t1 = new INSNetSourceCreatorTest();
                //t1.DoTests();

                //INSSBufferTest t2 = new INSSBufferTest();
                //t2.DoTests();

                //INSSBuffer2Test t3 = new INSSBuffer2Test();
                //t3.DoTests();

                //INSSBuffer3Test t4 = new INSSBuffer3Test();
                //t4.DoTests();

                //INSSBuffer4Test t5 = new INSSBuffer4Test();
                //t5.DoTests();

                //IServiceProviderTest t6 = new IServiceProviderTest();
                //t6.DoTests();

                //IWMAddressAccessTest t7 = new IWMAddressAccessTest();
                //t7.DoTests();

                //IWMAddressAccess2Test t8 = new IWMAddressAccess2Test();
                //t8.DoTests();

                //IWMBackupRestorePropsTest t9 = new IWMBackupRestorePropsTest();
                //t9.DoTests();

                //IWMBandwidthSharingTest t10 = new IWMBandwidthSharingTest();
                //t10.DoTests();

                //IWMClientConnectionsTest t11 = new IWMClientConnectionsTest();
                //t11.DoTests();

                //IWMClientConnections2Test t12 = new IWMClientConnections2Test();
                //t12.DoTests();

                //IWMCodecAMVideoAcceleratorTest t13 = new IWMCodecAMVideoAcceleratorTest();
                //t13.DoTests();

                //IWMCodecInfoTest t14 = new IWMCodecInfoTest();
                //t14.DoTests();

                //IWMCodecInfo2Test t15 = new IWMCodecInfo2Test();
                //t15.DoTests();

                //IWMCodecInfo3Test t16 = new IWMCodecInfo3Test();
                //t16.DoTests();

                //IWMCodecVideoAcceleratorTest t17 = new IWMCodecVideoAcceleratorTest();
                //t17.DoTests();

                //IWMCredentialCallbackTest t18 = new IWMCredentialCallbackTest();
                //t18.DoTests();

                //IWMDRMEditorTest t19 = new IWMDRMEditorTest();
                //t19.DoTests();

                //IWMDRMReaderTest t20 = new IWMDRMReaderTest();
                //t20.DoTests();

                //IWMDRMWriterTest t21 = new IWMDRMWriterTest();
                //t21.DoTests();

                //IWMHeaderInfoTest t22 = new IWMHeaderInfoTest();
                //t22.DoTests();

                //IWMHeaderInfo2Test t23 = new IWMHeaderInfo2Test();
                //t23.DoTests();

                //IWMHeaderInfo3Test t24 = new IWMHeaderInfo3Test();
                //t24.DoTests();

                //IWMImageInfoTest t25 = new IWMImageInfoTest();
                //t25.DoTests();

                //IWMIndexerTest t26 = new IWMIndexerTest();
                //t26.DoTests();

                //IWMIndexer2Test t27 = new IWMIndexer2Test();
                //t27.DoTests();

                //IWMInputMediaPropsTest t28 = new IWMInputMediaPropsTest();
                //t28.DoTests();

                //IWMIStreamPropsTest t29 = new IWMIStreamPropsTest();
                //t29.DoTests();

                //IWMLanguageListTest t30 = new IWMLanguageListTest();
                //t30.DoTests();

                //IWMLicenseBackupTest t31 = new IWMLicenseBackupTest();
                //t31.DoTests();

                //IWMLicenseRestoreTest t32 = new IWMLicenseRestoreTest();
                //t32.DoTests();

                //IWMMediaPropsTest t33 = new IWMMediaPropsTest();
                //t33.DoTests();

                //IWMMetadataEditorTest t34 = new IWMMetadataEditorTest();
                //t34.DoTests();

                //IWMMetadataEditor2Test t35 = new IWMMetadataEditor2Test();
                //t35.DoTests();

                //IWMMutualExclusionTest t36 = new IWMMutualExclusionTest();
                //t36.DoTests();

                //IWMMutualExclusion2Test t37 = new IWMMutualExclusion2Test();
                //t37.DoTests();

                //IWMOutputMediaPropsTest t38 = new IWMOutputMediaPropsTest();
                //t38.DoTests();

                //IWMPacketSizeTest t39 = new IWMPacketSizeTest();
                //t39.DoTests();

                //IWMPacketSize2Test t40 = new IWMPacketSize2Test();
                //t40.DoTests();

                //IWMPlayerTimestampHookTest t41 = new IWMPlayerTimestampHookTest();
                //t41.DoTests();

                //IWMProfileTest t42 = new IWMProfileTest();
                //t42.DoTests();

                //IWMProfile2Test t43 = new IWMProfile2Test();
                //t43.DoTests();

                //IWMProfile3Test t44 = new IWMProfile3Test();
                //t44.DoTests();

                //IWMProfileManagerTest t45 = new IWMProfileManagerTest();
                //t45.DoTests();

                //IWMProfileManager2Test t46 = new IWMProfileManager2Test();
                //t46.DoTests();

                //IWMProfileManagerLanguageTest t47 = new IWMProfileManagerLanguageTest();
                //t47.DoTests();

                //IWMPropertyVaultTest t48 = new IWMPropertyVaultTest();
                //t48.DoTests();

                //IWMReaderTest t49 = new IWMReaderTest();
                //t49.DoTests();

                //IWMReaderAcceleratorTest t50 = new IWMReaderAcceleratorTest();
                //t50.DoTests();

                //IWMReaderAdvancedTest t51 = new IWMReaderAdvancedTest();
                //t51.DoTests();

                //IWMReaderAdvanced2Test t52 = new IWMReaderAdvanced2Test();
                //t52.DoTests();

                //IWMReaderAdvanced3Test t53 = new IWMReaderAdvanced3Test();
                //t53.DoTests();

                //IWMReaderAdvanced4Test t54 = new IWMReaderAdvanced4Test();
                //t54.DoTests();

                //IWMReaderAllocatorExTest t55 = new IWMReaderAllocatorExTest();
                //t55.DoTests();

                //IWMReaderCallbackTest t56 = new IWMReaderCallbackTest();
                //t56.DoTests();

                //IWMReaderCallbackAdvancedTest t57 = new IWMReaderCallbackAdvancedTest();
                //t57.DoTests();

                //IWMReaderNetworkConfigTest t58 = new IWMReaderNetworkConfigTest();
                //t58.DoTests();

                //IWMReaderNetworkConfig2Test t59 = new IWMReaderNetworkConfig2Test();
                //t59.DoTests();

                //IWMReaderStreamClockTest t60 = new IWMReaderStreamClockTest();
                //t60.DoTests();

                //IWMReaderTimecodeTest t61 = new IWMReaderTimecodeTest();
                //t61.DoTests();

                //IWMReaderTypeNegotiationTest t62 = new IWMReaderTypeNegotiationTest();
                //t62.DoTests();

                //IWMRegisterCallbackTest t63 = new IWMRegisterCallbackTest();
                //t63.DoTests();

                //IWMSBufferAllocatorTest t64 = new IWMSBufferAllocatorTest();
                //t64.DoTests();

                //IWMSInternalAdminNetSourceTest t65 = new IWMSInternalAdminNetSourceTest();
                //t65.DoTests();

                //IWMSInternalAdminNetSource2Test t66 = new IWMSInternalAdminNetSource2Test();
                //t66.DoTests();

                //IWMSInternalAdminNetSource3Test t67 = new IWMSInternalAdminNetSource3Test();
                //t67.DoTests();

                //IWMStatusCallbackTest t68 = new IWMStatusCallbackTest();
                //t68.DoTests();

                //IWMStreamConfigTest t69 = new IWMStreamConfigTest();
                //t69.DoTests();

                //IWMStreamConfig2Test t70 = new IWMStreamConfig2Test();
                //t70.DoTests();

                //IWMStreamConfig3Test t71 = new IWMStreamConfig3Test();
                //t71.DoTests();

                //IWMStreamListTest t72 = new IWMStreamListTest();
                //t72.DoTests();

                //IWMStreamPrioritizationTest t73 = new IWMStreamPrioritizationTest();
                //t73.DoTests();

                //IWMSyncReaderTest t74 = new IWMSyncReaderTest();
                //t74.DoTests();

                //IWMSyncReader2Test t75 = new IWMSyncReader2Test();
                //t75.DoTests();

                //IWMVideoMediaPropsTest t76 = new IWMVideoMediaPropsTest();
                //t76.DoTests();

                //IWMWatermarkInfoTest t77 = new IWMWatermarkInfoTest();
                //t77.DoTests();

                //IWMWriterTest t78 = new IWMWriterTest();
                //t78.DoTests();

                //IWMWriterAdvancedTest t79 = new IWMWriterAdvancedTest();
                //t79.DoTests();

                //IWMWriterAdvanced2Test t80 = new IWMWriterAdvanced2Test();
                //t80.DoTests();

                //IWMWriterAdvanced3Test t81 = new IWMWriterAdvanced3Test();
                //t81.DoTests();

                //IWMWriterFileSinkTest t82 = new IWMWriterFileSinkTest();
                //t82.DoTests();

                //IWMWriterFileSink2Test t83 = new IWMWriterFileSink2Test();
                //t83.DoTests();

                //IWMWriterFileSink3Test t84 = new IWMWriterFileSink3Test();
                //t84.DoTests();

                //IWMWriterNetworkSinkTest t85 = new IWMWriterNetworkSinkTest();
                //t85.DoTests();

                //IWMWriterPostViewTest t86 = new IWMWriterPostViewTest();
                //t86.DoTests();

                //IWMWriterPostViewCallbackTest t87 = new IWMWriterPostViewCallbackTest();
                //t87.DoTests();

                //IWMWriterPreprocessTest t88 = new IWMWriterPreprocessTest();
                //t88.DoTests();

                //IWMWriterPushSinkTest t89 = new IWMWriterPushSinkTest();
                //t89.DoTests();

                //IWMWriterSinkTest t90 = new IWMWriterSinkTest();
                //t90.DoTests();

            }
            catch (Exception e)
            {
                int hr = Marshal.GetHRForException(e);
                string s = WMError.GetErrorText(hr);

                if (s == null)
                {
                    s = e.Message;
                }
                else
                {
                    s = string.Format("{0} ({1})", s, e.Message);
                }

                System.Windows.Forms.MessageBox.Show(string.Format("0x{0:x}: {1}", hr, s), "Exception", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
    }
}
