using UnityEngine;

namespace Battles.Attack {
	public class StraightBullet : Bullet {
		[SerializeField] private float speed;
		private StraightBulletFactory mother;
		private Rigidbody rigid;
		private GameObject target;

		public void Setup(StraightBulletFactory mom, GameObject targetObj) {
			mother = mom;
			target = targetObj;
			transform.LookAt(targetObj.transform);
		}

		private void Start() {
			rigid = GetComponentInChildren<Rigidbody>();
			rigid.velocity = transform.forward * speed;
			Destroy(gameObject, 5f);
		}

		private void OnTriggerEnter(Collider other) {
			mother.Hit(other);
			if (other.gameObject == target) Destroy(gameObject);
		}
	}
}
