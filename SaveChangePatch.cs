using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky
{
    public class SaveChangePatch : IPatch
    {
        static string SpelunkySave = "spelunky_save";
        static string SplitterSave = "splitter_save";
        static readonly byte?[] SearchSignature = SpelunkySave.ToCharArray().Select(c => (byte?)c).ToArray();
        static readonly byte[] RevertValue = SpelunkySave.ToCharArray().Select(c => (byte)c).ToArray();
        static readonly byte[] OverwriteValue = SplitterSave.ToCharArray().Select(c => (byte)c).ToArray();

        SpelunkyHooks Spelunky;
        List<int> PatchAddresses;

        public SaveChangePatch(SpelunkyHooks spelunky)
        {
            PatchAddresses = new List<int>();
            Spelunky = spelunky;
            FindPatchAddresses();
        }

        void FindPatchAddresses()
        {
            int? currentAddress = -1;
            while((currentAddress = Spelunky.Process.FindBytes(SearchSignature, currentAddress.Value + 1)).HasValue)
                PatchAddresses.Add(currentAddress.Value);
            if(PatchAddresses.Count == 0) { throw new PatchInitializationFailedException("Failed to find any patch offsets for save change patch"); }
        }

        public void Apply()
        {
            foreach(var address in PatchAddresses)
                Spelunky.Process.WriteBytes(address, OverwriteValue);
        }

        public void Revert()
        {
            foreach(var address in PatchAddresses)
                Spelunky.Process.WriteBytes(address, RevertValue);
        }
    }
}
