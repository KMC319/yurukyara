using System.Collections.Generic;
using System.Linq;
using Animations;
using Battles.Attack;
using Battles.Health;
using UniRx;
using UnityEngine;

namespace Players{
	public class AttackAnimControll : MonoBehaviour{
		private PlayAbleController playAbleController;
		private BoxContainer boxContainer;
		private List<AttackBox> AttackBoxs => boxContainer.AttackBoxs;
		private readonly Subject<AnimResponce> responseStream = new Subject<AnimResponce>();
		public Subject<AnimResponce> ResponseStream => responseStream;

		private AnimBox current;

		private void Start(){
			playAbleController = this.GetComponent<PlayAbleController>();
			boxContainer = this.GetComponent<BoxContainer>();

			playAbleController.PlayEndStream.Subscribe(FlowResponce);
		}

		public void Play(AnimBox anim_box){
			if (current == anim_box) return;
			playAbleController.TransAnimation(anim_box);
			current = anim_box;
		}

		public void CashClear(){
			current = null;
		}

		public void ChangeAnim(AnimBox anim_box){
			Play(anim_box);
		}
		
		public AttackBox FindAttack(AttackInputInfo info){
			return  AttackBoxs
					.Where(n => n.attackInputInfo.keyCodes.Count == info.keyCodes.Count)
					.Where(n=>n.attackInputInfo.applyPhase==ApplyPhase.Both||n.attackInputInfo.applyPhase == info.applyPhase)
					.Where(n => n.attackInputInfo.commandType == info.commandType)
					.ToList()
					.Find(n =>n.attackInputInfo.keyCodes.OrderBy(v=>v)
						.SequenceEqual(info.keyCodes.OrderBy(w=>w)));
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
			if (anim_box is AttackBox){
				responseStream.OnNext(AnimResponce.AttackEnd);
			}
		}
	}
}