using System;
using System.Linq;
using Battles.Animations;
using UnityEngine;

namespace Battles.Attack{
	[Serializable]
	public class AttackBox:AnimBox{
		public float bufferTime;
		public AttackInputInfo attackInputInfo;
		public float delayTimeForTools;
		public AttackTool[] attackTools;
		public AttackDamageBox attackDamageBox;
		[SerializeField]private AttackBox[] nextAttack=new AttackBox[0];

		public AttackBox NextAttack(int num=0){
			return HasNext ? nextAttack[num] : null;
		}

		public AttackBox(AnimationClip anim_clip) : base(anim_clip){
		}

		public bool HasNext => nextAttack != null &&nextAttack.Length == 1 &&nextAttack.First().clip != null;

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