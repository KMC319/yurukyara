using doma;
using UnityEngine;

namespace Battles.Attack{
	public class Grab : AttackTool{
		private bool isActive;

		private void Update(){
			if(!isActive)return;
			Target.gameObject.transform.position = transform.position;
			Target.DamageControll.InGrabed();
		}

		public override void On(){
			isActive = true;
		}

		public override void Off(){
			isActive = false;
			hitStream.OnNext(Target.gameObject);
		}
	}
}