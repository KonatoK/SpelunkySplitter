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

    public class EnabledPatchContainer
    {
        List<IPatch> Patches;

        public delegate IPatch PatchInstantiator();

        public EnabledPatchContainer()
        {
            Patches = new List<IPatch>();
        }

        public bool TryAddAndEnable(PatchInstantiator inst)
        {
            try
            {
                var patch = inst();
                patch.Apply();
                Patches.Add(patch);
                return true;
            }
            catch (PatchInitializationFailedException e) {
                Console.WriteLine("Warning: Failed to initialize patch: " + e.Message);
                return false;
            }
        }

        public void RevertAll()
        {
            foreach(var patch in Patches)
            {
                patch.Revert();
            }
        }
    }
}
