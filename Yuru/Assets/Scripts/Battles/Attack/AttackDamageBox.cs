using System;
using Players;
using UnityEngine;

namespace Battles.Attack{
	[Serializable]
	public struct AttackDamageBox{
		public float damage;
		public AttackType attackType;
		public Vector3 knockbackPower;
	}
}