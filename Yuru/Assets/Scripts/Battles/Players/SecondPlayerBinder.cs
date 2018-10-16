using UnityEngine;

namespace Battles.Players{
	public class SecondPlayerBinder : MonoBehaviour,IPlayerBinder{
		private FirstPlayerBinder target;

		public PlayerRoot TargetPlayerRoot => target.PlayerRoot;
		public PlayerRoot PlayerRoot{ get; private set; }
		

		private void Awake(){
			PlayerRoot = this.GetComponent<PlayerRoot>();
		}

		private void Start(){
		
		}

		public void SerUp(FirstPlayerBinder first_player_binder){
			target = first_player_binder;
		}

	}
}