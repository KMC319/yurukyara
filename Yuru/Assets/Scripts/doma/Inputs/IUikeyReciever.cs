namespace doma.Inputs{
	public interface IUikeyReciever:IInputReciever{
		void UpKey();
		void DownKey();
		void RightKey();
		void LeftKey();
		void EnterKey();
		void CancelKey();
	}
}