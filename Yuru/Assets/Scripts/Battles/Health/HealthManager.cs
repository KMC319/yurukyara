using System;
using Animations;
using Battles.Attack;
using doma;
using Players;
using UniRx;
using UnityEngine;

namespace Battles.Health{
	[Serializable]
	public class HealthManager{
		[SerializeField] private float health;

		private float maxHealth;
		
		private GaugeControll gaugeControll;

		private Subject<Unit> phaseChangeStream=new Subject<Unit>();
		public Subject<Unit> PhaseChangeNotification => phaseChangeStream;

		public HealthManager(float health,GaugeControll gauge_controll){
			this.health = health;
			maxHealth = this.health;
			gaugeControll = gauge_controll;
			
			gaugeControll.SetAmount(1);
		}

		public void DamageRecieve(AttackDamageBox attack_damage_box){
			health -= attack_damage_box.damage;
			gaugeControll.TempUpdate(health/maxHealth);
			gaugeControll.EntityUpdate();
			
			var tag = attack_damage_box.attackType;
			if (tag == AttackType.Strong || tag == AttackType.Grab){
				phaseChangeStream.OnNext(Unit.Default);
			}
		}
	}
}