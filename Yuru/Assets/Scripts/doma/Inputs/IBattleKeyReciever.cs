namespace doma.Inputs{
	public interface IBattleKeyReciever:IInputReciever{
		void ChangeHorizontalAxis(float delta);
		void ChangeVerticalAxis(float delta);
		void JumpKey();
		void RangeAtKey();
		void WeakAtKey();
		void StrongAtKey();
		void GuardKey();
	}
}