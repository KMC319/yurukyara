namespace Players{
	public interface IPlayerMove{
		float HorizontalMovement{ get; set; }
		float VerticalMovement{ get; set; }
		bool InJumping{ get; set;}
		void Move();
		void Jump();
		void Stop();
		void Cancel();
		void FallCheck(bool in_attack);
	}
}