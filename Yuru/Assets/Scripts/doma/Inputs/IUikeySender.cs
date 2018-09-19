using UniRx;

namespace doma.Inputs{
	public interface IUikeySender{
		IObservable<Unit>UpKey{ get; }
		IObservable<Unit>DownKey{ get; }
		IObservable<Unit>RightKey{ get; }
		IObservable<Unit>LeftKey{ get; }
		IObservable<Unit>EnterKey{ get; }
		IObservable<Unit>CancelKey{ get; }
	}
}