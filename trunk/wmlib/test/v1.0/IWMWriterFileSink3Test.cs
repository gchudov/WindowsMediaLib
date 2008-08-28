// I took a shot at OnDataUnitEx, following the docs and doing what they said.  But I never got WM to call it.  Apparently, the 
// code to call OnDataUnitEx is broken, even in c++ (see the post at 
// http://www.tech-archive.net/Archive/Media/microsoft.public.windowsmedia.sdk/2005-05/msg00098.html
// This guy is a long-time multi-media mvp.  If he can't make it go...

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
    public class IWMWriterFileSink3Test : IWMWriterFileSink3
    {
        private const string sFileName = @"c:\WmTestOut.wmv";
        IWMWriter m_Writer;
        private int m_iFrameRate = 3;
        private IWMWriterFileSink3 m_fs3;

        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);

        // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
        private Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

        public void DoTests()
        {
            Config();

            TestIndexing();
            TestUnbuffered();
            TestMisc();

            //TestSink();
        }

        private void TestIndexing()
        {
            bool b;

            m_fs3.SetAutoIndexing(false);
            m_fs3.GetAutoIndexing(out b);

            Debug.Assert(!b);
        }

        private void TestUnbuffered()
        {
            bool b;

            m_fs3.SetUnbufferedIO(true, true);
            m_fs3.GetUnbufferedIO(out b);

            Debug.Assert(b);
        }

        private void TestMisc()
        {
            FileSinkMode fsm;

            m_fs3.GetMode(out fsm);

            Debug.Assert(fsm == (FileSinkMode.FileSinkDataUnits | FileSinkMode.SingleBuffers | FileSinkMode.FileSinkUnbuffered) );

            m_fs3.CompleteOperations();

            m_fs3.SetControlStream(1, true);
        }

        private void TestSink()
        {
            IWMWriterAdvanced pWriterA;
            pWriterA = m_Writer as IWMWriterAdvanced;
            pWriterA.AddSink(this);

            Bitmap b = new Bitmap(@"c:\sga.png");
            Initialize(b);

            m_Writer.SetOutputFilename(sFileName);

            m_Writer.BeginWriting();
            INSSBuffer pSample = WriteOne(b);
            m_Writer.WriteSample(0, 1, WriteFlags.CleanPoint, pSample);
            m_Writer.Flush();
            m_Writer.EndWriting();
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

                // Get the buffer from the sample interface.    is
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
            IWMWriterFileSink pSink;

            WMUtils.WMCreateWriter(IntPtr.Zero, out m_Writer);
            m_Writer.SetProfileByID(g);

            WMUtils.WMCreateWriterFileSink(out pSink);

            IWMWriterAdvanced pWriterA;
            pWriterA = m_Writer as IWMWriterAdvanced;
            pWriterA.AddSink(pSink);
            m_fs3 = pSink as IWMWriterFileSink3;
        }

        #region IWMWriterFileSink3 Members

        public void OnHeader(INSSBuffer pHeader)
        {
        }

        public void IsRealTime(out bool pfRealTime)
        {
            pfRealTime = false;
        }

        public void AllocateDataUnit(int cbDataUnit, out INSSBuffer ppDataUnit)
        {
            TempBuff t = new TempBuff(cbDataUnit);
            ppDataUnit = t as INSSBuffer;
        }

        public void OnDataUnit(INSSBuffer pDataUnit)
        {
        }

        public void OnEndWriting()
        {
        }

        public void Open(string pwszFilename)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Start(long cnsStartTime)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Stop(long cnsStopTime)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void IsStopped(out bool pfStopped)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetFileDuration(out long pcnsDuration)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetFileSize(out long pcbFile)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Close()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void IsClosed(out bool pfClosed)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetAutoIndexing(bool fDoAutoIndexing)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetAutoIndexing(out bool pfAutoIndexing)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetControlStream(short wStreamNumber, bool fShouldControlStartAndStop)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetMode(out FileSinkMode pdwFileSinkMode)
        {
            pdwFileSinkMode = FileSinkMode.FileSinkDataUnits;
            pdwFileSinkMode = (FileSinkMode)(-1) & (~FileSinkMode.FileSinkUnbuffered);
        }

        public void OnDataUnitEx(FileSinkDataUnit pFileSinkDataUnit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetUnbufferedIO(bool fUnbufferedIO, bool fRestrictMemUsage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetUnbufferedIO(out bool pfUnbufferedIO)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CompleteOperations()
        {
        }

        #endregion
    }
}
