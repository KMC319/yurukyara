using System;
using Players;

namespace Battles.Attack{
	[Serializable]
	public struct AttackDamageBox{
		public float damage;
		public AttackType attackType;
	}
}