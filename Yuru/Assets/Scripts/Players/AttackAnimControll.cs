using System.Collections.Generic;
using System.Linq;
using Animations;
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

		public AttackBox FindAttack(PlayerKeyCode key1,CommandType command_type){
			return AttackBoxs
				.Where(n=>n.keyCodes.Count==1)
				.Where(n=>n.commandType==command_type)
				.ToList()
				.Find(n=>n.keyCodes.Contains(key1));
		}
		
		public AttackBox FindAttack(PlayerKeyCode key1,PlayerKeyCode key2,CommandType command_type){
			return AttackBoxs
				.Where(n=>n.keyCodes.Count==2)
				.Where(n=>n.commandType==command_type)
				.ToList()
				.Find(n=>n.keyCodes.Contains(key1)&&n.keyCodes.Contains(key2));
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