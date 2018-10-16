﻿using System;
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
		[SerializeField] private float keyBufferTime;
		private AttackBox currentAttack;
		private AttackBox currentRoot;
		private PlayerKeyCode? keyBuffer;
		
		private AttackAnimControll attackAnimControll;
		[NonSerialized]public PlayerMove iPlayerMove;

		private PlayerRootControll taregtPlayer;

		private PlayerRootControll TaregtPlayer{
			get{
				if (taregtPlayer == null){
					taregtPlayer = this.GetComponent<IPlayerBinder>().TargetPlayerRootControll;
				}
				return taregtPlayer;
			}
		}



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
			
			attackAnimControll.ResponseStream.Subscribe(RecieveResponce);
			
			transform.GetComponentsInChildren<AttackTool>().Select(n => n.HitStream).Merge()
				.Subscribe(RecieveHit);
			this.ObserveEveryValueChanged(n => n.InAttack).Where(n => n)
				.Subscribe(n => {
					if (currentAttack.attackInputInfo.commandType != CommandType.Jump){
						iPlayerMove.Cancel();
						iPlayerMove.ForceFall();
					}
				});
		}


		public void InputKey(PlayerKeyCode player_key_code){
			if (keyBuffer == player_key_code) return;
			
			Attack(player_key_code);
						
			keyBuffer = player_key_code;
			Observable.Timer(TimeSpan.FromSeconds(currentAttack?.bufferTime ?? keyBufferTime))
				.Subscribe(_ => { keyBuffer = null;});
		}

		private void RecieveHit(Collider collider){
			if (!(InAttack && hitEnable)) return;
			if (collider.gameObject != TaregtPlayer.gameObject) return;
			hitEnable = false;

			if (currentAttack.HasNext && currentAttack.NextAttack().attackInputInfo.commandType ==CommandType.Chain){
				ChainAttack();
				return;
			}

			TaregtPlayer.playerDamageControll.Hit(currentAttack.attackDamageBox);

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
			
			if (keyBuffer != null){
				var duo_info=info;
				duo_info.keyCodes.Add((PlayerKeyCode) keyBuffer);
				result = attackAnimControll.FindAttack(duo_info);
			}

			if (result == null){
				if (currentAttack == null){
					result = attackAnimControll.FindAttack(info);
				}
				else if (attackAnimControll.FindAttack(info) == currentRoot && currentAttack.HasNext){
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

		}

		private void ChainAttack(){
			currentAttack?.ToolsOff();
			currentAttack = currentAttack.NextAttack();
			currentAttack.ToolsOn();
			attackAnimControll.Play(currentAttack);
			InAttack = true;
			hitEnable = true;
		}
	}
}