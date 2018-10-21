namespace Battles.Players{
	public interface IPlayerBinder{
		PlayerRoot TargetPlayerRoot{ get; }
		void SetInputEnable(bool en);
	}
}