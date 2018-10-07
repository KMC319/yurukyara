using System;
using Battles.Health;

namespace Battles.Attack{
	[Serializable]
	public struct AttackDamageBox{
		public float damage;
		public AttackType attackType;
	}
}