using Battles.Attack;
using doma;
using UnityEngine;

namespace Battles.Players{
	public class GuardControll : MonoBehaviour{
		public bool InGuard{ get; set; }
		[SerializeField] private IAttackTool[] attackTools;

		private MotionAnimControll motionAnimControll;

		private int checker;
		private int recorder;
		
		private void Start(){
			motionAnimControll = transform.GetComponentInChildren<MotionAnimControll>();
		}

		private void LateUpdate(){
			if (InGuard&&(++recorder != checker)){
				Cancel();
			}
		}

		public void Guard(){
			checker++;
			InGuard = true;
			foreach (var item in attackTools){
				item.On();
			}
		}
		
		public void Cancel(){
			checker = 0;
			recorder = 0;
			InGuard = false;
			foreach (var item in attackTools) {
				item.Off();
			}
		}

		public void GuardCommand(){
			Guard();
			motionAnimControll.ChangeAnim(motionAnimControll.MyDic.GuardName);
		}
	}
}