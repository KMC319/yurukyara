using UniRx;
using UnityEngine;

namespace doma.Inputs{
	public class BattleInputFromUnity : MonoBehaviour,IBattleKeySender{
		private readonly Subject<float> horizontalAxsis=new Subject<float>();
		private readonly Subject<float> verticalAxsis=new Subject<float>();
		
		public IObservable<float> HorizontalAxsis => horizontalAxsis;
		public IObservable<float> VerticalAxsis => verticalAxsis;
		private readonly Subject<Unit> jumpKey=new Subject<Unit>();
		public IObservable<Unit> JumpKey => jumpKey;
		private readonly Subject<Unit> rangeAtKey=new Subject<Unit>();
		public IObservable<Unit> RangeAtKey => rangeAtKey;
		private readonly Subject<Unit> weakAtKey=new Subject<Unit>();
		public IObservable<Unit> WeakAtKey => weakAtKey;
		private readonly Subject<Unit> strongAtKey=new Subject<Unit>();
		public IObservable<Unit> StrongAtKey => strongAtKey;
		private readonly Subject<Unit> guardKey=new Subject<Unit>();
		public IObservable<Unit> GuardKey => guardKey;

		private void Update(){
			horizontalAxsis.OnNext(Input.GetAxisRaw("Horizontal"));
			verticalAxsis.OnNext(Input.GetAxisRaw("Vertical"));
		}

	}
}