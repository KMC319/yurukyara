using UnityEngine;
using Zenject;

namespace Players{
	public class PlayerSecondBinder : MonoBehaviour,IPlayerBinder{
		[Inject]private PlayerFirstBinder target;

		public PlayerRootControll TargetPlayerRootControll => target.PlayerRootControll;
		public PlayerRootControll PlayerRootControll{ get; set; }
		

		private void Awake(){
			PlayerRootControll = this.GetComponent<PlayerRootControll>();
		}

		private void Start(){
		
		}

	}
}