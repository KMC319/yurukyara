using System;
using UnityEngine;

namespace Battles.Attack{
	[Serializable]
	public struct AttackDamageBox{
		public float damage;
		public AttackType attackType;
		public Vector3 knockbackPower;

	}
	public enum AttackType{
		Weak,Strong,Finish,Range,Grab,Empty,Shot
	}
}