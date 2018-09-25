using doma.Inputs;
using UnityEngine;
using Zenject;

namespace Players{
	public class PlayerOneBinder : MonoBehaviour{
		private PlayerMoveControll playerMoveControll;

		[Inject] private InputRelayPoint inputRelayPoint;
		
		private void Start(){
			playerMoveControll = this.GetComponent<PlayerMoveControll>();
			
			Launch();
		}

		public void Launch(){
			inputRelayPoint.ChangeReciever(playerMoveControll);
			inputRelayPoint.IsActive = true;
		}
	}
}