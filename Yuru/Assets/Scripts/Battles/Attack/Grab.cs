using doma;
using UnityEngine;

namespace Battles.Attack{
	//実装が汚い、Targetに依存しすぎ
	public class Grab : AttackTool{
		private bool grabActive;
		private bool coliderActive; 
		
		private void Update(){
			if(!grabActive)return;
			Target.gameObject.transform.position = transform.position;
			Target.DamageControll.InGrabed();
		}

		public override void On(){
			coliderActive = true;
		}

		public override void Off(){
			grabActive = false;
			coliderActive = false;
			hitStream.OnNext(Target.gameObject);
		}
		
		
		private void OnTriggerEnter(Collider other){
			if(!coliderActive||grabActive)return;
			hitStream.OnNext(other.gameObject);
			if (other.gameObject == Target.gameObject){ grabActive = true; }
		}
	}
}