using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    public class ForwardMoveAttack : AttackToolEntity, IMovableAttack {
        [SerializeField] private float speed;
        [SerializeField] private float moveTime;
        [SerializeField] private AttackToolEntity[] attackToolEntities;
        [SerializeField] private float attackEnableTime;
        private GameObject vcam2d;

        private Rigidbody rigidBody;

        public bool IsActive { get; private set; }

        private readonly Subject<Unit> cancelStream = new Subject<Unit>();

        private void Awake() {
            rigidBody = this.GetComponent<Rigidbody>();

            attackToolEntities.Select(n => n.HitStream).Merge().Subscribe(n => hitStream.OnNext(n));
        }

        private void Start() {
            vcam2d = GameObject.Find("vcam 2d");
        }

        public override void On() {
            My.CurrentMoveCotroll.LookLock = true;
            transform.LookAt(transform.position + My.CurrentMoveCotroll.lookTarget.forward * Math.Sign(speed));

            var timer = Observable.Timer(TimeSpan.FromSeconds(moveTime));
            IsActive = true;
            Observable.EveryUpdate()
                .TakeUntil(timer)
                .TakeUntil(cancelStream)
                .Subscribe(n => {
                        rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0)
                                             + vcam2d.transform.right * speed;
                        transform.LookAt(transform.position + vcam2d.transform.right * speed);
                    },
                    () => rigidBody.velocity = Vector3.zero
                );
            Observable.Timer(TimeSpan.FromSeconds(attackEnableTime))
                .TakeUntil(cancelStream)
                .Subscribe(n => {
                    foreach (var item in attackToolEntities) {
                        item.On();
                    }
                });
        }

        public override void Off(bool cancel = false) {
            My.CurrentMoveCotroll.LookLock = false;
            cancelStream.OnNext(Unit.Default);
            foreach (var item in attackToolEntities) {
                item.Off();
            }

            IsActive = false;
        }
    }
}
