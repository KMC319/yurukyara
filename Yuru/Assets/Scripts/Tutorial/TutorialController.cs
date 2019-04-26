using System;
using System.Collections.Generic;
using System.Linq;
using Systems.Chars;
using Battles.Players;
using Battles.Systems;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

namespace Tutorial {
    public class TutorialController : BattleManager {
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

        private void Update() {
            if (Input.GetButtonDown("Start")){
                pausable.Submit();
                tutorialTimer.ReverseMoveAble();
            }
        }


        private void ChangeInputEnable(bool f) {
            foreach (var item in iplayerBinders) {
                item.SetInputEnable(f);
            }
        }

        private void NextTutorial() {
            state++;
            if (datas.Length <= state) {
                BGMer.Instance.Delete();
                SceneManager.LoadScene("Start");
                return;
            }
            checkListManager.DisplayCheckList(state);
            tutorialTimer.ResetTimer(datas[state].Time);

            foreach (var message in datas[state].Messages) {
                tutorialText.PlayStart(message);
            }
        }

        public override void BreakGame(string scene) {
            SceneManager.LoadScene(scene);
        }

        [Serializable]
        private struct TutorialData {
            [MultilineAttribute(2)] public string[] Messages;
            public float Time;
        }
    }
}
