using Battles.Systems;

namespace Battles.Players{
	public interface IPlayerBinder{
		PlayerRoot PlayerRoot{ get; }
		PlayerRoot TargetPlayerRoot{ get; }
		PlayerNum PlayerNum{ get; }
		void SetInputEnable(bool en);
	}
}