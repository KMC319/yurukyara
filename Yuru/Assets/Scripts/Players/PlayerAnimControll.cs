using System;
using System.Collections.Generic;
using Animations;
using doma;
using UniRx;
using UnityEngine;

namespace Players{
	public enum AnimResponce{
		Wait,AttackEnd
	}
	public class PlayerAnimControll : MonoBehaviour{
		[SerializeField] public PlayerAnimDictionary myDic;
		
		private PlayAbleController playAbleController;
		
		
		private readonly Subject<AnimResponce> responseStream=new Subject<AnimResponce>();
		public Subject<AnimResponce> ResponseStream => responseStream;

		private void Start (){
			playAbleController = this.GetComponent<PlayAbleController>();
			
			playAbleController.PlayEndStream.Subscribe(FlowResponce);
		}
		
		public void ChangeAnim(AnimBox anim_box){
			playAbleController.TransAnimation(anim_box);
		}

		private void FlowResponce(AnimBox anim_box){
			AnimResponce? a = null;
			
			if (anim_box is AttackBox){
				a = AnimResponce.AttackEnd;
			}

			if (a != null){
				responseStream.OnNext((AnimResponce) a);
			}
		}

	}
}
