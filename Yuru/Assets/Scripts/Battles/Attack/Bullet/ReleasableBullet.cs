using UnityEngine;

namespace Battles.Attack {
	
	//リリースされるまで親に追従する特殊な弾
	//同じ階層にBullet継承した奴が必須
	public class ReleasableBullet : MonoBehaviour {
		private Bullet bullet;
		private Transform parent;

		private void Start() {
			bullet = GetComponent<Bullet>();
		}

		private void LateUpdate() {
			if (bullet.Initialized) {
				Destroy(this);
				return;
			}

			if (parent == null) return;
			transform.position = parent.position;
			transform.rotation = parent.rotation;
		}

		public void SetParent(GameObject obj) {
			parent = obj.transform;
		}
	}
}
