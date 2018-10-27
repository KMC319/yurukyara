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
			transform.LookAt(targetObj.transform.position + new Vector3(0, 1, 0));
		}

		private void Start() {
			rigid = GetComponentInChildren<Rigidbody>();
			rigid.velocity = transform.forward * speed;
			Destroy(gameObject, 5f);
		}

		private void Update() {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime)) {
				OnTriggerEnter(hit.collider);
			}
		}

		private void OnTriggerEnter(Collider other) {
			Debug.Log(other.gameObject);
			mother.Hit(other);
		}
	}
}
