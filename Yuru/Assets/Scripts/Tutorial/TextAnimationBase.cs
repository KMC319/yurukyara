using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial {
    public abstract class TextAnimationBase : MonoBehaviour {
        protected List<string> messageList = new List<string>();
        [SerializeField] private Text text;
        [SerializeField, Range(0.001f, 0.3f)] private float interval = 0.05f;
        private readonly float blankTime = 1.5f;
        private float time;

        private Subject<Unit> endStream = new Subject<Unit>();
        public UniRx.IObservable<Unit> EndStream => endStream;
        private bool isActive;

        protected virtual void Update() {
            if (!isActive) return;
            Play();
        }

        private void Play() {
            if (messageList.Count < 1) {
                isActive = false;
                return;
            }

            bool isEnd = messageList[0].Length < 1;
            if (isEnd) {
                isActive = false;
                if (messageList.Count > 1) {
                    PlayNext();
                } else {
                    messageList.RemoveAt(0);
                    Observable.Timer(TimeSpan.FromSeconds(blankTime))
                        .Subscribe(_ => endStream.OnNext(Unit.Default));
                }
            } else {
                time -= Time.deltaTime;
                if (time < 0) {
                    time = interval;
                    text.text += messageList[0].Substring(0, 1);
                    messageList[0] = messageList[0].Substring(1);
                }
            }
        }

        private void PlayNext() {
            messageList.RemoveAt(0);
            Observable.Timer(TimeSpan.FromSeconds(blankTime))
                .Subscribe(_ => {
                    isActive = true;
                    text.text = "";
                });
        }

        public void PlayStart(string message) {
            isActive = true;
            messageList.Add(message);
            text.text = "";
        }
    }
}
