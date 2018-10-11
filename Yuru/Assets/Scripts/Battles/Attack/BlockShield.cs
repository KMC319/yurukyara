using System;
using Players;
using UniRx;
using UnityEngine;

namespace Battles.Attack{
	public class BlockShield :AttackTool{
		[SerializeField] private float guardTime;
		
		private PlayerGuardControll playerGuardControll;

		private void Start(){
			playerGuardControll = this.GetComponent<PlayerGuardControll>();
		}

		public override void On(){
			playerGuardControll.InGuard = true;
			Observable.Timer(TimeSpan.FromSeconds(guardTime)).Subscribe(n => playerGuardControll.InGuard = false);
		}

		public override void Off(){
			playerGuardControll.InGuard = false;
		}
	}
}