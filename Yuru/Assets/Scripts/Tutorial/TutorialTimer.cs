using Battles.Systems;

namespace Tutorial {
    public class TutorialTimer : Timer{        
        public void ResetTimer(float time) {
            Pause();
            Set(time);
            TimerStart();
        }
    }
}
