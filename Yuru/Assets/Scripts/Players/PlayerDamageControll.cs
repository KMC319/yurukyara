using System;
using Battles.Attack;
using doma;
using UniRx;
using UnityEngine;

namespace Players{
	public class PlayerDamageControll : MonoBehaviour{
		[SerializeField] private float blowTime;
		
		private Subject<AttackDamageBox> damageStream=new Subject<AttackDamageBox>();
		public UniRx.IObservable<AttackDamageBox> DamageStream => damageStream;

		public bool InDamage{ get; private set; }

		private MotionAnimControll motionAnimControll;
		
		private void Start(){
			motionAnimControll = this.GetComponentInChildren<MotionAnimControll>();

			motionAnimControll.ResponseStream.Subscribe(RecieveResponce);
		}
		
		private void RecieveResponce(AnimResponce responce){
			if(!InDamage)return;
			if (responce == AnimResponce.Damaged){
				InDamage = false;
			}
		}

		public void Hit(AttackDamageBox attack_damage_box){
			damageStream.OnNext(attack_damage_box);
			motionAnimControll.ForceChangeAnim(motionAnimControll.MyDic.SmallDamage);
			InDamage = true;
		}
	}
}