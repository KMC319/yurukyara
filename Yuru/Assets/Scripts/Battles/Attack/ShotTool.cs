using doma;
using UnityEngine;
using UniRx;

namespace Battles.Attack {
    public class ShotTool : AttackTool {
        [SerializeField] private Bullet bullet;

        private void Start() {
            if (bullet != null) bullet.HitStream.Subscribe(n => hitStream.OnNext(n.gameObject));
        }

        public override void On() {
            DebugLogger.Log(Target.gameObject);
            var obj = Instantiate(bullet.gameObject, transform.position, transform.rotation).GetComponent<Bullet>();
            obj.HitStream.Subscribe(n => hitStream.OnNext(n.gameObject)).AddTo(obj);
            if (obj.GetComponent<BulletFactory>() != null) obj.GetComponent<BulletFactory>().MyPos = transform.position;
        }

        public override void Off() { }
    }
}
