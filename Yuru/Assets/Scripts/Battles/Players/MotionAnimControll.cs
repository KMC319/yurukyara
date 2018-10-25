using Battles.Animations;
using UniRx;
using UnityEngine;

namespace Battles.Players{
	public enum AnimResponce{
		Wait,AttackEnd,Damaged,JumpLaunch
	}
	[RequireComponent(typeof(BoxContainer))]
	[RequireComponent(typeof(PlayAbleController))]
	public class MotionAnimControll : MonoBehaviour{
		[SerializeField] public MotionDictionary MyDic;
		
		private PlayAbleController playAbleController;
		private BoxContainer boxContainer;
		
		
		private readonly Subject<AnimResponce> responseStream=new Subject<AnimResponce>();
		public IObservable<AnimResponce> ResponseStream => responseStream;


		private void Start (){
			playAbleController = this.GetComponent<PlayAbleController>();
			boxContainer = this.GetComponent<BoxContainer>();
			
			playAbleController.PlayEndStream.Subscribe(FlowResponce);
		}

		private void Play(AnimBox anim_box){
			if(playAbleController.CurrentAnimBox==anim_box)return;
			playAbleController.TransAnimation(anim_box);
		}

		public AnimBox ChangeAnim(string name){
			var res = boxContainer.FindAnim(name);
			Play(res);
			return res;
		}
		
		public void ChangeAnim(AnimBox anim_box){
			Play(anim_box);
		}

		public void ForceChangeAnim(AnimBox anim_box){
			playAbleController.TransAnimation(anim_box);
		}
		public AnimBox ForceChangeAnim(string name){
			var res = boxContainer.FindAnim(name);
			playAbleController.TransAnimation(res);
			return res;
		}

		private void FlowResponce(AnimBox anim_box){
			AnimResponce? a = null;
			var name = anim_box.clip.name;
			if (name == MyDic.SmallDamage||name == MyDic.BigDamage){
				a = AnimResponce.Damaged;
			}

			if (name == MyDic.JumpStartName){
				a = AnimResponce.JumpLaunch;
			}
			

			if (a != null){
				responseStream.OnNext((AnimResponce) a);
			}
		}

	}
}
