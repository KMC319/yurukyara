using System;
using System.Collections.Generic;
using Animations;
using Battles.Attack;
using Players;
using UnityEngine;

namespace Battles.Health{
	[Serializable]
	public class AttackBox:AnimBox{
		public float enableTime;
		public List<PlayerKeyCode> keyCodes;
		public CommandType commandType;
		public AttackDamageBox attackDamageBox;
		public AttackBox[] nextAttack=new AttackBox[0];
		
		public AttackBox(AnimationClip anim_clip) : base(anim_clip){
		}
	}
}