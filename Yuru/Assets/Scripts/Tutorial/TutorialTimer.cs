using Battles.Systems;
using UnityEngine;

namespace Tutorial {
    public class TutorialTimer : Timer{        
        public void ResetTimer(float time) {
            if (time <= 0) time = 0.5f;
            Pause();
            Set(time);
            TimerStart();
        }
    }
}
