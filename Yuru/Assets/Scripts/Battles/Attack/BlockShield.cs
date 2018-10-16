using System;
using Battles.Players;
using UniRx;
using UnityEngine;

namespace Battles.Attack{
	public class BlockShield :AttackTool{
		[SerializeField] private float guardTime;
		
		private GuardControll guardControll;

		private void Start(){
			guardControll = this.GetComponent<GuardControll>();
		}

		public override void On(){
			guardControll.InGuard = true;
			Observable.Timer(TimeSpan.FromSeconds(guardTime)).Subscribe(n => guardControll.InGuard = false);
		}

		public override void Off(){
			guardControll.InGuard = false;
		}
	}
}