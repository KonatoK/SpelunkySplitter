using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LiveSplit.Spelunky
{
    public class ReadOnlyProcess : IDisposable
    {
        public string ProcessName { get; private set; }
        private IntPtr ProcessHandle;
        private Process Process;
        public Int32 BaseAddress { get; private set; }
        public bool HasExited => Process.HasExited;
        public string FilePath => Process.MainModule.FileName;

        public ReadOnlyProcess(string processName)
        {
            this.ProcessName = processName;

            var processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
                throw new Exception("Failed to find process by name: " + processName);

            var process = processes[0];
            this.Process = process;
            this.BaseAddress = process.MainModule.BaseAddress.ToInt32();
            this.ProcessHandle = Kernel32.OpenProcess(Kernel32.PROCESS_VM_READ | Kernel32.PROCESS_QUERY_INFORMATION, false, process.Id);
        }

        public int ReadInt32(Int32 address)
        {
            AssertValid();
            return BitConverter.ToInt32(ReadBytes(address, sizeof(int)), 0);
        }

        public bool ReadBool(Int32 address)
        {
            AssertValid();
            return BitConverter.ToBoolean(ReadBytes(address, sizeof(bool)), 0);
        }

        public double ReadDouble(Int32 address)
        {
            AssertValid();
            return BitConverter.ToDouble(ReadBytes(address, sizeof(double)), 0);
        }
        
        public byte[] ReadBytes(Int32 address, int count)
        {
            AssertValid();
            byte[] bytes = new byte[count];
            ReadBytes(address, ref bytes);
            return bytes;
        }

        public int ReadBytes(Int32 address, ref byte[] bytes)
        {
            int bytesRead = 0;
            Kernel32.ReadProcessMemory((int)ProcessHandle, address, bytes, bytes.Length, ref bytesRead);
            return bytesRead;
        }

        public void AssertValid()
        {
            if (ProcessHandle == IntPtr.Zero)
                throw new ObjectDisposedException(nameof(ReadOnlyProcess));
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
