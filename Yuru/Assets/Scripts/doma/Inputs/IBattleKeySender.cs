using UniRx;

namespace doma.Inputs{
	public interface IBattleKeySender{
		IObservable<float> HorizontalAxsis{ get; }
		IObservable<float> VerticalAxsis{ get; }
		IObservable<Unit> JumpKey{ get; }
		IObservable<Unit> RangeAtKey{ get; }
		IObservable<Unit> WeakAtKey{ get; }
		IObservable<Unit> StrongAtKey{ get; }
		IObservable<Unit> GuardKey{ get; }
	}
}