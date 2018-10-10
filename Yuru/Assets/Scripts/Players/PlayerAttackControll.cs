using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Animations;
using Battles.Attack;
using Battles.Health;
using Battles.Systems;
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


		private ApplyPhase CurrentPhase{
			get{
				var now = PhaseManager.Instance.NowPhase;
				if ( now == Phase.P2D){
					return ApplyPhase.P2D;
				}
				if (now == Phase.P3D){
					return ApplyPhase.P3D;
				}
				return ApplyPhase.Both;
			}
		}

		private CommandType CurrentState{
			get{ return iPlayerMove.InJumping ? CommandType.Jump : CommandType.Normal; }
		}

		public bool InAttack{ get; private set; }
		private bool hitEnable;

		private void Start(){
			attackAnimControll = this.GetComponentInChildren<AttackAnimControll>();
			taregtPlayer = this.GetComponent<IPlayerBinder>().TargetPlayerRootControll;

			attackAnimControll.ResponseStream.Subscribe(RecieveResponce);
			
			transform.GetComponentsInChildren<AttackTool>().Select(n => n.HitStream).Merge()
				.Subscribe(RecieveHit);
		}


		public void InputKey(PlayerKeyCode player_key_code){
			Attack(player_key_code);
		}

		private void RecieveHit(Collider collider){
			if (!(InAttack && hitEnable)) return;
			if (collider.gameObject != taregtPlayer.gameObject) return;
			hitEnable = false;

			if (currentAttack.HasNext && currentAttack.NextAttack().attackInputInfo.commandType ==CommandType.Chain){
				ChainAttack();
				return;
			}

			taregtPlayer.playerDamageControll.Hit(currentAttack.attackDamageBox);

		}

		private void RecieveResponce(AnimResponce anim_responce){
			if (anim_responce == AnimResponce.AttackEnd){
				currentAttack.ToolsOff();
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
			var info=new AttackInputInfo(new List<PlayerKeyCode>(){player_key_code},CurrentState,CurrentPhase);
			if (currentAttack == null){
				result= attackAnimControll.FindAttack(info);
			}else{
				if (keyBuffer != null){
					info.keyCodes.Add((PlayerKeyCode)keyBuffer);
					result = attackAnimControll.FindAttack(info);
				}else if (attackAnimControll.FindAttack(info)==currentRoot&&currentAttack.HasNext){
					result = currentAttack.NextAttack();
				}
			}

			
			if(result==null)return;
			currentAttack?.ToolsOff();
			currentAttack = result;
			currentAttack.ToolsOn();
			if (currentRoot == null) currentRoot = result;
			attackAnimControll.Play(currentAttack);
			InAttack = true;
			hitEnable = true;
			
			keyBuffer = player_key_code;
			Observable.Timer(TimeSpan.FromSeconds(currentAttack.bufferTime))
				.Subscribe(_ => { keyBuffer = null;});
		}

		private void ChainAttack(){
			currentAttack?.ToolsOff();
			if (currentAttack != null){
				currentAttack = currentAttack.NextAttack();
				currentAttack.ToolsOn();
				attackAnimControll.Play(currentAttack);
			}

			InAttack = true;
			hitEnable = true;
		}
	}
}