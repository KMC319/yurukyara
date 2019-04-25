using System;
using Battles.Attack;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Systems{
	[Serializable]
	public class HealthManager{
		public float health;
		
		private float stackableHealth;
		private float maxHealth;
		private float stackHealth;
		
		private readonly GaugeControll gaugeControll;

		private Subject<Unit> deadStream=new Subject<Unit>();
		public UniRx.IObservable<Unit> DeadStream=>deadStream;
		
		public HealthManager(float health,float stackable_health,GaugeControll gauge_controll){
			this.health = health;
			maxHealth = this.health;
			stackableHealth = stackable_health;
			gaugeControll = gauge_controll;
			
			gaugeControll.SetAmount(1);
		}

		public void DamageRecieve(AttackDamageBox attack_damage_box){
			health -= attack_damage_box.damage;
			Debug.Log(attack_damage_box.damage);
			gaugeControll.TempUpdate(health/maxHealth);
			gaugeControll.EntityUpdate();

			if (health <= 0){
				deadStream.OnNext(Unit.Default);
				return;
			}
			PhaseControll(attack_damage_box);
		}
		
		private void PhaseControll(AttackDamageBox attack_damage_box){
			var attack_type = attack_damage_box.attackType;
			switch (PhaseManager.Instance.NowPhase){
				case Phase.P3D:
					stackHealth = 0;
					if (attack_type == AttackType.Strong || attack_type == AttackType.Grab){
						DebugLogger.Log("to 2D phase by"+attack_damage_box.attackType);
						PhaseManager.Instance.NowPhase = Phase.P2D;
					}
					break;
				case Phase.P2D:
					stackHealth += attack_damage_box.damage;
					if (stackHealth>=stackableHealth&&attack_type != AttackType.Weak){
						stackHealth = 0;
						PhaseManager.Instance.NowPhase = Phase.P3D;
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}