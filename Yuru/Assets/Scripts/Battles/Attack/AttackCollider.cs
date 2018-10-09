using UniRx;
using UnityEngine;

namespace Battles.Attack{
	public class AttackCollider : MonoBehaviour{

		private Subject<Collider> hitStream=new Subject<Collider>();
		public IObservable<Collider> HitStream=>hitStream;
		
		public bool IsActive{ get; set; }
		
		private void OnTriggerEnter(Collider other){
			if(!IsActive)return;
			hitStream.OnNext(other);
		}
	}
}