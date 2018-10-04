using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Animations{
	[Serializable]
	public abstract class AnimBox{
		public AnimationClip clip;
		public float delayTime;
		public AnimBox(AnimationClip anim_clip){
			clip = anim_clip;
		}
	}
}