using System;
using UnityEngine;

namespace Players{
	[Serializable]
	public struct PlayerAnimDictionary{
		public string WaitName;
		public string RunName;
		public string JumpName;
		public string FallName;
		public string RangeName;
		public string WeakName;
		public string StrongName;
		public string JumpAtName;
		public string GrabName;
		public string GuardName;
	}
}