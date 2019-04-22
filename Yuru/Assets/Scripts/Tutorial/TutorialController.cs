using System;
using System.Collections.Generic;
using System.Linq;
using Systems.Chars;
using Battles.Players;
using UnityEngine;
using UniRx;

namespace Tutorial {
    public class TutorialController : MonoBehaviour {
        [SerializeField] private TutorialText tutorialText;
        [SerializeField] private TutorialTimer tutorialTimer;
        [SerializeField] private CheckListManager checkListManager;
        [SerializeField] private TutorialData[] datas;
        [SerializeField] private GameObject root;

        private Pausable pausable;
        private List<IPlayerBinder> iplayerBinders;

        private int state;

        private void Start() {
            iplayerBinders = this.transform.GetComponentsInChildren<IPlayerBinder>().ToList();
            pausable = this.GetComponent<Pausable>();

            Observable.Zip(tutorialText.EndStream, tutorialTimer.EndStream)
                .Subscribe(_ => NextTutorial());
            state = -1;
            Observable.Timer(TimeSpan.FromSeconds(1))
                .Subscribe(_ => {
                    ChangeInputEnable(true);
                    NextTutorial();
                });
        }


        private void ChangeInputEnable(bool f) {
            foreach (var item in iplayerBinders) {
                item.SetInputEnable(f);
            }
        }

        private void NextTutorial() {
            state++;
            checkListManager.DisplayCheckList(state);
            tutorialTimer.ResetTimer(datas[state].Time);

            foreach (var message in datas[state].Messages) {
                tutorialText.PlayStart(message);
            }
        }

        [Serializable]
        private struct TutorialData {
            [MultilineAttribute(2)] public string[] Messages;
            public float Time;
        }
    }
}
