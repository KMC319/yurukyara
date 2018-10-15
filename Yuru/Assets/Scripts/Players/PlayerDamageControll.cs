using System;
using Battles.Attack;
using doma;
using UniRx;
using UnityEngine;

namespace Players{
	public class PlayerDamageControll : MonoBehaviour{
		[SerializeField] private float blowTime;
		[SerializeField] private float reductionRate;
		
		private Subject<AttackDamageBox> damageStream=new Subject<AttackDamageBox>();
		public UniRx.IObservable<AttackDamageBox> DamageStream => damageStream;

		public bool InDamage{ get; private set; }

		private Rigidbody rigid;
		private MotionAnimControll motionAnimControll;
		private PlayerGuardControll playerGuardControll;
		
		private void Start(){
			motionAnimControll = this.GetComponentInChildren<MotionAnimControll>();
			playerGuardControll=this.GetComponent<PlayerGuardControll>();

			rigid = this.GetComponent<Rigidbody>();
			motionAnimControll.ResponseStream.Subscribe(RecieveResponce);
		}
		
		private void RecieveResponce(AnimResponce responce){
			if(!InDamage)return;
			if (responce == AnimResponce.Damaged){
				InDamage = false;
			}
		}

		public void Hit(AttackDamageBox attack_damage_box){

			if (playerGuardControll.InGuard){
				if (attack_damage_box.attackType == AttackType.Weak){//guard succeced
					return;
				}else if(attack_damage_box.attackType==AttackType.Strong){
					attack_damage_box.damage *= reductionRate;
				}
			}
			damageStream.OnNext(attack_damage_box);
			motionAnimControll.ForceChangeAnim(motionAnimControll.MyDic.SmallDamage);
			InDamage = true;
		}
	}
}