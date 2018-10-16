using UnityEngine;
using Zenject;

namespace Players{
	public class PlayerSecondBinder : MonoBehaviour,IPlayerBinder{
		private PlayerFirstBinder target;

		public PlayerRootControll TargetPlayerRootControll => target.PlayerRootControll;
		public PlayerRootControll PlayerRootControll{ get; private set; }
		

		private void Awake(){
			PlayerRootControll = this.GetComponent<PlayerRootControll>();
		}

		private void Start(){
		
		}

		public void SerUp(PlayerFirstBinder player_first_binder){
			target = player_first_binder;
		}

	}
}