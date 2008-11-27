#region license

/*
WindowsMediaLib - Provide access to Windows Media interfaces via .NET
Copyright (C) 2008
http://sourceforge.net/projects/windowsmedianet

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

#endregion

using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

using WindowsMediaLib;
using WindowsMediaLib.Defs;

namespace MultiMedia
{
    #region Enums

    [Flags]
    public enum WaveOpenFlags
    {
        None = 0,
        FormatQuery = 0x0001,
        AllowSync = 0x0002,
        Mapped = 0x0004,
        FormatDirect = 0x0008,
        Null = 0x00000000,      /* no callback */
        Window = 0x00010000,    /* dwCallback is a HWND */
        Thread = 0x00020000,    /* dwCallback is a THREAD */
        Function = 0x00030000,  /* dwCallback is a FARPROC */
        Event = 0x00050000      /* dwCallback is an EVENT Handle */
    }

    [Flags]
    public enum SupportedFormats
    {
        InvalidFormat = 0x00000000,       /* invalid format */
        F1M08 = 0x00000001,  /* 11.025 kHz, Mono,   8-bit  */
        F1S08 = 0x00000002,  /* 11.025 kHz, Stereo, 8-bit  */
        F1M16 = 0x00000004,  /* 11.025 kHz, Mono,   16-bit */
        F1S16 = 0x00000008,  /* 11.025 kHz, Stereo, 16-bit */
        F2M08 = 0x00000010,  /* 22.05  kHz, Mono,   8-bit  */
        F2S08 = 0x00000020,  /* 22.05  kHz, Stereo, 8-bit  */
        F2M16 = 0x00000040,  /* 22.05  kHz, Mono,   16-bit */
        F2S16 = 0x00000080,  /* 22.05  kHz, Stereo, 16-bit */

        F44M08 = 0x00000100,   /* 44.1   kHz, Mono,   8-bit  */
        F44S08 = 0x00000200,   /* 44.1   kHz, Stereo, 8-bit  */
        F44M16 = 0x00000400,   /* 44.1   kHz, Mono,   16-bit */
        F44S16 = 0x00000800,   /* 44.1   kHz, Stereo, 16-bit */
        F48M08 = 0x00001000,   /* 48     kHz, Mono,   8-bit  */
        F48S08 = 0x00002000,   /* 48     kHz, Stereo, 8-bit  */
        F48M16 = 0x00004000,   /* 48     kHz, Mono,   16-bit */
        F48S16 = 0x00008000,   /* 48     kHz, Stereo, 16-bit */
        F96M08 = 0x00010000,   /* 96     kHz, Mono,   8-bit  */
        F96S08 = 0x00020000,   /* 96     kHz, Stereo, 8-bit  */
        F96M16 = 0x00040000,   /* 96     kHz, Mono,   16-bit */
        F96S16 = 0x00080000,   /* 96     kHz, Stereo, 16-bit */
    }

    public enum MOWM
    {
        WOM_OPEN = 0x3BB,
        WOM_CLOSE = 0x3BC,
        WOM_DONE = 0x3BD,
    }

    public enum MIWM
    {
        WIM_OPEN = 0x3BE,           /* waveform input */
        WIM_CLOSE = 0x3BF,
        WIM_DATA = 0x3C0
    }

    [Flags]
    public enum WaveCapsFlags
    {
        None = 0,
        Pitch = 0x0001,         /* supports pitch control */
        PlaybackRate = 0x0002,  /* supports playback rate control */
        Volume = 0x0004,        /* supports volume control */
        LRVolume = 0x0008,      /* separate left-right volume control */
        Sync = 0x0010,
        SampleAccurate = 0x0020
    }

    [Flags]
    public enum RiffChunkFlags
    {
        None = 0,
        FindChunk = 0x0010,     /* mmioDescend: find a chunk by ID */
        FindRiff = 0x0020,      /* mmioDescend: find a LIST chunk */
        FindList = 0x0040,      /* mmioDescend: find a RIFF chunk */
        CreateRiff = 0x0020,    /* mmioCreateChunk: make a LIST chunk */
        CreateList = 0x0040     /* mmioCreateChunk: make a RIFF chunk */
    }

    [Flags]
    public enum MMIOCloseFlags
    {
        None = 0,
        FHOPEN = 0x0010  /* mmioClose: keep file handle open */
    }

    [Flags]
    public enum MMIOFlushFlags
    {
        None = 0,
        EmptyBuf = 0x0010  /* mmioFlush: empty the I/O buffer */
    }

    public enum MMIOSeekFlags
    {
        Set = 0,
        Cur = 1,
        End = 2
    }

    public enum RWMode
    {
        Read = 0x00000000,          /* open file for reading only */
        Write = 0x00000001,         /* open file for writing only */
        ReadWrite = 0x00000002      /* open file for reading and writing */
    }

    public enum MMIOError
    {
        NoError = 0,
        FileNotFound = 257,     /* file not found */
        OutOfMemory = 258,      /* out of memory */
        CannotOpen = 259,       /* cannot open */
        CannotClose = 260,      /* cannot close */
        CannotRead = 261,       /* cannot read */
        CannotWrite = 262,      /* cannot write */
        CannotSeek = 263,       /* cannot seek */
        CannotExpand = 264,     /* cannot expand file */
        ChunkNotFound = 265,    /* chunk not found */
        Unbuffered = 266,       /*  */
        PathNotFound = 267,     /* path incorrect */
        AccessDenied = 268,     /* file was protected */
        SharingViolation = 269, /* file in use */
        NetworkError = 270,     /* network not responding */
        TooManyOpenFiles = 271, /* no more file handles  */
        InvalidFile = 272       /* default error file error */
    }

    [Flags]
    public enum MMIOFlags
    {
        /* constants for dwFlags field of MMIOINFO */
        Create = 0x00001000,        /* create new file (or truncate file) */
        Parse = 0x00000100,         /* parse new file returning path */
        Delete = 0x00000200,        /* create new file (or truncate file) */
        Exist = 0x00004000,         /* checks for existence of file */
        AllocBuf = 0x00010000,      /* mmioOpen() should allocate a buffer */
        GetTemp = 0x00020000,       /* mmioOpen() should retrieve temp name */

        Dirty = 0x10000000,         /* I/O buffer is dirty */

        /* read/write mode numbers (bit field MMIO_RWMODE) */
        Read = 0x00000000,          /* open file for reading only */
        Write = 0x00000001,         /* open file for writing only */
        ReadWrite = 0x00000002,     /* open file for reading and writing */

        /* share mode numbers (bit field MMIO_SHAREMODE) */
        Compat = 0x00000000,        /* compatibility mode */
        Exclusive = 0x00000010,     /* exclusive-access mode */
        DenyWrite = 0x00000020,     /* deny writing to other processes */
        DenyRead = 0x00000030,      /* deny reading to other processes */
        DenyNone = 0x00000040,      /* deny nothing to other processes */
    }

    #endregion

    #region Structs

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode),
    UnmanagedName("WAVEOUTCAPSW")]
    public class WaveOutCaps
    {
        public short wMid;                  /* manufacturer ID */
        public short wPid;                  /* product ID */
        public int vDriverVersion;          /* version of the driver */
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;              /* product name (NULL terminated string) */
        public SupportedFormats dwFormats;  /* formats supported */
        public short wChannels;             /* number of sources supported */
        public short wReserved1;            /* packing */
        public WaveCapsFlags dwSupport;     /* functionality supported by driver */
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode),
    UnmanagedName("WAVEINCAPSW")]
    public class WaveInCaps
    {
        public short wMid;
        public short wPid;
        public int vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;
        public SupportedFormats dwFormats;
        public short wChannels;
        public short wReserved1;
    }

    [StructLayout(LayoutKind.Sequential), UnmanagedName("MMTIME")]
    public class MMTIME
    {
        [Flags]
        public enum MMTimeFlags
        {
            MS = 0x0001,        /* time in milliseconds */
            Samples = 0x0002,   /* number of wave samples */
            Bytes = 0x0004,     /* current byte offset */
            SMPTE = 0x0008,     /* SMPTE time */
            Midi = 0x0010,      /* MIDI time */
            Ticks = 0x0020      /* Ticks within MIDI stream */
        }

        public MMTimeFlags wType;
        public int u;
        public int x;
    }

    [StructLayout(LayoutKind.Sequential), UnmanagedName("WAVEHDR")]
    public class WAVEHDR : IDisposable
    {
        [Flags]
        public enum WHDR
        {
            None = 0x0,
            Done = 0x00000001,      /* done bit */
            Prepared = 0x00000002,  /* set if this header has been prepared */
            BeginLoop = 0x00000004, /* loop start block */
            EndLoop = 0x00000008,   /* loop end block */
            InQueue = 0x00000010    /* reserved for driver */
        }

        public IntPtr lpData;
        public int dwBufferLength;
        public int dwBytesRecorded;
        public IntPtr dwUser;
        public WHDR dwFlags;
        public int dwLoops;
        public IntPtr lpNext;
        public IntPtr Reserved;

        public WAVEHDR()
        {
        }

        public WAVEHDR(int iMaxSize)
        {
            lpData = Marshal.AllocCoTaskMem(iMaxSize);
            dwBufferLength = iMaxSize;
            dwUser = IntPtr.Zero;
            dwFlags = WHDR.None;
            dwLoops = 0;
            lpNext = IntPtr.Zero;
            Reserved = IntPtr.Zero;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (lpData != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(lpData);
                lpData = IntPtr.Zero;
            }
        }

        #endregion
    }

    [StructLayout(LayoutKind.Sequential), UnmanagedName("MMCKINFO")]
    public class MMCKINFO
    {
        public FourCC ckid;
        public int ckSize;
        public FourCC fccType;
        public int dwDataOffset;
        public MMIOFlags dwFlags;
    }

    [StructLayout(LayoutKind.Sequential, Pack=1), UnmanagedName("MMIOINFO")]
    public class MMIOINFO
    {
        public MMIOFlags dwFlags;
        public FourCC fccIOProc;
        public IntPtr pIOProc;
        public MMIOError wErrorRet;
        public IntPtr htask;
        public int cchBuffer;
        public IntPtr pchBuffer;
        public IntPtr pchNext;
        public IntPtr pchEndRead;
        public IntPtr pchEndWrite;
        public int lBufOffset;
        public int lDiskOffset;
        public int adwInfo1;
        public int adwInfo2;
        public int adwInfo3;
        public int dwReserved1;
        public int dwReserved2;
        public IntPtr hmmio;
    }

    #endregion

    public static class MMIO
    {
        public delegate int MMIOProc(
          [In] string lpmmioinfo,
          int uMsg,
          IntPtr lParam1,
          IntPtr lParam2
        );

        public static string Errorstring(MMIOError i)
        {
            string sRet;

            switch (i)
            {
                case MMIOError.NoError:
                    sRet = "No error";
                    break;
                case MMIOError.FileNotFound:
                    sRet = "File not found";
                    break;
                case MMIOError.OutOfMemory:
                    sRet = "Out of memory";
                    break;
                case MMIOError.CannotOpen:
                    sRet = "Cannot open";
                    break;
                case MMIOError.CannotClose:
                    sRet = "Cannot close";
                    break;
                case MMIOError.CannotRead:
                    sRet = "Cannot read";
                    break;
                case MMIOError.CannotWrite:
                    sRet = "Cannot write";
                    break;
                case MMIOError.CannotSeek:
                    sRet = "Cannot seek";
                    break;
                case MMIOError.CannotExpand:
                    sRet = "Cannot expand file";
                    break;
                case MMIOError.ChunkNotFound:
                    sRet = "Chunk not found";
                    break;
                case MMIOError.Unbuffered:
                    sRet = "Unbuffered";
                    break;
                case MMIOError.PathNotFound:
                    sRet = "Path incorrect";
                    break;
                case MMIOError.AccessDenied:
                    sRet = "File was protected (Access denied)";
                    break;
                case MMIOError.SharingViolation:
                    sRet = "file in use (Sharing violation)";
                    break;
                case MMIOError.NetworkError:
                    sRet = "Network not responding";
                    break;
                case MMIOError.TooManyOpenFiles:
                    sRet = "No more file handles";
                    break;
                case MMIOError.InvalidFile:
                    sRet = "Invalid File";
                    break;
                default:
                    sRet = "Unknown error number: " + i.ToString();
                    break;
            }
            return sRet;
        }

        public static void ThrowExceptionForError(MMIOError i)
        {
            if (i != MMIOError.NoError)
            {
                throw new Exception(Errorstring(i));
            }
        }

        #region Externs

#if false
        // These methods are unnecessary or deprecated

        [DllImport("winmm.dll",
        CharSet = CharSet.Unicode,
        ExactSpelling=true,
        EntryPoint="mmioStringToFOURCCW"),
        SuppressUnmanagedCodeSecurity]
        public static extern FourCC mmioStringToFOURCC(
            [In] string sz,
            int uFlags);

        //LPMMIOPROC WINAPI mmioInstallIOProc( FOURCC fccIOProc, LPMMIOPROC pIOProc, DWORD dwFlags);
#endif

        [DllImport("winmm.dll", ExactSpelling = true, CharSet = CharSet.Unicode, EntryPoint = "mmioRenameW"),
        SuppressUnmanagedCodeSecurity]
        public static extern MMIOError Rename(
            [In] string pszFileName,
            [In] string pszNewFileName,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] MMIOINFO pmmioinfo,
            int fdwRename);

        [DllImport("winmm.dll", EntryPoint = "mmioGetInfo"),
        SuppressUnmanagedCodeSecurity]
        public static extern MMIOError GetInfo(
            IntPtr hmmio,
            [Out, MarshalAs(UnmanagedType.LPStruct)] MMIOINFO pmmioinfo,
            int fuInfo);

        [DllImport("winmm.dll", EntryPoint = "mmioSetInfo"),
        SuppressUnmanagedCodeSecurity]
        public static extern MMIOError SetInfo(
            IntPtr hmmio,
            [In, MarshalAs(UnmanagedType.LPStruct)] MMIOINFO pmmioinfo,
            int fuInfo);

        [DllImport("winmm.dll", EntryPoint = "mmioSetBuffer"),
        SuppressUnmanagedCodeSecurity]
        public static extern MMIOError SetBuffer(
            IntPtr hmmio,
            IntPtr pchBuffer,
            int cchBuffer,
            int fuBuffer);

        [DllImport("winmm.dll", EntryPoint = "mmioAdvance"),
        SuppressUnmanagedCodeSecurity]
        public static extern MMIOError Advance(
            IntPtr hmmio,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] MMIOINFO pmmioinfo,
            RWMode fuAdvance);

        [DllImport("winmm.dll", EntryPoint = "mmioSendMessage"),
        SuppressUnmanagedCodeSecurity]
        public static extern MMIOError SendMessage(
            IntPtr hmmio,
            int uMsg,
            IntPtr lParam1,
            IntPtr lParam2);

        [DllImport("winmm.dll", EntryPoint = "mmioClose"),
        SuppressUnmanagedCodeSecurity]
        public static extern MMIOError Close(
            IntPtr hmmio,
            MMIOCloseFlags uFlags);

        [DllImport("winmm.dll", EntryPoint = "mmioFlush"),
        SuppressUnmanagedCodeSecurity]
        public static extern MMIOError Flush(
            IntPtr hmmio,
            MMIOFlushFlags uFlags);

        [DllImport("winmm.dll", EntryPoint = "mmioDescend"),
        SuppressUnmanagedCodeSecurity]
        public static extern MMIOError Descend(
            IntPtr hmmio,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] MMCKINFO lpck,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] MMCKINFO lpckParent,
            RiffChunkFlags uFlags);

        [DllImport("winmm.dll", EntryPoint = "mmioCreateChunk"),
        SuppressUnmanagedCodeSecurity]
        public static extern MMIOError CreateChunk(
            IntPtr hmmio,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] MMCKINFO lpck,
            RiffChunkFlags uFlags);

        [DllImport("winmm.dll", ExactSpelling = true, CharSet = CharSet.Unicode, EntryPoint = "mmioOpenW"),
        SuppressUnmanagedCodeSecurity]
        public static extern IntPtr Open(
            [In] string szFileName,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] MMIOINFO lpmmioinfo,
            MMIOFlags dwOpenFlags);

        [DllImport("winmm.dll", EntryPoint = "mmioRead"),
        SuppressUnmanagedCodeSecurity]
        public static extern MMIOError Read(
            IntPtr hmmio,
            IntPtr pch,
            int cch);

        [DllImport("winmm.dll", EntryPoint = "mmioWrite"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Write(
            IntPtr hmmio,
            IntPtr h,
            int cch);

        [DllImport("winmm.dll", EntryPoint = "mmioSeek"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Seek(
            IntPtr hmmio,
            int lOffset,
            MMIOSeekFlags iOrigin);

        [DllImport("winmm.dll", EntryPoint = "mmioAscend"),
        SuppressUnmanagedCodeSecurity]
        public static extern MMIOError Ascend(
            IntPtr hmmio,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] MMCKINFO lpck,
            int uFlags); // must be zero

        #endregion

    }

    public static class waveIn
    {
        private const short MAXERRORLENGTH = 256;

        public delegate void WaveInDelegate(
            IntPtr hwo,
            MIWM uMsg,
            IntPtr dwInstance,
            IntPtr dwParam1,
            IntPtr dwParam2);

        public static void ThrowExceptionForError(int rc)
        {
            if (rc != 0)
            {
                StringBuilder foo = new StringBuilder(MAXERRORLENGTH);
                GetErrorText(rc, foo, MAXERRORLENGTH);

                throw new Exception(foo.ToString());
            }
        }

        #region Externs

#if false
        // These methods are unnecessary or deprecated

        [DllImport("winmm.dll"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetID(
            IntPtr hwi,
            out int puDeviceID);
#endif

        [DllImport("winmm.dll", EntryPoint = "waveInGetPosition"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetPosition(
            IntPtr hwi,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] MMTIME pmmt,
            int cbmmt);

        [DllImport("winmm.dll", EntryPoint = "waveInMessage"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Message(
            IntPtr hwi,
            int uMsg,
            IntPtr dw1,
            IntPtr dw2);

        [DllImport("winmm.dll", ExactSpelling = true, CharSet = CharSet.Unicode, EntryPoint = "waveInGetDevCapsW"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetDevCaps(
            int uDeviceID,
            [Out, MarshalAs(UnmanagedType.LPStruct)] WaveInCaps pwic,
            int cbwic);

        [DllImport("winmm.dll", EntryPoint = "waveInGetNumDevs"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetNumDevs();

        [DllImport("winmm.dll", EntryPoint = "waveInStop"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Stop(
            IntPtr hwi);

        [DllImport("winmm.dll", ExactSpelling = true, CharSet = CharSet.Unicode, EntryPoint = "waveInGetErrorTextW"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetErrorText(
            int errvalue,
            [Out] StringBuilder lpText,
            int uSize);

        [DllImport("winmm.dll", EntryPoint = "waveInOpen"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Open(
            out IntPtr hwi,
            int uDeviceID,
            [In, MarshalAs(UnmanagedType.LPStruct)] WaveFormatEx b,
            WaveInDelegate dwCallback,
            IntPtr dwCallbackInstance,
            WaveOpenFlags dwFlags);

        [DllImport("winmm.dll", EntryPoint = "waveInPrepareHeader"),
        SuppressUnmanagedCodeSecurity]
        public static extern int PrepareHeader(
            IntPtr hwi,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] WAVEHDR lpWaveInHdr,
            int uSize);

        [DllImport("winmm.dll", EntryPoint = "waveInUnprepareHeader"),
        SuppressUnmanagedCodeSecurity]
        public static extern int UnprepareHeader(
            IntPtr hwi,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] WAVEHDR lpWaveInHdr,
            int uSize);

        [DllImport("winmm.dll", EntryPoint = "waveInStart"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Start(
            IntPtr hwi);

        [DllImport("winmm.dll", EntryPoint = "waveInAddBuffer"),
        SuppressUnmanagedCodeSecurity]
        public static extern int AddBuffer(
            IntPtr hwi,
            IntPtr lpWaveInHdr,
            int uSize);

        [DllImport("winmm.dll", EntryPoint = "waveInClose"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Close(
            IntPtr hwi);

        [DllImport("winmm.dll", EntryPoint = "waveInReset"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Reset(
            IntPtr hwi);

        #endregion

    }

    public static class waveOut
    {
        private const short MAXERRORLENGTH = 256;

        public delegate void WaveOutDelegate(
            IntPtr hwo,
            MOWM uMsg,
            IntPtr dwInstance,
            IntPtr dwParam1,
            IntPtr dwParam2);

        public static void ThrowExceptionForError(int rc)
        {
            if (rc != 0)
            {
                StringBuilder foo = new StringBuilder(MAXERRORLENGTH);
                GetErrorText(rc, foo, MAXERRORLENGTH);

                throw new Exception(foo.ToString());
            }
        }

        #region Externs

#if false
        // These methods are unnecessary or deprecated

        [DllImport("winmm.dll"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetID(
            IntPtr hwo,
            out int puDeviceID);
#endif

        [DllImport("winmm.dll", EntryPoint = "waveOutGetNumDevs"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetNumDevs();

        [DllImport("winmm.dll", ExactSpelling = true, CharSet = CharSet.Unicode, EntryPoint = "waveOutGetDevCapsW"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetDevCaps(
            int uDeviceID,
            [Out, MarshalAs(UnmanagedType.LPStruct)] WaveOutCaps pwoc,
            int cbwoc);

        [DllImport("winmm.dll", EntryPoint = "waveOutGetPitch"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetPitch(
            IntPtr hwo,
            out int pdwPitch);

        [DllImport("winmm.dll", EntryPoint = "waveOutSetPitch"),
        SuppressUnmanagedCodeSecurity]
        public static extern int SetPitch(
            IntPtr hwo,
            int dwPitch);

        [DllImport("winmm.dll", EntryPoint = "waveOutGetPlaybackRate"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetPlaybackRate(
            IntPtr hwo,
            out int pdwRate);

        [DllImport("winmm.dll", EntryPoint = "waveOutSetPlaybackRate"),
        SuppressUnmanagedCodeSecurity]
        public static extern int SetPlaybackRate(
            IntPtr hwo,
            int dwRate);

        [DllImport("winmm.dll", EntryPoint = "waveOutMessage"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Message(
            IntPtr hwo,
            int uMsg,
            IntPtr dw1,
            IntPtr dw2);

        [DllImport("winmm.dll", EntryPoint = "waveOutBreakLoop"),
        SuppressUnmanagedCodeSecurity]
        public static extern int BreakLoop(
            IntPtr hwo);

        [DllImport("winmm.dll", EntryPoint = "waveOutGetPosition"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetPosition(
            IntPtr hWaveOut,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] MMTIME lpInfo,
            int uSize);

        [DllImport("winmm.dll", ExactSpelling = true, CharSet = CharSet.Unicode, EntryPoint = "waveOutGetErrorTextW"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetErrorText(
            int errvalue,
            [Out] StringBuilder lpText,
            int uSize);

        [DllImport("winmm.dll", EntryPoint = "waveOutOpen"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Open(
            out IntPtr hWaveOut,
            int uDeviceID,
            [In, MarshalAs(UnmanagedType.LPStruct)] WaveFormatEx b,
            WaveOutDelegate dwCallback, // If using Function callback
            IntPtr dwCallbackInstance,
            WaveOpenFlags dwFlags);

        [DllImport("winmm.dll", EntryPoint = "waveOutOpen"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Open(
            out IntPtr hWaveOut,
            int uDeviceID,
            [In, MarshalAs(UnmanagedType.LPStruct)] WaveFormatEx b,
            IntPtr dwCallback, // If using Event
            IntPtr dwCallbackInstance,
            WaveOpenFlags dwFlags);

        [DllImport("winmm.dll", EntryPoint = "waveOutPrepareHeader"),
        SuppressUnmanagedCodeSecurity]
        public static extern int PrepareHeader(
            IntPtr hWaveOut,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] WAVEHDR lpWaveOutHdr,
            int uSize);

        [DllImport("winmm.dll", EntryPoint = "waveOutReset"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Reset(
            IntPtr hWaveOut);

        [DllImport("winmm.dll", EntryPoint = "waveOutUnprepareHeader"),
        SuppressUnmanagedCodeSecurity]
        public static extern int UnprepareHeader(
            IntPtr hWaveOut,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] WAVEHDR lpWaveOutHdr,
            int uSize);

        [DllImport("winmm.dll", EntryPoint = "waveOutClose"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Close(
            IntPtr hWaveOut);

        [DllImport("winmm.dll", EntryPoint = "waveOutWrite"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Write(
            IntPtr hWaveOut,
            IntPtr lpWaveOutHdr,
            int uSize);

        [DllImport("winmm.dll", EntryPoint = "waveOutPause"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Pause(
            IntPtr hWaveOut);

        [DllImport("winmm.dll", EntryPoint = "waveOutRestart"),
        SuppressUnmanagedCodeSecurity]
        public static extern int Restart(
            IntPtr hWaveOut);

        [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume"),
        SuppressUnmanagedCodeSecurity]
        public static extern int SetVolume(
            IntPtr uDeviceID,
            int dwVolume);

        [DllImport("winmm.dll", EntryPoint = "waveOutGetVolume"),
        SuppressUnmanagedCodeSecurity]
        public static extern int GetVolume(
            IntPtr uDeviceID,
            out int lpdwVolume);

        #endregion

    }
}
