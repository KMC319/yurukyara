using System;
using UniRx;

namespace Tutorial {
    public class TutorialText : TextAnimationBase {
        private void Start() {
            transform.parent.gameObject.SetActive(false);
            Observable.Timer(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                    transform.parent.gameObject.SetActive(true));
        }
    }
}
