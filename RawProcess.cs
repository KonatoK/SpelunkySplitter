using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LiveSplit.Spelunky
{
    public class RawProcess : IDisposable
    {
        public string ProcessName { get; private set; }
        private IntPtr ProcessHandle;
        private Process Process;
        public int BaseAddress { get; private set; }
        public bool HasExited => Process.HasExited;
        public string FilePath => Process.MainModule.FileName;

        public RawProcess(string processName)
        {
            ProcessName = processName;

            var processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
                throw new Exception("Failed to find process by name: " + processName);

            var process = processes[0];
            Process = process;
            BaseAddress = process.MainModule.BaseAddress.ToInt32();
            ProcessHandle = Kernel32.OpenProcess(Kernel32.PROCESS_ALL_ACCESS, false, process.Id);
        }

        // returns an offset containing the signature match (if it exists)
        int? FindSignatureMatch(byte[] buf, int bufUsed, byte?[] signature)
        {
            var end = bufUsed - signature.Length + 1;
            for (var offs = 0; offs < end; ++offs)
            {
                for(var i = 0; i < signature.Length; ++i)
                {
                    byte? s = signature[i];
                    if(s.HasValue && s.Value != buf[i+offs]) { goto NoMatch; }
                }
                return offs;
                NoMatch: continue;
            }
            return null;
        }

        const int BUF_SCAN_SIZE = 4096;
        public int? FindBytes(byte?[] signature, int startAddr = 0)
        {
            IntPtr addr = (IntPtr)startAddr;
            var mbi = new Kernel32.MEMORY_BASIC_INFORMATION();
            byte[] buf = new byte[4096];

            while(Kernel32.VirtualQueryEx((int)ProcessHandle, addr, ref mbi, Kernel32.MBI_SIZE) > 0)
            {
                addr = mbi.BaseAddress;
                if(mbi.State != Kernel32.StateEnum.MEM_COMMIT)
                {
                    addr += (int)mbi.RegionSize;
                    continue;
                }

                int pageRemaining = (int)mbi.RegionSize;
                int bufUsed;
                do
                {
                    bufUsed = Math.Min(buf.Length, pageRemaining);
                    if (mbi.State == Kernel32.StateEnum.MEM_COMMIT)
                    {
                        int szRead = ReadBytes((int)addr, ref buf);
                        if (szRead == 0) { return null; }
                        var maybeOffs = FindSignatureMatch(buf, bufUsed, signature);
                        if (maybeOffs.HasValue) { return (int)addr + maybeOffs.Value; }
                    }
                    pageRemaining -= bufUsed;
                    addr += bufUsed;
                }
                while (pageRemaining > 0);
            }

            return null;
        }

        public int WriteInt32(int address, int value)
        {
            return WriteBytes(address, BitConverter.GetBytes(value));
        }

        public int WriteBool(int address, bool value)
        {
            return WriteBytes(address, BitConverter.GetBytes(value));
        }

        public int WriteDouble(int address, double value)
        {
            return WriteBytes(address, BitConverter.GetBytes(value));
        }

        public int WriteSingle(int address, float value)
        {
            return WriteBytes(address, BitConverter.GetBytes(value));
        }

        public int WriteBytes(int address, byte[] bytes)
        {
            AssertValid();
            int bytesWritten = 0;
            Kernel32.WriteProcessMemory((int)ProcessHandle, address, bytes, bytes.Length, ref bytesWritten);
            return bytesWritten;
        }

        public int ReadInt32(int address)
        {
            return BitConverter.ToInt32(ReadBytes(address, sizeof(int)), 0);
        }

        public bool ReadBool(int address)
        {
            return BitConverter.ToBoolean(ReadBytes(address, sizeof(bool)), 0);
        }

        public float ReadSingle(int address)
        {
            return BitConverter.ToSingle(ReadBytes(address, sizeof(float)), 0);
        }

        public double ReadDouble(int address)
        {
            return BitConverter.ToDouble(ReadBytes(address, sizeof(double)), 0);
        }
        
        public byte[] ReadBytes(int address, int count)
        {
            byte[] bytes = new byte[count];
            ReadBytes(address, ref bytes);
            return bytes;
        }

        public int ReadBytes(int address, ref byte[] bytes)
        {
            AssertValid();
            int bytesRead = 0;
            Kernel32.ReadProcessMemory((int)ProcessHandle, address, bytes, bytes.Length, ref bytesRead);
            return bytesRead;
        }

        public void AssertValid()
        {
            if (ProcessHandle == IntPtr.Zero)
                throw new ObjectDisposedException(nameof(RawProcess));
            else if (Process.HasExited)
                throw new Exception("Target process has exited");
        }

        public void Dispose()
        {
            if (ProcessHandle == IntPtr.Zero)
                return;
            Kernel32.CloseHandle((int)ProcessHandle);
            ProcessHandle = IntPtr.Zero;
        }
    }
}
