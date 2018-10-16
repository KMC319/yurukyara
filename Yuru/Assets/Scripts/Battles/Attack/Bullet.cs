using UniRx;
using UnityEngine;

namespace Battles.Attack{
	public abstract class Bullet : MonoBehaviour{
		protected readonly Subject<Collider> hitStream=new Subject<Collider>();
		public IObservable<Collider> HitStream=>hitStream;

		private void OnTriggerEnter(Collider other){
			hitStream.OnNext(other);
		}
	}
}