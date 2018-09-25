using System;
using UnityEngine;

namespace Players{
	[Serializable]
	public struct PlayerAnimDictionary{
		[SerializeField] private string waitName;public string WaitName=>waitName;
		[SerializeField] private string runName;public string RunName => runName;
		[SerializeField] private string jumpName;public string JumpName => jumpName;
		[SerializeField] private string rangeName;public string RangeName => rangeName;
		[SerializeField] private string weakName;public string WeakName => weakName;
		[SerializeField] private string strongName;public string StrongName => strongName;
		[SerializeField] private string guardName;public string GuardName => guardName;
		
	}
}