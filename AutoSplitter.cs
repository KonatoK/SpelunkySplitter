using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.IO;
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
        EnabledPatchContainer Patches;
        ISegment[] Segments;
        TimerModel Timer;
        string AutoSaveLoadOpt;
        bool SaveLoaded;

        public string SaveBackupPath => Hooks.GameDirectoryPath + @"\Data\spelunky_save.ss.bak";

        public AutoSplitter(SpelunkyHooks hooks, Category category, EnabledPatchContainer patches, TimerModel timer, string autoSaveLoadOpt)
        {
            Hooks = hooks;
            Category = category;
            Patches = patches;
            Segments = category.NewInstance();
            Timer = timer;
            AutoSaveLoadOpt = autoSaveLoadOpt;
            SaveLoaded = false;
            AssertHooksActive();
        }

        delegate void SplitAction();

        SegmentStatus UpdateAutoLoad(LiveSplitState state)
        {
            if (state.CurrentSplitIndex == -1)
            {
                if (AutoSaveLoadOpt != null && !SaveLoaded)
                {
                    if (Hooks.CurrentState != SpelunkyState.SPLASH_SCREEN)
                    {
                        return new SegmentStatus()
                        {
                            Type = SegmentStatusType.ERROR,
                            Message = "Return to the splash screen (before the main menu) to autoload the save."
                        };
                    }
                    else
                    {
                        try
                        {
                            var gameSavePath = Hooks.GameSavePath;
                            if (!File.Exists(SaveBackupPath)) { File.Copy(gameSavePath, SaveBackupPath); }
                            File.Copy(AutoSaveLoadOpt, gameSavePath, true);
                            SaveLoaded = true;
                            return null;
                        }
                        catch(Exception e)
                        {
                            return new SegmentStatus()
                            {
                                Type = SegmentStatusType.ERROR,
                                Message = "Failed to autoload: " + e.Message
                            };
                        }
                    }
                }
                return null;
            }
            else
            {
                SaveLoaded = false;
                return null;
            }
        }

        public SegmentStatus Update(LiveSplitState state)
        {
            AssertHooksActive();
            
            if(state.Run.Count != Segments.Length - 1) // Validate user splits
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.ERROR,
                    Message = "Expected " + (Segments.Length - 1) + " segments, got " + state.Run.Count + " (correct this before continuing)."
                };
            }
            else if(state.CurrentSplitIndex + 1 >= Segments.Length) // Check if the run is done
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.INFO,
                    Message = "Run completed!"
                };
            }
            else
            {
                var autoLoadResult = UpdateAutoLoad(state);
                if (autoLoadResult != null) { return autoLoadResult; }

                SplitAction splitAction; 
                if(state.CurrentSplitIndex == -1) { splitAction = delegate () { Timer.Start(); }; }
                else { splitAction = delegate () { Timer.Split(); }; }

                ISegment segment = Segments[state.CurrentSplitIndex + 1];
                var status = segment.CheckStatus(Hooks);
                if (segment.Cycle(Hooks)) { splitAction(); }
                return status;
            }
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
            Patches.Dispose();
            Hooks.Dispose();
        }
    }
}
