using System;
using Battles.Players;
using UniRx;
using UnityEngine;

namespace Battles.Attack{
	public class BlockShield :AttackTool{
		[SerializeField] private float guardTime;
		[SerializeField] private AttackTool[] attackTools;
		
		private GuardControll guardControll;

		private void Start(){
			guardControll = this.GetComponent<GuardControll>();
		}

		public override void On(){
			guardControll.InGuard = true;
			Observable.Timer(TimeSpan.FromSeconds(guardTime)).Subscribe(n => {
				guardControll.InGuard = false;
				foreach (var item in attackTools){
					item.On();
				}
			});
		}

		public override void Off(){
			guardControll.InGuard = false;
			foreach (var item in attackTools){
				item.Off();
			}
		}
	}
}