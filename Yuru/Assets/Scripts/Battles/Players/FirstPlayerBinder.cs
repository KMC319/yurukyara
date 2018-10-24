using Battles.Systems;
using doma.Inputs;
using UnityEngine;
using Zenject;

namespace Battles.Players{
	public class FirstPlayerBinder : MonoBehaviour,IPlayerBinder{

		private SecondPlayerBinder target;

		public PlayerRoot TargetPlayerRoot => target.PlayerRoot;
		public PlayerNum PlayerNum => PlayerNum.P1;

		public void SetInputEnable(bool en){
			inputRelayPoint.IsActive = en;
		}

		public PlayerRoot PlayerRoot{ get; set; }
		
		
		[Inject] private InputRelayPoint inputRelayPoint;

		private void Awake(){
			PlayerRoot = this.GetComponent<PlayerRoot>();
		}

		private void Start(){
			Launch();
		}


		public void Launch(){
			inputRelayPoint.ChangeReciever(PlayerRoot);
		}

		public void SetUp(SecondPlayerBinder second_player_binder){
			target = second_player_binder;
		}
	}
}