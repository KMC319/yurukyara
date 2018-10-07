using UniRx;
using UnityEngine;

namespace Battles.Attack{
	public class AttackCollider : MonoBehaviour{

		private Subject<Collider> hitStream=new Subject<Collider>();
		public IObservable<Collider> HitStream=>hitStream;
		
		private void OnTriggerEnter(Collider other){
			hitStream.OnNext(other);
		}
	}
}