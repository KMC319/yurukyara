using doma.Inputs;
using UnityEngine;

namespace Systems{
	public class InstantUiInput : MonoBehaviour{
		[SerializeField] private float delaykey=0.1f;

		private float upInterval;
		private float downInterval;
		private float rightInterval;
		private float leftInterval;
		public IUikeyReciever iUiKyReciever;
		[SerializeField] private bool isActive;public bool IsActive{set{ isActive = value; }
		}

		private void Update(){
			if (!isActive) return;
			var vertical= Input.GetAxisRaw("Vertical2");
			var horizontal=Input.GetAxisRaw("Horizontal2");
			if(Input.GetButtonDown("Submit2"))iUiKyReciever.EnterKey();
			if(Input.GetButtonDown("Cancel2"))iUiKyReciever.CancelKey();


			if (vertical != 0){
				if (vertical > 0){
					if (upInterval == -0.1f) upInterval = delaykey*0.9f;
					upInterval += Time.deltaTime;
					downInterval = 0;
					if (upInterval > delaykey){
						iUiKyReciever.UpKey();
						upInterval = 0;
					} 	
				}
				else if (vertical < 0){
					if (downInterval == -0.1f) downInterval = delaykey*0.9f;
					downInterval += Time.deltaTime;
					upInterval = 0;
					if (downInterval > delaykey){
						iUiKyReciever.DownKey();
						downInterval = 0;
					} 
				}
			}else{
				upInterval = -0.1f;
				downInterval = -0.1f;
			}
			if (horizontal != 0){
				if (horizontal > 0){
					if (rightInterval == -0.1f) rightInterval = delaykey*0.9f;
					rightInterval += Time.deltaTime;
					leftInterval = 0;
					if (rightInterval > delaykey){
						iUiKyReciever.RightKey();
						rightInterval = 0;
					} 	
				}
				else if (horizontal < 0){
					if (leftInterval == -0.1f) leftInterval = delaykey*0.9f;
					leftInterval += Time.deltaTime;
					rightInterval = 0;
					if (leftInterval > delaykey){
						iUiKyReciever.LeftKey();
						leftInterval = 0;
					} 
				}
			}else{
				rightInterval = -0.1f;
				leftInterval = -0.1f;
			}
		}
	}
}