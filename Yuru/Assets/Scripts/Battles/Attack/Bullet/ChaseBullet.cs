using Battles.Effects;
using UnityEngine;

namespace Battles.Attack {
    public class ChaseBullet : Bullet {
        [SerializeField] private float speed;
        [SerializeField] private float chaseTime;
        private BulletFactory mother;
        private Rigidbody rigid;
        private GameObject target;

        private bool isPausing;

        public override void Setup(BulletFactory mom, GameObject targetObj) {
            rigid = gameObject.AddComponent<Rigidbody>();
            rigid.useGravity = false;
            rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            mother = mom;
            target = targetObj;
            rigid.velocity = transform.forward * speed;
            Destroy(gameObject, 10f);
            Initialized = true;
        }

        private void FixedUpdate() {
            if (isPausing) return;
            if (chaseTime > 0 && target != null) {
                chaseTime -= Time.deltaTime;
                var targetRotation = Quaternion.LookRotation(target.transform.position + new Vector3(0, 1f, 0) - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 / 2f);
            }

            rigid.velocity = transform.forward * speed;
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, speed * Time.deltaTime);
            if (hits.Length <= 0) return;
            foreach (var hit in hits) {
                OnTriggerEnter(hit.collider);
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (!Initialized) return;
            mother.Hit(other);
            if (other.gameObject == target) {
                HitEffectFactory.Instance.InstantiateEffect(null, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        public override void Pause() {
            isPausing = true;
            if (!Initialized) return;
            rigid.Sleep();
        }

        public override void Resume() {
            isPausing = false;
            if (!Initialized) return;
            rigid.WakeUp();
        }
    }
}
