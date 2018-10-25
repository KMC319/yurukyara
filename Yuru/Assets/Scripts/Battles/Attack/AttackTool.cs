using System;
using Battles.Players;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Attack{
	public abstract class AttackTool : MonoBehaviour{
		protected Subject<GameObject> hitStream=new Subject<GameObject>();
		public UniRx.IObservable<GameObject> HitStream=>hitStream;

		private IPlayerBinder myBinder;

		
		//どうしてもキャラのコンポーネントを触る場合には使う、余り使うのはよろしくない
		private IPlayerBinder MyBinder{
			get{
				if (myBinder == null){
					myBinder = this.GetComponent<IPlayerBinder>();
					var current = transform;
					while (myBinder==null){
						var parent = current.parent;
						if(parent==null){break;}
						myBinder = parent.GetComponent<IPlayerBinder>();
						current = parent;
					}
				}
				return myBinder;
			}
		}

		protected PlayerRoot My => MyBinder.PlayerRoot;
		protected PlayerRoot Target => MyBinder.TargetPlayerRoot;
		
		
		public abstract void On();
		public abstract void Off();
	}
}