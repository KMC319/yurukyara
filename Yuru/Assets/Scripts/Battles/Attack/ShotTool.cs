using UnityEngine;
using UniRx;

namespace Battles.Attack {
    public class ShotTool : AttackTool {
        [SerializeField] private Bullet bullet;

        private void Start() {
            if (bullet != null) bullet.HitStream.Subscribe(n => hitStream.OnNext(n));
        }

        public override void On() {
            var obj = Instantiate(bullet.gameObject, transform.position, transform.rotation).GetComponent<Bullet>();
            obj.HitStream.Subscribe(n => hitStream.OnNext(n)).AddTo(obj);
            if (obj.GetComponent<BulletFactory>() != null) obj.GetComponent<BulletFactory>().MyPos = transform.position;
        }

        public override void Off() { }
    }
}
