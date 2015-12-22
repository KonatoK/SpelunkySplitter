using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky
{
    public class PatchInitializationFailedException : Exception
    {
        public PatchInitializationFailedException() : base("Patch initialization failed") { }
        public PatchInitializationFailedException(string message) : base(message) { }
        public PatchInitializationFailedException(string message, Exception inner) : base(message, inner) { }
    }

    public interface IPatch
    {
        void Apply();
        void Revert();
    }

    public class EnabledPatchContainer : IDisposable
    {
        List<IPatch> Patches;

        public delegate IPatch PatchInstantiator();

        public EnabledPatchContainer()
        {
            Patches = new List<IPatch>();
        }

        public bool TryAdd(PatchInstantiator inst)
        {
            try
            {
                Patches.Add(inst());
                return true;
            }
            catch (PatchInitializationFailedException e) {
                Console.WriteLine("Warning: Failed to initialize patch: " + e.Message);
                return false;
            }
        }

        public void Dispose()
        {
            foreach(var patch in Patches)
            {
                patch.Revert();
            }
        }
    }
}
