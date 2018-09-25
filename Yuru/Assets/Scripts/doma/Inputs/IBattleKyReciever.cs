namespace doma.Inputs{
	public interface IBattleKyReciever:IInputReciever{
		void ChangeHorizontalAxis(float delta);
		void ChangeVerticalAxis(float delta);
		void JumpKey();
		void RangeAtKey();
		void WeakAtKey();
		void StrongAtKey();
		void GuardKey();
	}
}