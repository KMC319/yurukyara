using UniRx;
using UnityEngine;

namespace doma.Inputs{
	public class UiInputFromUnity : MonoBehaviour,IUikeySender{
		[SerializeField] private float delaykey=0.1f;

		private float upInterval;
		private float downInterval;
		private float rightInterval;
		private float leftInterval;
		
		private readonly Subject<Unit> upSubject=new Subject<Unit>();
		public IObservable<Unit>UpKey => upSubject;
		private readonly Subject<Unit> downSubject=new Subject<Unit>();
		public IObservable<Unit>DownKey => downSubject;
		private readonly Subject<Unit> rightKeySubject=new Subject<Unit>();
		public IObservable<Unit> RightKey => rightKeySubject;
		private readonly Subject<Unit> leftKeySubject=new Subject<Unit>();
		public IObservable<Unit> LeftKey => leftKeySubject;
		private Subject<Unit> enterKey=new Subject<Unit>();
		public IObservable<Unit> EnterKey => enterKey;
		private Subject<Unit> cancelKey=new Subject<Unit>();
		public IObservable<Unit> CancelKey => cancelKey;

		private void Update (){
			var vertical= Input.GetAxisRaw("Vertical");
			var horizontal=Input.GetAxisRaw("Horizontal");
			if(Input.GetButtonDown("A1"))enterKey.OnNext(Unit.Default);
			if(Input.GetButtonDown("B1"))cancelKey.OnNext(Unit.Default);

			if (vertical != 0){
				if (vertical > 0){
					if (upInterval == -0.1f) upInterval = delaykey*0.9f;
					upInterval += Time.deltaTime;
					downInterval = 0;
					if (upInterval > delaykey){
						upSubject.OnNext(Unit.Default);
						upInterval = 0;
					} 	
				}
				else if (vertical < 0){
					if (downInterval == -0.1f) downInterval = delaykey*0.9f;
					downInterval += Time.deltaTime;
					upInterval = 0;
					if (downInterval > delaykey){
						downSubject.OnNext(Unit.Default);
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
						rightKeySubject.OnNext(Unit.Default);
						rightInterval = 0;
					} 	
				}
				else if (horizontal < 0){
					if (leftInterval == -0.1f) leftInterval = delaykey*0.9f;
					leftInterval += Time.deltaTime;
					rightInterval = 0;
					if (leftInterval > delaykey){
						leftKeySubject.OnNext(Unit.Default);
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
