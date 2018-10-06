using UnityEngine;

namespace Players{
	public class PlayerMoveControll2D : MonoBehaviour,IPlayerMove{
		public float HorizontalMovement{ get; set; }
		public float VerticalMovement{ get; set; }
		public bool InJumping{ get; set; }

		public void Move(){
			throw new System.NotImplementedException();
		}

		public void Jump(){
			throw new System.NotImplementedException();
		}

		public void Stop(){
			throw new System.NotImplementedException();
		}

		public void Cancel(){
			throw new System.NotImplementedException();
		}

		public void FallCheck(bool in_attack){
			throw new System.NotImplementedException();
		}
	}
}