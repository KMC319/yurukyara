using System;
using System.Collections.Generic;
using Animations;
using Battles.Attack;
using Players;
using UnityEngine;

namespace Battles.Health{
	[Serializable]
	public class AttackBox:AnimBox{
		public float bufferTime;
		public List<PlayerKeyCode> keyCodes;
		public CommandType commandType;
		public AttackCollider[] attackColliders;
		public AttackDamageBox attackDamageBox;
		public AttackBox[] nextAttack=new AttackBox[0];
		
		public AttackBox(AnimationClip anim_clip) : base(anim_clip){
		}

		public void ColliderOn(){
			foreach (var item in attackColliders){
				item.IsActive = true;
			}
		}
		
		public void ColliderOff(){
			foreach (var item in attackColliders){
				item.IsActive = false;
			}
		}
	}
}