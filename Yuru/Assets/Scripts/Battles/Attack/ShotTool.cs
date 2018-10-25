using doma;
using UnityEngine;
using UniRx;

namespace Battles.Attack {
    public class ShotTool : AttackToolEntity{
        [SerializeField] private Bullet bullet;

        private void Start() {
            if (bullet != null) bullet.HitStream.Subscribe(n => hitStream.OnNext(n.gameObject));
        }

        public override void On() {
            var obj = Instantiate(bullet.gameObject, transform.position, transform.rotation).GetComponent<Bullet>();
            obj.HitStream.Subscribe(n => hitStream.OnNext(n.gameObject)).AddTo(obj);
        }

        public override void Off(){
           
        }
    }
}
