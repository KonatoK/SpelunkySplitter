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
        TimerModel Timer;

        public AutoSplitter(SpelunkyHooks hooks, Category category, TimerModel timer)
        {
            this.Hooks = hooks;
            this.Category = category;
            this.Segments = category.NewInstance();
            this.Timer = timer;
            AssertHooksActive();
        }

        delegate void SplitAction();

        public SegmentStatus Update(LiveSplitState state)
        {
            AssertHooksActive();

            SplitAction splitAction; 
            if(state.CurrentSplitIndex == -1) { splitAction = delegate () { Timer.Start(); }; }
            else { splitAction = delegate () { Timer.Split(); }; }

            ISegment segment = Segments[state.CurrentSplitIndex + 1];
            var status = segment.CheckStatus(Hooks);
            if (segment.Cycle(Hooks))
                splitAction();

            return status;
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
