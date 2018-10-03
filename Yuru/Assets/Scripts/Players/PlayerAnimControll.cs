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
		[SerializeField] private List<AnimBox> myAims;

		private PlayAbleController playAbleController;
		
		private readonly Subject<AnimResponce> responseStream=new Subject<AnimResponce>();
		public Subject<AnimResponce> ResponseStream => responseStream;

		private void Start (){
			playAbleController = this.GetComponent<PlayAbleController>();
			
			playAbleController.PlayEndStream.Subscribe(FlowResponce);

		}
		public void ChangeAnim(string anim_name){
			playAbleController.TransAnimation(FindMyAnim(anim_name));
		}

		private void FlowResponce(string name){
			var check = new Func<string,bool>((n) => name == n);
			if(check(myDic.WeakName))responseStream.OnNext(AnimResponce.AttackEnd);
		}

		private AnimBox FindMyAnim(string anim_name){
			var res = myAims.Find(n => n.clip.name == anim_name);
			if (res == null){
				DebugLogger.LogError(anim_name+"is not found");
				return null;
			}
			return res;
		}
	}
}
