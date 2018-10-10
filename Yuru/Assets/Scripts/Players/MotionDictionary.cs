using System;
using UnityEngine;

namespace Players{
	[Serializable]
	public struct MotionDictionary{
		public string WaitName;
		public string RunName;
		public string JumpName;
		public string FallName;
		public string SmallDamage;
		public string BigDamage;
	}
}