using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Animations;
using Battles.Attack;
using Battles.Health;
using doma;
using UniRx;
using UnityEngine;

namespace Players{
	public class PlayerAttackControll : MonoBehaviour{
		private AttackBox currentAttack;
		private AttackBox currentRoot;
		private PlayerKeyCode? keyBuffer;
		
		private AttackAnimControll attackAnimControll;
		public IPlayerMove iPlayerMove;

		private PlayerRootControll taregtPlayer;


		private CommandType CurrentState{
			get{ return iPlayerMove.InJumping ? CommandType.Jump : CommandType.Normal; }
		}

		public bool InAttack{ get; private set; }
		private bool hitEnable;

		private void Start(){
			attackAnimControll = this.GetComponentInChildren<AttackAnimControll>();
			taregtPlayer = this.GetComponent<IPlayerBinder>().TargetPlayerRootControll;

			attackAnimControll.ResponseStream.Subscribe(RecieveResponce);
			Observable.Merge(transform.GetComponentsInChildren<AttackCollider>().Select(n => n.HitStream))
				.Subscribe(RecieveHit);
		}


		public void InputKey(PlayerKeyCode player_key_code){
			Attack(player_key_code);
		}

		private void RecieveHit(Collider collider){
			if (!(InAttack && hitEnable)) return;
			if (collider.gameObject != taregtPlayer.gameObject) return;
			taregtPlayer.playerDamageControll.Hit(currentAttack.attackDamageBox);
			hitEnable = false;
		}

		private void RecieveResponce(AnimResponce anim_responce){
			if (anim_responce == AnimResponce.AttackEnd){
				currentAttack = null;
				currentRoot = null;
				keyBuffer = null;
				InAttack = false;
				hitEnable = false;
				attackAnimControll.CashClear();
			}
		}

		private void Attack(PlayerKeyCode player_key_code){
			AttackBox result = null;
			if (currentAttack == null){
				result= attackAnimControll.FindAttack(player_key_code, CurrentState);
			}else{
				if (keyBuffer != null){
					result = attackAnimControll.FindAttack(player_key_code,(PlayerKeyCode)keyBuffer, CurrentState);
					
				}else if (attackAnimControll.FindAttack(player_key_code,CurrentState)==currentRoot&&
				          currentAttack.nextAttack != null &&
				          currentAttack.nextAttack.Length == 1 && 
				          currentAttack.nextAttack.First().clip != null){
					result = currentAttack.nextAttack.First();
				}
			}

			
			if(result==null)return;
			currentAttack?.ColliderOff();
			currentAttack = result;
			currentAttack.ColliderOn();
			if (currentRoot == null) currentRoot = result;
			attackAnimControll.Play(currentAttack);
			InAttack = true;
			hitEnable = true;
			
			keyBuffer = player_key_code;
			Observable.Timer(TimeSpan.FromSeconds(currentAttack.bufferTime))
				.Subscribe(_ => { keyBuffer = null;});
		}
	}
}