using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Animations{
	[Serializable]
	public class AnimBox{
		public AnimationClip clip;
		public bool loop;
		public AnimBox[] autoAdvance;

		public AnimBox(AnimationClip anim_clip){
			clip = anim_clip;
		}
		public AnimBox(AnimationClip anim,AnimBox anim_box):this(anim){
			autoAdvance=new AnimBox[0];
			autoAdvance[0] = anim_box;
		}
	}
}