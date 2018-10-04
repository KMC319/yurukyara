using System;
using UnityEngine;

namespace Animations{
	[Serializable]
	public class AttackBox:AnimBox{
		public float rigorTime;
		public AttackBox[] nextAttack=new AttackBox[0];
		
		public AttackBox(AnimationClip anim_clip) : base(anim_clip){
		}
	}
}