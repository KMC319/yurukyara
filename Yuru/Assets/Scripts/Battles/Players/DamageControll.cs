using System;
using Battles.Attack;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Players{
	public class DamageControll : MonoBehaviour{
		[SerializeField] private float blowTime;
		[SerializeField] private float reductionRate;
		
		private Subject<AttackDamageBox> damageStream=new Subject<AttackDamageBox>();
		public UniRx.IObservable<AttackDamageBox> DamageStream => damageStream;

		public bool InDamage{ get; private set; }

		private Rigidbody rigid;
		private MotionAnimControll motionAnimControll;
		private GuardControll guardControll;
		
		private void Start(){
			motionAnimControll = this.GetComponentInChildren<MotionAnimControll>();
			guardControll=this.GetComponent<GuardControll>();

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
			if (guardControll.InGuard){
				if (attack_damage_box.attackType == AttackType.Weak){//guard succeced
					return;
				}else if(attack_damage_box.attackType==AttackType.Strong){
					attack_damage_box.damage *= reductionRate;
				}
			}
			var po = attack_damage_box.knockbackPower;
			rigid.AddForce(transform.forward+new Vector3(po.x,po.y,po.z*Math.Sign(transform.forward.z)*-1),ForceMode.Impulse);

			damageStream.OnNext(attack_damage_box);
			motionAnimControll.ForceChangeAnim(motionAnimControll.MyDic.SmallDamage);
			InDamage = true;
		}
	}
}