using UnityEngine;

namespace Battles.Attack {
    public class ChaseBullet : Bullet {
        [SerializeField] private float speed;
        private ChaseBulletMother mother;
        private Rigidbody rigid;
        private GameObject target;

        public void Setup(ChaseBulletMother mom, GameObject targetObj) {
            mother = mom;
            target = targetObj;
        }

        private void Start() {
            rigid = GetComponentInChildren<Rigidbody>();
            Destroy(gameObject, 5f);
        }

        private void Update() {
            if (target != null) {
                var targetRotation = Quaternion.LookRotation(target.transform.position + new Vector3(0, 1f, 0) - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 / 4f);
            }
            rigid.velocity = transform.forward * speed;
        }

        private void OnTriggerEnter(Collider other) {
            mother.Hit(other);
            if (other.gameObject == target) Destroy(gameObject);
        }
    }
}
