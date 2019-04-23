﻿using System;
using Battles.Attack;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Players{
	public class DamageControll : MonoBehaviour{
		[SerializeField] private float blowPowerBorder;
		[SerializeField] private float reductionRate;
		
		private Subject<AttackDamageBox> damageStream=new Subject<AttackDamageBox>();
		public UniRx.IObservable<AttackDamageBox> DamageStream => damageStream;

		public bool InDamage{ get; private set; }

		private Rigidbody rigid;
		private MotionAnimControll motionAnimControll;
		private IPlayerCancelProcess[] playerCancelProcesses;
		private GuardControll guardControll;
		
		private void Start(){
			motionAnimControll = this.GetComponentInChildren<MotionAnimControll>();
			guardControll=this.GetComponent<GuardControll>();
			playerCancelProcesses = transform.GetComponentsInChildren<IPlayerCancelProcess>();
			
			rigid = this.GetComponent<Rigidbody>();
			motionAnimControll.ResponseStream.Subscribe(RecieveResponce);
		}
		
		private void RecieveResponce(AnimResponce responce){
			if (responce == AnimResponce.Damaged){
				InDamage = false;
				rigid.velocity=new Vector3(rigid.velocity.x,rigid.velocity.y,0);
			}
		}

		public void Hit(AttackDamageBox attack_damage_box){
			if(InDamage)return;
			foreach (var item in playerCancelProcesses){
				item.Cancel();
			}
			if (guardControll.InGuard){
				if (attack_damage_box.attackType == AttackType.Weak){
					return;//ガード成功
				}else if(attack_damage_box.attackType==AttackType.Strong || attack_damage_box.attackType==AttackType.Finish){
					attack_damage_box.damage *= reductionRate;//削り
					attack_damage_box.attackType = AttackType.Weak;//ガードでフェイズが変わらないように
					damageStream.OnNext(attack_damage_box);
					return;
				}
			}
			//自分から見てどれくらい吹っ飛ぶか
			var po = -transform.forward * attack_damage_box.knockbackPower.z + transform.right * attack_damage_box.knockbackPower.x + transform.up * attack_damage_box.knockbackPower.y;
			rigid.AddForce(new Vector3(po.x, po.y, po.z), ForceMode.Impulse);

			damageStream.OnNext(attack_damage_box);//ダメージを受けた通知をするだけで計算はHealthManager任せ
			
			if (attack_damage_box.knockbackPower.magnitude > blowPowerBorder){
				motionAnimControll.ForceChangeAnim(motionAnimControll.MyDic.BigDamage);
				InDamage = true;
			} else{
				motionAnimControll.ForceChangeAnim(motionAnimControll.MyDic.SmallDamage);
			}
		}

		public void Grabed(){
			InDamage = true;
			motionAnimControll.ChangeAnim(motionAnimControll.MyDic.GuardName);
		}

		public void GrabRelease() {
			InDamage = false;
		}
	}
}