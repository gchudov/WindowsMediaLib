using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

using WindowsMediaLib;
using MultiMedia;

namespace v1._0
{
    class TestMMIO
    {
        const int BUFSIZE = 1024 * 8;
        const string sFilename = @"c:\testout.wav";
        const string sFilename2 = @"c:\testout.xyz";

        public void DoTests()
        {
            MMIOINFO mminfo = new MMIOINFO();
            MMIOINFO mminfo2 = new MMIOINFO();

            IntPtr ipOutput;

            ipOutput = MMIO.Open(sFilename, mminfo, MMIOFlags.ReadWrite);

            int i = MMIO.Seek(ipOutput, 0, MMIOSeekFlags.Set);
            Debug.Assert(i >= 0);

            MMIOError mm = MMIO.Flush(ipOutput, MMIOFlushFlags.None);
            MMIO.ThrowExceptionForError(mm);

            mm = MMIO.GetInfo(ipOutput, mminfo, Marshal.SizeOf(typeof(MMIOINFO)));
            MMIO.ThrowExceptionForError(mm);

            Debug.Assert(mminfo.dwFlags == MMIOFlags.ReadWrite);

            mm = MMIO.SetInfo(ipOutput, mminfo, Marshal.SizeOf(typeof(MMIOINFO)));
            MMIO.ThrowExceptionForError(mm);

            IntPtr ipbuff = Marshal.AllocCoTaskMem(BUFSIZE);
            mm = MMIO.SetBuffer(ipOutput, ipbuff, BUFSIZE, 0);
            MMIO.ThrowExceptionForError(mm);

            mm = MMIO.GetInfo(ipOutput, mminfo2, Marshal.SizeOf(typeof(MMIOINFO)));
            MMIO.ThrowExceptionForError(mm);

            mm = MMIO.Advance(ipOutput, mminfo2, RWMode.Read);
            MMIO.ThrowExceptionForError(mm);

            Debug.Assert(mminfo2.lDiskOffset == BUFSIZE);

            mm = MMIO.Close(ipOutput, MMIOCloseFlags.None);
            MMIO.ThrowExceptionForError(mm);

            mm = MMIO.Rename(sFilename, sFilename2, null, 0);
            MMIO.ThrowExceptionForError(mm);
            mm = MMIO.Rename(sFilename2, sFilename, null, 0);
            MMIO.ThrowExceptionForError(mm);

        }
    }
}
