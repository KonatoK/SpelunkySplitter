using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky
{
    public class AutoSplitter : IDisposable
    {
        public SpelunkyHooks Hooks { get; private set; }
        public Category Category { get; private set; }
        ISegment[] Segments;

        public AutoSplitter(SpelunkyHooks hooks, Category category)
        {
            this.Hooks = hooks;
            this.Category = category;
            this.Segments = category.NewInstance();

            AssertHooksActive();
        }

        public SegmentStatus Update(LiveSplitState state)
        {
            AssertHooksActive();
            Console.WriteLine("Current Split Index = " + state.CurrentSplitIndex);
            return new SegmentStatus()
            {
                Type = SegmentStatusType.INFO,
                Message = "Debugging."
            };
        }

        void AssertHooksActive()
        {
            if(Hooks.Invalidated)
            {
                throw new Exception("Process exited unexpectedly");
            }
        }

        public void Dispose()
        {
            Hooks.Dispose();
        }
    }
}
