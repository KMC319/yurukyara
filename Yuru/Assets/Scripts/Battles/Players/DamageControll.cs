using System;
using Battles.Attack;
using Battles.Effects;
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
		public bool IsDown { get; private set; }

		private Rigidbody rigid;
		private MotionAnimControll motionAnimControll;
		private IPlayerCancelProcess[] playerCancelProcesses;
		private GuardControll guardControll;
		private SoundEffectControl soundEffectControl;
		
		private void Start(){
			motionAnimControll = this.GetComponentInChildren<MotionAnimControll>();
			guardControll=this.GetComponent<GuardControll>();
			playerCancelProcesses = transform.GetComponentsInChildren<IPlayerCancelProcess>();
			
			rigid = this.GetComponent<Rigidbody>();
			motionAnimControll.ResponseStream.Subscribe(RecieveResponce);
			soundEffectControl = GetComponent<SoundEffectControl>();
		}
		
		private void RecieveResponce(AnimResponce responce){
			if (responce == AnimResponce.Damaged){
				InDamage = false;
				IsDown = false;
				rigid.velocity=new Vector3(rigid.velocity.x,rigid.velocity.y,0);
			}
		}

		public void Hit(AttackDamageBox attack_damage_box){
			if(IsDown)return;
			foreach (var item in playerCancelProcesses){
				item.Cancel();
			}
			if (guardControll.InGuard){
				if (attack_damage_box.attackType == AttackType.Weak || attack_damage_box.attackType == AttackType.Shot){
					soundEffectControl.Play(ESoundType.WeakGuard);
					return;//ガード成功
				}else if(attack_damage_box.attackType==AttackType.Strong || attack_damage_box.attackType==AttackType.Finish){
					attack_damage_box.damage *= reductionRate;//削り
					soundEffectControl.Play(ESoundType.StrongGuard);
					attack_damage_box.attackType = AttackType.Weak;//ガードでフェイズが変わらないように
					damageStream.OnNext(attack_damage_box);
					return;
				}
			}
			//自分から見てどれくらい吹っ飛ぶか
			var po = -transform.forward * attack_damage_box.knockbackPower.z + transform.right * attack_damage_box.knockbackPower.x + transform.up * attack_damage_box.knockbackPower.y;
			rigid.AddForce(new Vector3(po.x, po.y, po.z), ForceMode.VelocityChange);

			damageStream.OnNext(attack_damage_box);//ダメージを受けた通知をするだけで計算はHealthManager任せ
			
			if (attack_damage_box.knockbackPower.magnitude > blowPowerBorder){
				motionAnimControll.ForceChangeAnim(motionAnimControll.MyDic.BigDamage);
				soundEffectControl.Play(ESoundType.StrongHit);
				InDamage = true;
				IsDown = true;
			} else{
				motionAnimControll.ForceChangeAnim(motionAnimControll.MyDic.SmallDamage);
				soundEffectControl.Play(ESoundType.WeakHit);
				InDamage = true;
			}
		}

		public void Grabed(){
			IsDown = true;
			motionAnimControll.Pause();
		}

		public void GrabRelease() {
			IsDown = false;
			motionAnimControll.Resume();
		}
	}
}