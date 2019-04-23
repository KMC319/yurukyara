using System;
using Battles.Animations;
using UnityEngine;

namespace Battles.Players{
	[Serializable]
	public class MotionBox:AnimBox{
		public MotionBox(AnimationClip anim_clip) : base(anim_clip){
		}
	}
}