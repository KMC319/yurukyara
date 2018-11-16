using UnityEngine;

namespace Battles.Attack {
	public class StraightBullet : Bullet {
		[SerializeField] private float speed;
		private BulletFactory mother;
		private Rigidbody rigid;
		private GameObject target;

		private float destroyCount;
		private bool isPausing;
		
		public override void Setup(BulletFactory mom, GameObject targetObj) {
			rigid = gameObject.AddComponent<Rigidbody>();
			rigid.useGravity = false;
			rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			mother = mom;
			target = targetObj;
			transform.LookAt(targetObj.transform.position + new Vector3(0, 1, 0));
			rigid.velocity = transform.forward * speed;
			Initialized = true;
		}

		private void Update() {
			if (!Initialized) return;
			if (isPausing) return;
			destroyCount += Time.deltaTime;
			if (destroyCount >= 5) Destroy(gameObject);
		}

		private void FixedUpdate() {
			if (!Initialized) return;
			if (isPausing) return;
			RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, speed * Time.deltaTime);
			if(hits.Length <= 0) return;
			foreach (var hit in hits) {
				OnTriggerEnter(hit.collider);
			}
		}

		private void OnTriggerEnter(Collider other) {
	        if(!Initialized) return;
            mother.Hit(other);
            if (other.gameObject == target) Destroy(gameObject);
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
			rigid.velocity = transform.forward * speed;
		}
	}
}
