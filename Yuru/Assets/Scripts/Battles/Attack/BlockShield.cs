using System;
using Battles.Players;
using UniRx;
using UnityEngine;

namespace Battles.Attack{
	public class BlockShield :AttackToolEntity{
		[SerializeField] private float guardTime;
		[SerializeField] private IAttackTool[] attackTools;
		
		private GuardControll guardControll;

		private bool isActive;
		
		private void Start(){
			guardControll = this.GetComponent<GuardControll>();
		}

		private void Update(){
			if (!isActive)return;
			guardControll.Guard();
		}

		public override void On(){
			isActive = true;
			Observable.Timer(TimeSpan.FromSeconds(guardTime)).Subscribe(n => {
				isActive = false;
				foreach (var item in attackTools){
					item.On();
				}
			});
		}

		public override void Off(){
			isActive = false;
			foreach (var item in attackTools){
				item.Off();
			}
		}
	}
}