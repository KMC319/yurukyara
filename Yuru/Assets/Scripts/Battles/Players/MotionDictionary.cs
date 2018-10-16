using System;

namespace Battles.Players{
	[Serializable]
	public struct MotionDictionary{
		public string WaitName;
		public string RunName;
		public string JumpName;
		public string FallName;
		public string SmallDamage;
		public string BigDamage;
		public string GuardName;
	}
}