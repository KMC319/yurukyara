using System;
using Battles.Players;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Attack{
	public abstract class AttackTool : MonoBehaviour{
		protected Subject<GameObject> hitStream=new Subject<GameObject>();
		public UniRx.IObservable<GameObject> HitStream=>hitStream;

		private PlayerRoot target;

		protected PlayerRoot Target{
			get{
				if (target == null){
					IPlayerBinder myBinder;
					myBinder = this.GetComponent<IPlayerBinder>();
					var current = transform;
					while (myBinder==null){
						var parent = current.parent;
						if(parent==null){break;}
						myBinder = parent.GetComponent<IPlayerBinder>();
						current = parent;
					}

					try{
						target = myBinder.TargetPlayerRoot;
					} catch (Exception e){
						DebugLogger.Log("Missing target");
						throw;
					}
				}
				return target;
			}
		}
		public abstract void On();
		public abstract void Off();
	}
}