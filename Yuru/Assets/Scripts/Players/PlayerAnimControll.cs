using System.Collections.Generic;
using Animations;
using doma;
using UnityEngine;

namespace Players{
	public class PlayerAnimControll : MonoBehaviour{
		[SerializeField] private List<AnimBox> myAims;

		private PlayAbleController playAbleController;

		private void Start (){
			playAbleController = this.GetComponent<PlayAbleController>();
			
		}
		public void ChangeAnim(string anim_name){
			playAbleController.TransAnimation(FindMyAnim(anim_name));
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
