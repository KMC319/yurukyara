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
	[RequireComponent(typeof(BoxContainer))]
	public class PlayerAnimControll : MonoBehaviour{
		[SerializeField] public PlayerAnimDictionary MyDic;
		
		private PlayAbleController playAbleController;
		private BoxContainer boxContainer;
		
		
		private readonly Subject<AnimResponce> responseStream=new Subject<AnimResponce>();
		public Subject<AnimResponce> ResponseStream => responseStream;

		private void Start (){
			playAbleController = this.GetComponent<PlayAbleController>();
			boxContainer = this.GetComponent<BoxContainer>();
			
			playAbleController.PlayEndStream.Subscribe(FlowResponce);
		}

		public AnimBox ChangeAnim(string name){
			var res = boxContainer.FindAnim(name);
			playAbleController.TransAnimation(res);
			return res;
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
