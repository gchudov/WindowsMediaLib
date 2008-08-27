// Two of the methods here (GetStatistics & GetStatistics) are actually tested over in IWMWriter, since
// they require a fully configured file, and I didn't want to dupe it all here.

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
    public class IWMWriterSinkTest : IWMWriterSink
    {
        bool m_OnHeader, m_IsRealTime, m_AllocateDataUnit, m_OnDataUnit, m_OnEndWriting;
        private const string sFileName = @"c:\WmTestOut.wmv";
        IWMWriter m_Writer;
        private int m_iFrameRate = 3;

        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);

        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

        public void DoTests()
        {
            Config();

            Test();
        }

        private void Test()
        {
            Bitmap b = new Bitmap(@"c:\sga.png");
            Initialize(b);

            m_Writer.SetOutputFilename(sFileName);

            m_Writer.BeginWriting();
            INSSBuffer pSample = WriteOne(b);
            m_Writer.WriteSample(0, 1, WriteFlags.CleanPoint, pSample);
            m_Writer.Flush();
            m_Writer.EndWriting();

            Debug.Assert(m_OnHeader && m_IsRealTime && m_AllocateDataUnit && m_OnDataUnit && m_OnEndWriting);
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
            m_Writer.GetInputProps(0, out pProps);
            pProps.SetMediaType(mt);

            // Now take the inputprops, and set them on the file writer
            m_Writer.SetInputProps(0, pProps);
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

        private void Config()
        {
            m_OnHeader = m_IsRealTime = m_AllocateDataUnit = m_OnDataUnit = m_OnEndWriting = false;
            IWMWriterAdvanced pWriterA;

            WMUtils.WMCreateWriter(IntPtr.Zero, out m_Writer);
            pWriterA = m_Writer as IWMWriterAdvanced;
            m_Writer.SetProfileByID(g);

            pWriterA.AddSink(this);
        }

        #region IWMWriterSink Members

        public void OnHeader(INSSBuffer pHeader)
        {
            if (pHeader != null)
            {
                m_OnHeader = true;
            }
        }

        public void IsRealTime(out bool pfRealTime)
        {
            m_IsRealTime = true;
            pfRealTime = false;
        }

        public void AllocateDataUnit(int cbDataUnit, out INSSBuffer ppDataUnit)
        {
            if (cbDataUnit > 0)
            {
                m_AllocateDataUnit = true;
            }
            TempBuff t = new TempBuff(cbDataUnit);
            ppDataUnit = t as INSSBuffer;
        }

        public void OnDataUnit(INSSBuffer pDataUnit)
        {
            if (pDataUnit != null)
            {
                m_OnDataUnit = true;
            }
        }

        public void OnEndWriting()
        {
            m_OnEndWriting = true;
        }

        #endregion
    }
}
