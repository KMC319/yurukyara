using UniRx;
using UnityEngine;

namespace Battles.Attack{
	public abstract class AttackTool : MonoBehaviour{
		protected Subject<Collider> hitStream=new Subject<Collider>();
		public IObservable<Collider> HitStream=>hitStream;
		
		public abstract void On();
		public abstract void Off();
	}
}