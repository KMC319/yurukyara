using System;
using System.Collections.Generic;
using System.Linq;
using Battles.Animations;
using doma;
using UnityEngine;

namespace Battles.Attack{
	[Serializable]
	public class AttackBox:AnimBox{
		public float bufferTime;
		public AttackInputInfo attackInputInfo;
		public float delayTimeForTools;
		public IAttackTool[] attackTools;
		public AttackDamageBox attackDamageBox;
		[SerializeField]private AttackBox[] nextAttack=new AttackBox[0];

		public AttackBox NextAttack(int num=0){
			return HasNext ? nextAttack[num] : null;
		}

		public AttackBox[] GetTree() {
			var res = new List<AttackBox>() {this};
			var target= this.NextAttack();
			while (target!=null) {
				res.Add(target);
				target = target.NextAttack();
			}
			return res.ToArray();
		}

		public AttackBox(AnimationClip anim_clip) : base(anim_clip){
		}

		public bool HasNext => nextAttack != null &&nextAttack.Length == 1 &&nextAttack.First().clip != null;

		public void ToolsOn(){
			try{
				foreach (var item in attackTools){
					item.On();
				}
			}catch (Exception e){
				DebugLogger.LogError(e+",in "+clip.name);
			}
		}
		
		public void ToolsOff(){
			try{
				foreach (var item in attackTools){
					item.Off();
				}
			}catch (Exception e){
				DebugLogger.LogError(e+",in "+clip.name);
			}
		}

		public void ToolsCancel(){
			try{
				foreach (var item in attackTools){
					item.Off(true);
				}
			}catch (Exception e){
				DebugLogger.LogError(e+",in "+clip.name);
			}
		}
	}
}