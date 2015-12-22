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
}
