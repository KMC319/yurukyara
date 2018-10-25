using doma;
using UniRx;
using UnityEngine;

namespace Battles.Attack{
	public class AttackCollider : AttackToolEntity{
		private bool IsActive{ get; set; }
		private Collider myCollider;

		private void Start(){
			myCollider = this.GetComponent<Collider>();
			myCollider.enabled = false;
		}

		private void OnTriggerEnter(Collider other){
			if(!IsActive)return;
			hitStream.OnNext(other.gameObject);
		}

		public override void On(){
			IsActive = true;
			myCollider.enabled = true;
		}

		public override void Off(){
			IsActive = false;
			myCollider.enabled = false;
		}
	}
}