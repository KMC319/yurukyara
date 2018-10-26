using UnityEngine;

namespace Battles.Attack {
	public class StraightBulletFactory : BulletFactory {
		protected override void Create(BulletInfo t) {
			if(!(t.Bullet is StraightBullet))return;
			if(!isActive) return;
			var a = Instantiate(t.Bullet, transform.position + transform.forward * t.SpawnLocalPos.z + transform.right * t.SpawnLocalPos.x + transform.up * t.SpawnLocalPos.y, Quaternion.Euler(transform.rotation.eulerAngles + t.SpawnLocalRota));
			a.GetComponent<StraightBullet>().Setup(this, Target.gameObject);
			RegisterBullet(a.gameObject);
		}
	}
}