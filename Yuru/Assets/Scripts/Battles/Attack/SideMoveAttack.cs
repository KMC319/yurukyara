using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    //ほぼコピペコードになっちゃってるけど絶対拡張されない２クラスのために基底作りたくなかった
    public class SideMoveAttack : AttackToolEntity {
        [SerializeField] private float speed;
        [SerializeField] private float moveTime;
        [SerializeField] private AttackToolEntity[] attackToolEntities;
        [SerializeField] private float attackEnableTime;

        private Rigidbody rigidBody;

        private readonly Subject<Unit> cancelStream=new Subject<Unit>();
        private void Awake() {
            rigidBody = this.GetComponent<Rigidbody>();

            attackToolEntities.Select(n => n.HitStream).Merge().Subscribe(n => hitStream.OnNext(n));
        }

        public override void On() {
            var timer = Observable.Timer(TimeSpan.FromSeconds(moveTime));
            
            Observable.EveryUpdate()
                .TakeUntil(timer)
                .TakeUntil(cancelStream)
                .Subscribe(n => {
                    rigidBody.velocity =new Vector3(0, rigidBody.velocity.y, 0)
                                        + My.CurrentMoveCotroll.lookTarget.right*speed;
                });
            Observable.Timer(TimeSpan.FromSeconds(attackEnableTime))
                .TakeUntil(cancelStream)
                .Subscribe(n => {
                    foreach (var item in attackToolEntities) {
                        item.On();
                    }
                });

        }

        public override void Off(bool cancel = false) {
            cancelStream.OnNext(Unit.Default);
            foreach (var item in attackToolEntities) {
                item.Off();
            }
        }
    }
}