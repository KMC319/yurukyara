using doma;
using UnityEngine;

namespace Battles.Players{
	public class GuardControll : MonoBehaviour{
		public bool InGuard{ get; set; }

		private MotionAnimControll motionAnimControll;

		private int checker;
		private int recorder;
		
		private void Start(){
			motionAnimControll = transform.GetComponentInChildren<MotionAnimControll>();
		}

		private void LateUpdate(){
			if (InGuard&&(++recorder != checker)){
				DebugLogger.Log("a");
				Cancel();
			}
		}

		public void Cancel(){
			checker = 0;
			recorder = 0;
			InGuard = false;
		}

		public void GuardCommand(){
			checker++;
			InGuard = true;
			motionAnimControll.ChangeAnim(motionAnimControll.MyDic.GuardName);
		}
	}
}