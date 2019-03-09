using System;
using System.Collections.Generic;
using System.Linq;
using Battles.Players;
using UnityEngine;
using UniRx;

namespace Tutorial {
    public class TutorialController : MonoBehaviour {
        [SerializeField] private TutorialText tutorialText;
        [SerializeField] private TutorialTimer tutorialTimer;
        [SerializeField] private CheckListManager checkListManager;
        [SerializeField] private TutorialData[] datas;

        private TutorialFlgCollider[] flgColliders;
        private Pausable pausable;
        private List<IPlayerBinder> iplayerBinders;

        private int state = 0;

        private void Start() {
            iplayerBinders = this.transform.GetComponentsInChildren<IPlayerBinder>().ToList();
            flgColliders = transform.GetComponentsInChildren<TutorialFlgCollider>();
            foreach (var flgCollider in flgColliders) {
                flgCollider.HitStream
                    .First()
                    .Subscribe(_ => NextTutorial());
            }

            pausable = this.GetComponent<Pausable>();

            tutorialTimer.EndStream.Subscribe(_ => NextTutorial());
            Observable.Timer(TimeSpan.FromSeconds(1))
                .Subscribe(_ => {
                    tutorialText.PlayStart(datas[state].Message);
                    Observable.Timer(TimeSpan.FromSeconds(1.5))
                        .Subscribe(__ => {
                            NextTutorial();
                            ChangeInputEnable(true);
                        });
                });
        }


        private void ChangeInputEnable(bool f) {
            foreach (var item in iplayerBinders) {
                item.SetInputEnable(f);
            }
        }

        private void NextTutorial() {
            checkListManager.DisplayCheckList(state);
            state++;
            if (datas[state].Time > 0) {
                tutorialTimer.ResetTimer(datas[state].Time);
            } else {
                tutorialTimer.Pause();
            }
            tutorialText.PlayStart(datas[state].Message);
        }

        [Serializable]
        private struct TutorialData {
            [MultilineAttribute(2)] public string Message;
            public float Time;
        }
    }
}
