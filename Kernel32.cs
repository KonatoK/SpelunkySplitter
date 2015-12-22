using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace LiveSplit.Spelunky
{
    public static class Kernel32
    {
        // http://www.pinvoke.net/default.aspx/Structures/MEMORY_BASIC_INFORMATION.html
        public enum AllocationProtectEnum : uint
        {
            PAGE_EXECUTE = 0x00000010,
            PAGE_EXECUTE_READ = 0x00000020,
            PAGE_EXECUTE_READWRITE = 0x00000040,
            PAGE_EXECUTE_WRITECOPY = 0x00000080,
            PAGE_NOACCESS = 0x00000001,
            PAGE_READONLY = 0x00000002,
            PAGE_READWRITE = 0x00000004,
            PAGE_WRITECOPY = 0x00000008,
            PAGE_GUARD = 0x00000100,
            PAGE_NOCACHE = 0x00000200,
            PAGE_WRITECOMBINE = 0x00000400
        }

        // http://www.pinvoke.net/default.aspx/Structures/MEMORY_BASIC_INFORMATION.html
        public enum StateEnum : uint
        {
            MEM_COMMIT = 0x1000,
            MEM_FREE = 0x10000,
            MEM_RESERVE = 0x2000
        }

        // http://www.pinvoke.net/default.aspx/Structures/MEMORY_BASIC_INFORMATION.html
        public enum TypeEnum : uint
        {
            MEM_IMAGE = 0x1000000,
            MEM_MAPPED = 0x40000,
            MEM_PRIVATE = 0x20000
        }

        // http://www.pinvoke.net/default.aspx/Structures/MEMORY_BASIC_INFORMATION.html
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public AllocationProtectEnum AllocationProtect;
            public IntPtr RegionSize;
            public StateEnum State;
            public AllocationProtectEnum Protect;
            public TypeEnum Type;
        }

        public static readonly uint MBI_SIZE = (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION));

        public const int PROCESS_CREATE_PROCESS = 0x0080;
        public const int PROCESS_CREATE_THREAD = 0x0002;
        public const int PROCESS_DUP_HANDLE = 0x0040;
        public const int PROCESS_QUERY_INFORMATION = 0x0400;
        public const int PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;
        public const int PROCESS_SET_INFORMATION = 0x0200;
        public const int PROCESS_SET_QUOTA = 0x0100;
        public const int PROCESS_SUSPEND_RESUME = 0x0800;
        public const int PROCESS_TERMINATE = 0x0001;
        public const int PROCESS_VM_OPERATION = 0x0008;
        public const int PROCESS_VM_READ = 0x0010;
        public const int PROCESS_VM_WRITE = 0x0020;
        public const int SYNCHRONIZE = 0x00100000;
        public const int PROCESS_ALL_ACCESS =
            PROCESS_CREATE_PROCESS | PROCESS_CREATE_THREAD | PROCESS_DUP_HANDLE |
            PROCESS_QUERY_INFORMATION | PROCESS_QUERY_LIMITED_INFORMATION | PROCESS_QUERY_INFORMATION |
            PROCESS_SET_INFORMATION | PROCESS_SET_QUOTA | PROCESS_SUSPEND_RESUME | PROCESS_TERMINATE |
            PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE | SYNCHRONIZE;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(int hProcess, IntPtr lpAddress, ref MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(int hObject);

        [DllImport("kernel32.dll")]
        public static extern int GetLastError();
    }
}
