using System;
using System.Collections.Generic;
using Animations;
using Battles.Attack;
using Battles.Systems;
using Players;
using UnityEngine;

namespace Battles.Health{
	[Serializable]
	public class AttackBox:AnimBox{
		public float bufferTime;
		public AttackInputInfo attackInputInfo;
		public AttackTool[] attackTools;
		public AttackDamageBox attackDamageBox;
		public AttackBox[] nextAttack=new AttackBox[0];
		
		public AttackBox(AnimationClip anim_clip) : base(anim_clip){
		}

		public void ToolsOn(){
			foreach (var item in attackTools){
				item.On();
			}
		}
		
		public void ToolsOff(){
			foreach (var item in attackTools){
				item.Off();
			}
		}
	}
}