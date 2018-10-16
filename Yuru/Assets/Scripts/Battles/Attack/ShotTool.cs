using UnityEngine;
using UniRx;

namespace Battles.Attack{
	public class ShotTool : AttackTool{
		[SerializeField] private Bullet bullet;

		private void Start(){
			if(bullet!=null)bullet.HitStream.Subscribe(n => hitStream.OnNext(n));
		}

		public override void On(){
			Instantiate(bullet.gameObject,transform.position,transform.rotation);
		}

		public override void Off(){
			
		}
	}
}