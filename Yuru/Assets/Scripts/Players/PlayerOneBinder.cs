using doma.Inputs;
using UnityEngine;
using Zenject;

namespace Players{
	public class PlayerOneBinder : MonoBehaviour{
		private PlayerRootControll playerRootControll;

		[Inject] private InputRelayPoint inputRelayPoint;
		
		private void Start(){
			playerRootControll = this.GetComponent<PlayerRootControll>();
			Launch();
		}

		public void Launch(){
			inputRelayPoint.ChangeReciever(playerRootControll);
			inputRelayPoint.IsActive = true;
		}
	}
}