using System;
using System.Collections.Generic;
using System.Linq;
using Battles.Animations;
using Battles.Attack;
using UniRx;
using UnityEngine;

namespace Battles.Players {
    [RequireComponent(typeof(BoxContainer))]
    [RequireComponent(typeof(PlayAbleController))]
    public class AttackAnimControll : MonoBehaviour {
        private PlayAbleController playAbleController;
        private readonly Subject<AnimResponce> responseStream = new Subject<AnimResponce>();
        public UniRx.IObservable<AnimResponce> ResponseStream => responseStream;

        private AnimBox current;

        private void Start() {
            playAbleController = this.GetComponent<PlayAbleController>();

            playAbleController.PlayEndStream.Subscribe(FlowResponce);
        }

        public void ChangeAnim(AnimBox anim_box) {
            if (current == anim_box) return;
            playAbleController.TransAnimation(anim_box);
            current = anim_box;
        }

        public void CashClear() {
            current = null;
        }

        public void ForceChangeAnim(AnimBox anim_box) {
            playAbleController.TransAnimation(anim_box);
        }

        private void FlowResponce(AnimBox anim_box) {
            if (anim_box is AttackBox) {
                Observable.Timer(TimeSpan.FromSeconds(anim_box.delayTime))
                    .Subscribe(_ => responseStream.OnNext(AnimResponce.AttackEnd));
            }
        }
    }
}
