using doma;
using doma.Inputs;
using UniRx;
using UnityEngine;

namespace Systems{
	public class InstantBattleInput : MonoBehaviour{
		public IBattleKeyReciever iBattleKeyReciever;
		[SerializeField] private bool isActive;public bool IsActive{set{ isActive = value; }}


		private void Start(){
		}

		private void Update(){
			if (!isActive) return;
			iBattleKeyReciever.ChangeHorizontalAxis(Input.GetAxisRaw("Horizontal2"));
			iBattleKeyReciever.ChangeVerticalAxis(Input.GetAxisRaw("Vertical2"));
			
			if(Input.GetButtonDown("A2"))iBattleKeyReciever.JumpKey();
			if(Input.GetButtonDown("B2"))iBattleKeyReciever.RangeAtKey();
			if(Input.GetButtonDown("X2"))iBattleKeyReciever.WeakAtKey();
			if(Input.GetButtonDown("Y2"))iBattleKeyReciever.StrongAtKey();
			if(Input.GetButton("RB2"))iBattleKeyReciever.GuardKey();
		}
		
	}
}