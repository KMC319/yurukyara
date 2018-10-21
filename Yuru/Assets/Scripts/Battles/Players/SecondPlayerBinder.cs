using Systems;
using Battles.Systems;
using doma.Inputs;
using UnityEngine;

namespace Battles.Players{
	public class SecondPlayerBinder : MonoBehaviour,IPlayerBinder{
		private FirstPlayerBinder target;

		public PlayerRoot TargetPlayerRoot => target.PlayerRoot;
		public PlayerNum PlayerNum => PlayerNum.P2;

		public void SetInputEnable(bool en){
			instantBattleInput.IsActive = en;
		}

		private InstantBattleInput instantBattleInput;
		public PlayerRoot PlayerRoot{ get; private set; }
		

		private void Awake(){
			PlayerRoot = this.GetComponent<PlayerRoot>();
			instantBattleInput = gameObject.AddComponent<InstantBattleInput>();
			instantBattleInput.iBattleKeyReciever=this.GetComponent<IBattleKeyReciever>();
		}

		private void Start(){
		
		}

		public void SerUp(FirstPlayerBinder first_player_binder){
			target = first_player_binder;
		}

	}
}