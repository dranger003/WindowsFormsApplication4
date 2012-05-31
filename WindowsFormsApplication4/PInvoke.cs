using System;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication4
{
    public static class PInvoke
    {
        public enum MSHCTX : uint
        {
            LOCAL = 0,
            NOSHAREDMEM = 1,
            DIFFERENTMACHINE = 2,
            INPROC = 3,
            CROSSCTX = 4
        }

        public enum MSHLFLAGS : uint
        {
            NORMAL = 0,
            TABLESTRONG = 1,
            TABLEWEAK = 2,
            NOPING = 4,
            RESERVED1 = 8,
            RESERVED2 = 16,
            RESERVED3 = 32,
            RESERVED4 = 64
        }

        public enum GMEM : uint
        {
            FIXED = 0x0000,
            MOVEABLE = 0x0002,
            NOCOMPACT = 0x0010,
            NODISCARD = 0x0020,
            ZEROINIT = 0x0040,
            MODIFY = 0x0080,
            DISCARDABLE = 0x0100,
            NOT_BANKED = 0x1000,
            SHARE = 0x2000,
            DDESHARE = 0x2000,
            NOTIFY = 0x4000,
            LOWER = NOT_BANKED,
            VALID_FLAGS = 0x7F72,
            INVALID_HANDLE = 0x8000
        }

        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();

        //__checkReturn WINOLEAPI CoGetMarshalSizeMax(__out ULONG *pulSize, __in REFIID riid, __in LPUNKNOWN pUnk,
        //                    __in DWORD dwDestContext, __in_opt LPVOID pvDestContext, __in DWORD mshlflags);
        [DllImport("ole32.dll")]
        public static extern int CoGetMarshalSizeMax(
            [Out] out uint pulSize,
            [In] ref Guid riid,
            [In, MarshalAs(UnmanagedType.IUnknown)] object pUnk,
            [In] MSHCTX dwDestContext,
            [In] IntPtr pvDestContext,
            [In] MSHLFLAGS mshflags);

        //__checkReturn WINOLEAPI CoMarshalInterface(__in LPSTREAM pStm, __in REFIID riid, __in LPUNKNOWN pUnk,
        //                    __in DWORD dwDestContext, __in_opt LPVOID pvDestContext, __in DWORD mshlflags);
        [DllImport("ole32.dll")]
        public static extern int CoMarshalInterface(
            [In, MarshalAs(UnmanagedType.Interface)] object pStm,
            [In] ref Guid riid,
            [In, MarshalAs(UnmanagedType.IUnknown)] object pUnk,
            [In] MSHCTX dwDestContext,
            [In] IntPtr pvDestContext,
            [In] MSHLFLAGS mshflags);

        //WINBASEAPI
        //__out_opt
        //HGLOBAL
        //WINAPI
        //GlobalAlloc (
        //    __in UINT uFlags,
        //    __in SIZE_T dwBytes
        //    );
        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalAlloc(
            [In] GMEM uFlags,
            [In] uint dwBytes);

        //WINBASEAPI
        //__out_opt
        //HGLOBAL
        //WINAPI
        //GlobalFree(
        //    __deref HGLOBAL hMem
        //    );
        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalFree(
            [In] IntPtr hMem);

        //WINBASEAPI
        //__out_opt
        //LPVOID
        //WINAPI
        //GlobalLock (
        //    __in HGLOBAL hMem
        //    );
        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalLock(
            [In] IntPtr hMem);

        //WINBASEAPI
        //BOOL
        //WINAPI
        //GlobalUnlock(
        //    __in HGLOBAL hMem
        //    );
        [DllImport("kernel32.dll")]
        public static extern bool GlobalUnlock(
            [In] IntPtr hMem);

        //__checkReturn WINOLEAPI CreateStreamOnHGlobal (IN HGLOBAL hGlobal, IN BOOL fDeleteOnRelease,
        //                                OUT LPSTREAM FAR* ppstm);
        [DllImport("ole32.dll")]
        public static extern int CreateStreamOnHGlobal(
            [In] IntPtr hGlobal,
            [In] bool fDeleteOnRelease,
            [Out, MarshalAs(UnmanagedType.Interface)] out object ppstm);
    }
}
