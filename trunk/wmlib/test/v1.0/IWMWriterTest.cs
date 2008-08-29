using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

using WindowsMediaLib;
using WindowsMediaLib.Defs;
using System.Runtime.InteropServices.ComTypes;

namespace v1._0
{
    public class IWMWriterTest : IWMWriterPostViewCallback
    {
        private const string sFileName = @"c:\WmTestOut.wmv";
        private IWMWriter m_Writer;
        private bool m_ViewSample;
        private bool m_Alloc;

        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);
        private int m_iFrameRate = 3;
        private int m_dwVideoInput = 0;
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);

        public void DoTests()
        {
            Config();

            TestProfile();
            TestPostView();
            TestInput();
            TestWrite();
            TestPostView2();
        }

        private void TestPostView2()
        {
            Debug.Assert(m_ViewSample);
            Debug.Assert(m_Alloc);
        }

        private void TestPostView()
        {
            m_ViewSample = false;
            m_Alloc = false;

            IWMWriterPostView w = m_Writer as IWMWriterPostView;
            w.SetPostViewCallback(this, new IntPtr(17));
            w.SetAllocateForPostView(1, true);
            w.SetReceivePostViewSamples(1, true);
        }

        private void TestWrite()
        {
            Bitmap b = new Bitmap(@"c:\sga.png");
            Initialize(b);

            m_Writer.SetOutputFilename(sFileName);

            m_Writer.BeginWriting();
            INSSBuffer pSample = WriteOne(b);
            m_Writer.WriteSample(0, 1, WM_SF.CleanPoint, pSample);
            m_Writer.Flush();
            TestAdvanced(b);
            m_Writer.EndWriting();
        }

        // These routines are for testing IWMWriterAdvanced
        private void TestAdvanced(Bitmap b)
        {
            IWMWriterAdvanced adv = m_Writer as IWMWriterAdvanced;
            WriterStatistics ws;
            adv.GetStatistics(0, out ws);

            Debug.Assert(ws.dwCurrentBitrate > 0);

            INSSBuffer pSample = WriteOne(b);
            adv.WriteStreamSample(1, 1234, 5678, 101001, WM_SF.CleanPoint, pSample);
        }

        private INSSBuffer WriteOne(Bitmap hBitmap)
        {
            INSSBuffer pSample;
            Rectangle r = new Rectangle(0, 0, hBitmap.Width, hBitmap.Height);

            // Lock the bitmap, which gets us a pointer to the raw bitmap data
            BitmapData bmd = hBitmap.LockBits(r, ImageLockMode.ReadOnly, hBitmap.PixelFormat);

            try
            {
                // Compute size of bitmap in bytes.  Strides may be negative.
                int iSize = Math.Abs(bmd.Stride * bmd.Height);
                IntPtr ip;

                // Get a sample interface
                m_Writer.AllocateSample(iSize, out pSample);

                // Get the buffer from the sample interface.  This is
                // where we copy the bitmap data to
                pSample.GetBuffer(out ip);

                // Copy the bitmap data into the sample buffer
                LoadSample(bmd, ip, iSize);
            }
            finally
            {
                hBitmap.UnlockBits(bmd);
            }

            return pSample;
        }

        private void LoadSample(BitmapData bmd, IntPtr ip, int iSize)
        {
            // If the bitmap is rightside up
            if (bmd.Stride < 0)
            {
                CopyMemory(ip, bmd.Scan0, iSize);
            }
            else
            {
                // Copy it line by line from bottom to top
                IntPtr ip2 = (IntPtr)(ip.ToInt32() + iSize - bmd.Stride);
                for (int x = 0; x < bmd.Height; x++)
                {
                    CopyMemory(ip2, (IntPtr)(bmd.Scan0.ToInt32() + (bmd.Stride * x)), bmd.Stride);
                    ip2 = (IntPtr)(ip2.ToInt32() - bmd.Stride);
                }
            }
        }

        private void TestInput()
        {
            int iCount, iFCount;
            IWMInputMediaProps pMP, pMP2;

            m_Writer.GetInputCount(out iCount);
            m_Writer.GetInputProps(0, out pMP);
            m_Writer.GetInputFormatCount(0, out iFCount);
            m_Writer.GetInputFormat(0, 0, out pMP2);
            m_Writer.SetInputProps(0, pMP);
        }

        private void TestProfile()
        {
            // Initialize all member variables

            IWMProfile pWMProfile = CreateProfile();

            m_Writer.SetProfile(pWMProfile);
            m_Writer.SetProfileByID(g);
        }

        private IWMProfile CreateProfile()
        {
            IWMProfileManager pWMProfileManager = null;
            IWMProfile pWMProfile = null;

            // Open the profile manager
            WMUtils.WMCreateProfileManager(out pWMProfileManager);

            // Convert pWMProfileManager to a IWMProfileManager2
            IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;

            // Specify the version number of the profiles to use
            pProfileManager2.SetSystemProfileVersion(WMVersion.V8_0);

            pProfileManager2.LoadProfileByID(g, out pWMProfile);

            return pWMProfile;

        }

        private void Initialize(Bitmap hBitmap)
        {
            AMMediaType mt = new AMMediaType();
            VideoInfoHeader videoInfo = new VideoInfoHeader();

            // Create the VideoInfoHeader using info from the bitmap
            videoInfo.BmiHeader.Size = Marshal.SizeOf(typeof(BitmapInfoHeader));
            videoInfo.BmiHeader.Width = hBitmap.Width;
            videoInfo.BmiHeader.Height = hBitmap.Height;
            videoInfo.BmiHeader.Planes = 1;

            // compression thru clrimportant don't seem to be used. Init them anyway
            videoInfo.BmiHeader.Compression = 0;
            videoInfo.BmiHeader.ImageSize = 0;
            videoInfo.BmiHeader.XPelsPerMeter = 0;
            videoInfo.BmiHeader.YPelsPerMeter = 0;
            videoInfo.BmiHeader.ClrUsed = 0;
            videoInfo.BmiHeader.ClrImportant = 0;

            switch (hBitmap.PixelFormat)
            {
                case PixelFormat.Format32bppRgb:
                    mt.subType = MediaSubType.RGB32;
                    videoInfo.BmiHeader.BitCount = 32;
                    break;
                case PixelFormat.Format24bppRgb:
                    mt.subType = MediaSubType.RGB24;
                    videoInfo.BmiHeader.BitCount = 24;
                    break;
                case PixelFormat.Format16bppRgb555:
                    mt.subType = MediaSubType.RGB555;
                    videoInfo.BmiHeader.BitCount = 16;
                    break;
                default:
                    throw new Exception("Unrecognized Pixelformat in bitmap");
            }
            videoInfo.SrcRect = new Rectangle(0, 0, hBitmap.Width, hBitmap.Height);
            videoInfo.TargetRect = videoInfo.SrcRect;
            videoInfo.BmiHeader.ImageSize = hBitmap.Width * hBitmap.Height * (videoInfo.BmiHeader.BitCount / 8);
            videoInfo.BitRate = videoInfo.BmiHeader.ImageSize * m_iFrameRate;
            videoInfo.BitErrorRate = 0;
            videoInfo.AvgTimePerFrame = 10000 * 1000 / m_iFrameRate;

            mt.majorType = MediaType.Video;
            mt.fixedSizeSamples = true;
            mt.temporalCompression = false;
            mt.sampleSize = videoInfo.BmiHeader.ImageSize;
            mt.formatType = FormatType.VideoInfo;
            mt.unkPtr = IntPtr.Zero;
            mt.formatSize = Marshal.SizeOf(typeof(VideoInfoHeader));

            mt.formatPtr = Marshal.AllocCoTaskMem(mt.formatSize);
            Marshal.StructureToPtr(videoInfo, mt.formatPtr, false);

            IWMInputMediaProps pProps;
            m_Writer.GetInputProps(m_dwVideoInput, out pProps);
            pProps.SetMediaType(mt);

            // Now take the inputprops, and set them on the file writer
            m_Writer.SetInputProps(m_dwVideoInput, pProps);
        }

        private void Config()
        {
            WMUtils.WMCreateWriter(IntPtr.Zero, out m_Writer);
        }

        #region IWMWriterPostViewCallback Members

        public void OnStatus(Status iStatus, int hr, AttrDataType dwType, IntPtr pValue, IntPtr pvContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OnPostViewSample(short wStreamNumber, long cnsSampleTime, long cnsSampleDuration, WM_SF dwFlags, INSSBuffer pSample, IntPtr pvContext)
        {
            Debug.Assert(pSample != null && pvContext.ToInt32() == 17);
            m_ViewSample = true;
        }

        public void AllocateForPostView(short wStreamNum, int cbBuffer, out INSSBuffer ppBuffer, IntPtr pvContext)
        {
            m_Alloc = true;
            TempBuff b = new TempBuff(100000);
            ppBuffer = b as INSSBuffer;
        }

        #endregion
    }

    internal class TempBuff : INSSBuffer
    {
        IntPtr m_ip;
        int m_iCurSize;

        public TempBuff(int i)
        {
            m_ip = Marshal.AllocCoTaskMem(i);
            m_iCurSize = i;
        }

        #region INSSBuffer Members

        public void GetLength(out int pdwLength)
        {
            pdwLength = m_iCurSize;
        }

        public void SetLength(int dwLength)
        {
            m_iCurSize = dwLength;
        }

        public void GetMaxLength(out int pdwLength)
        {
            pdwLength = m_iCurSize;
        }

        public void GetBuffer(out IntPtr ppdwBuffer)
        {
            ppdwBuffer = m_ip;
        }

        public void GetBufferAndLength(out IntPtr ppdwBuffer, out int pdwLength)
        {
            ppdwBuffer = m_ip;
            pdwLength = m_iCurSize;
        }

        #endregion
    }
}
