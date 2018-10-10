using UniRx;
using UnityEngine;

namespace Battles.Attack{
	public class AttackCollider : AttackTool{
		
		private bool IsActive{ get; set; }
		
		private void OnTriggerEnter(Collider other){
			if(!IsActive)return;
			hitStream.OnNext(other);
		}

		public override void On(){
			IsActive = true;
		}

		public override void Off(){
			IsActive = false;
		}
	}
}