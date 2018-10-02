using doma.Inputs;
using UniRx;
using UnityEngine;

namespace Systems{
	public class InstantBattleInput : MonoBehaviour{
		public IBattleKeyReciever iBattleKeyReciever;
		[SerializeField] private bool isActive;public bool IsActive{set{ isActive = value; }}


		private void Update(){
			if (!isActive) return;
			iBattleKeyReciever.ChangeHorizontalAxis(Input.GetAxisRaw("Horizontal2"));
			iBattleKeyReciever.ChangeHorizontalAxis(Input.GetAxisRaw("Vertical2"));
		}
		
	}
}