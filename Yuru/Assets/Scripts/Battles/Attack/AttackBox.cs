using System;
using Animations;
using Battles.Attack;
using Players;
using UnityEngine;

namespace Battles.Health{
	[Serializable]
	public class AttackBox:AnimBox{
		public float rigorTime;
		public float enableTime;
		public bool endFg;
		public AttackDamageBox attackDamageBox;
		public AttackBox[] nextAttack=new AttackBox[0];
		
		public AttackBox(AnimationClip anim_clip) : base(anim_clip){
		}
	}
}