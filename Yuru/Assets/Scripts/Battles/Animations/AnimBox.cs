using System;
using UnityEngine;

namespace Battles.Animations{
	[Serializable]
	public abstract class AnimBox{
		public AnimationClip clip;
		public float delayTime;
		public AnimBox(AnimationClip anim_clip){
			clip = anim_clip;
		}
	}
}