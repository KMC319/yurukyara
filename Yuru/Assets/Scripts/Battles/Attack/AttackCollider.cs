using Battles.Effects;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    public class AttackCollider : AttackToolEntity {
        private bool IsActive { get; set; }
        private Collider myCollider;

        private void Start() {
            myCollider = this.GetComponent<Collider>();
            myCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other) {
            if (!IsActive || other.CompareTag("Stage")) return;
            hitStream.OnNext(other.gameObject);
            HitEffectFactory.Instance.InstantiateEffect(null, this.gameObject.transform.position, this.gameObject.transform.rotation);
        }

        public override void On() {
            IsActive = true;
            myCollider.enabled = true;
        }

        public override void Off(bool cancel = false) {
            IsActive = false;
            myCollider.enabled = false;
        }
    }
}
