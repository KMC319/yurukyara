using System;
using System.Collections.Generic;
using System.Linq;
using Battles.Attack;
using Battles.Systems;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Players{
	public class AttackControll : MonoBehaviour{
		[SerializeField] private float keyBufferTime;
		private AttackBox currentAttack;
		private AttackBox currentRoot;
		private PlayerKeyCode? keyBuffer;
		
		private AttackAnimControll attackAnimControll;
		[NonSerialized]public MoveCotroll iMoveCotroll;

		private PlayerRoot taregtPlayer;

		private PlayerRoot TaregtPlayer{
			get{
				if (taregtPlayer == null){
					taregtPlayer = this.GetComponent<IPlayerBinder>().TargetPlayerRoot;
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

		private CommandType CurrentState => iMoveCotroll.InJumping ? CommandType.Jump : CommandType.Normal;

		public bool InAttack{ get; private set; }
		public bool AttackEnable{ get; private set; }
		public AttackType? GetCurrentType => currentAttack?.attackDamageBox.attackType;
		private bool hitEnable;

		private void Start(){
			attackAnimControll = this.GetComponentInChildren<AttackAnimControll>();
			
			attackAnimControll.ResponseStream.Subscribe(RecieveResponce);
			
			transform.GetComponentsInChildren<AttackToolEntity>()
				.Select(n => n.HitStream)
				.Merge()
				.Subscribe(RecieveHit);
			this.ObserveEveryValueChanged(n => n.InAttack).Where(n => n)
				.Subscribe(n => {
					if (currentAttack.attackInputInfo.commandType != CommandType.Jump){
						iMoveCotroll.Pause();
						iMoveCotroll.ForceFall();
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

		private void RecieveHit(GameObject other){
			if (!(InAttack && hitEnable)) return;
			if (other != TaregtPlayer.gameObject) return;
			
			hitEnable = false;

			//相殺処理。汚い
			if (TaregtPlayer.AttackControll.GetCurrentType != null){
				var check_normal_attack=new Func<AttackType,bool>(
					(n)=>n==AttackType.Weak||n==AttackType.Strong);
				var my = (AttackType) GetCurrentType;
				var en = (AttackType) TaregtPlayer.AttackControll.GetCurrentType;
				
				if ((check_normal_attack(my)&&
				     en==AttackType.Grab&&
				    !AttackEnable)||
				    (my==AttackType.Grab&&
				     check_normal_attack(en) &&
				     !TaregtPlayer.AttackControll.AttackEnable)){
					AttackEnd();//相殺
				}else if(my==AttackType.Grab&&
				         check_normal_attack(en)&&
				         TaregtPlayer.AttackControll.AttackEnable){
					return;//掴み無効化
				}
			}
			DebugLogger.Log("s"+other);
			//連続モーションの判定
			if (currentAttack.HasNext && currentAttack.NextAttack().attackInputInfo.commandType ==CommandType.Chain){
				ChainAttack();
				return;
			}
			//ここまで行ったらダメージをコール
			TaregtPlayer.DamageControll.Hit(currentAttack.attackDamageBox);
		}

		private void RecieveResponce(AnimResponce anim_responce){
			if (anim_responce == AnimResponce.AttackEnd){
				AttackEnd();
			}
		}

		private void AttackEnd(){
			currentAttack.ToolsOff();
			currentAttack = null;
			currentRoot = null;
			keyBuffer = null;
			InAttack = false;
			hitEnable = false;
			AttackEnable = false;
			attackAnimControll.CashClear();
		}

		private void Attack(PlayerKeyCode player_key_code){
			AttackBox result = null;
			var info=new AttackInputInfo(new List<PlayerKeyCode>(){player_key_code},CurrentState,CurrentPhase);
			
			if (keyBuffer != null){
				var duo_info=info;
				duo_info.keyCodes = new List<PlayerKeyCode>(info.keyCodes){(PlayerKeyCode) keyBuffer};
				result = attackAnimControll.FindAttack(duo_info);
			}
			
			if (result == null){
				if (currentAttack == null){
					result = attackAnimControll.FindAttack(info);
					var str =result+",";
				}else if (attackAnimControll.FindAttack(info) == currentRoot && 
				          currentAttack.HasNext &&
				          currentAttack.NextAttack().attackInputInfo.commandType!=CommandType.Chain){
					result = currentAttack.NextAttack();
				}
			}

			if(result==null)return;//ここでNullなら攻撃がないので非実行
			currentAttack?.ToolsOff();
			currentAttack = result;
			Observable
				.Timer(TimeSpan.FromSeconds(result.delayTimeForTools))
				.Subscribe(n => {
					if (result == currentAttack){
						currentAttack.ToolsOn();
						AttackEnable = true;
					}
				});
			if (currentRoot == null) currentRoot = result;
			attackAnimControll.ChangeAnim(currentAttack);
			
			InAttack = true;
			hitEnable = true;

		}

		private void ChainAttack(){//連続モーションの実行はここ（コンボは違う
			currentAttack?.ToolsOff();
			currentAttack = currentAttack.NextAttack();
			currentAttack.ToolsOn();
			attackAnimControll.ChangeAnim(currentAttack);
			InAttack = true;
			hitEnable = true;
		}
	}
}