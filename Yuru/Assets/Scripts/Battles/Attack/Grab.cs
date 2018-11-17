using System;
using doma;
using UnityEngine;

namespace Battles.Attack{
	//実装が汚い、Targetに依存しすぎ
	public class Grab : AttackToolEntity{
		private bool grabActive;
		private bool coliderActive;

		private Collider myCollider;

		private void Awake(){
			try{
				myCollider = this.GetComponent<Collider>();
				myCollider.enabled = false;
			}catch (Exception e){
				DebugLogger.LogError(e+".in "+gameObject.name);
				throw;
			}
		}

		private void Update(){
			if(!grabActive)return;
			Target.gameObject.transform.position = transform.position;
			Target.DamageControll.InGrabed();
		}

		public override void On(){
			coliderActive = true;
			myCollider.enabled = true;
		}

		public override void Off(bool cancel = false){
			if (grabActive){
				hitStream.OnNext(Target.gameObject);
			}
			grabActive = false;
			coliderActive = false;
			myCollider.enabled = false;
		}
		
		
		private void OnTriggerEnter(Collider other){
			if(!coliderActive||grabActive)return;
			hitStream.OnNext(other.gameObject);
			if (other.gameObject == Target.gameObject){ grabActive = true; }
		}
	}
}