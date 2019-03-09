using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial {
    public abstract class TextAnimationBase : MonoBehaviour {
        protected List<string> messageList = new List<string>();
        [SerializeField] private Text text;
        [SerializeField, Range(0.001f, 0.3f)] private float interval = 0.05f;
        private float time;

        private Subject<Unit> endStream = new Subject<Unit>();
        public IObservable<Unit> EndStream => endStream;
        public bool IsPlaying { get; protected set; }

        protected virtual void Update() {
            if (!IsPlaying) return;
            Play();
        }

        private void Play() {
            if (messageList.Count < 1) return;
            bool isEnd = messageList[0].Length < 1;
            if (isEnd) {
                messageList.RemoveAt(0);
                IsPlaying = false;
                endStream.OnNext(Unit.Default);
            } else {
                time -= Time.deltaTime;
                if (time < 0) {
                    time = interval;
                    text.text += messageList[0].Substring(0, 1);
                    messageList[0] = messageList[0].Substring(1);
                }
            }
        }
        
        public void PlayStart(string message) {
            IsPlaying = true;
            messageList.Add(message);
            text.text = "";
        }
    }
}
