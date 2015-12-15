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

        public ReadOnlyProcess(string processName)
        {
            this.ProcessName = processName;

            var processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
                throw new Exception("Failed to find process by name: " + processName);

            // Process.Handle exists, but we want to enforce PROCESS_VM_READ / PROCESS_QUERY_INFORMATION permissions
            this.ProcessHandle = Kernel32.OpenProcess(Kernel32.PROCESS_VM_READ | Kernel32.PROCESS_QUERY_INFORMATION, false, processes[0].Id);
        }

        public int ReadInt32(Int32 address)
        {
            AssertUndisposed();
            return BitConverter.ToInt32(ReadBytes(address, sizeof(int)), 0);
        }

        public bool ReadBool(Int32 address)
        {
            AssertUndisposed();
            return BitConverter.ToBoolean(ReadBytes(address, sizeof(bool)), 0);
        }

        public double ReadDouble(Int32 address)
        {
            AssertUndisposed();
            return BitConverter.ToDouble(ReadBytes(address, sizeof(double)), 0);
        }
        
        public byte[] ReadBytes(Int32 address, int count)
        {
            AssertUndisposed();
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

        public void AssertUndisposed()
        {
            if (ProcessHandle == IntPtr.Zero)
                throw new ObjectDisposedException(nameof(ReadOnlyProcess));
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
