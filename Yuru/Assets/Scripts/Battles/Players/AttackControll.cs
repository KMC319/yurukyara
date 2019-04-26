using System;
using System.Collections.Generic;
using System.Linq;
using Battles.Attack;
using Battles.Systems;
using doma;
using UniRx;
using UnityEngine;
using Zenject;

namespace Battles.Players{
	public class AttackControll : MonoBehaviour,IPlayerCancelProcess{
		[SerializeField] private float keyBufferTime;
		private AttackBox currentAttack;
		private AttackBox currentRoot;
		private PlayerKeyCode? keyBuffer;
		
		private AttackAnimControll attackAnimControll;
		[NonSerialized]public MoveCotroll iMoveCotroll;
		private IPlayerCancelProcess[] playerCancelProcesses;
		
		private readonly Subject<AttackDamageBox> onAttackHit=new Subject<AttackDamageBox>();
		public UniRx.IObservable<AttackDamageBox> OnAttackHit => onAttackHit;

		private AttackBox[] AttackBoxs;
		
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
	
		private void Start() {
			AttackBoxs = transform.GetComponentInChildren<BoxContainer>().AttackBoxs.ToArray();
			attackAnimControll = this.GetComponentInChildren<AttackAnimControll>();
			playerCancelProcesses = transform.GetComponentsInChildren<IPlayerCancelProcess>();
		
			attackAnimControll.ResponseStream.Subscribe(RecieveResponce);

			AttackBoxs.SelectMany(n=>n.GetTree()).ToList()
				.ForEach(n => {
					foreach (var item in n.attackTools.OfType<AttackToolEntity>()) {
						item.HitStream
							.Where(m=>m==TaregtPlayer.gameObject)
							.Where(m=>n.attackDamageBox.attackType==AttackType.Shot||
							          n.attackDamageBox.attackType==AttackType.Grab||
							          n==currentAttack)
							.ThrottleFirst(TimeSpan.FromSeconds(0.5f))
							.Subscribe(m => {
								onAttackHit.OnNext(n.attackDamageBox);
								if (currentAttack!= null && currentAttack.HasNext &&
								    currentAttack.NextAttack().attackInputInfo.commandType ==CommandType.Chain){
									ChainAttack();
								}
							});
					}
				});

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

		private void RecieveResponce(AnimResponce anim_responce){
			if (anim_responce == AnimResponce.AttackEnd){
				currentAttack?.ToolsOff();
				AttackEnd();
			}
		}

		public void Cancel(){
			currentAttack?.ToolsCancel();
			AttackEnd();
		}

		public void AttackEnd(){
			InAttack = false;
			currentRoot = null;
			keyBuffer = null;
			AttackEnable = false;
			currentAttack = null;
			attackAnimControll.CashClear();
		}

		private void Attack(PlayerKeyCode player_key_code){
			AttackBox result = null;
			var info=new AttackInputInfo(new List<PlayerKeyCode>(){player_key_code},CurrentState,CurrentPhase);

			if (keyBuffer != null) {
				var duo_info = info;
				duo_info.keyCodes = new List<PlayerKeyCode>(info.keyCodes) {(PlayerKeyCode) keyBuffer};
				result = FindAttack(duo_info);
				if (result != null) {
					foreach (var item in playerCancelProcesses.Where(i=>!(i is AttackControll))) {
						item.Cancel();
					}
				}
			}

			if (result == null){
				if (currentAttack == null){
					result = FindAttack(info);
					var str =result+",";
				}else if (FindAttack(info) == currentRoot && 
				          currentAttack.HasNext &&
				          currentAttack.NextAttack().attackInputInfo.commandType!=CommandType.Chain){
					result = currentAttack.NextAttack();
				}
			}

			if (currentAttack != null && currentAttack == result) return;
			if(result==null)return;//ここでNullなら攻撃がないので非実行
			currentAttack?.ToolsCancel();
			currentAttack = result;
			Observable
				.Timer(TimeSpan.FromSeconds(result.delayTimeForTools))
				.Where(n=>this.InAttack)
				.Subscribe(n => {
					if (result == currentAttack){
						currentAttack.ToolsOn();
						AttackEnable = true;
					}
				});
			if (currentRoot == null) currentRoot = result;
			attackAnimControll.ChangeAnim(currentAttack);
			
			InAttack = true;
		}

		private void ChainAttack(){//連続モーションの実行はここ（コンボは違う
			currentAttack?.ToolsOff();
			currentAttack = currentAttack.NextAttack();
			currentAttack.ToolsOn();
			attackAnimControll.ChangeAnim(currentAttack);
			InAttack = true;
		}
		
		private AttackBox FindAttack(AttackInputInfo info){
			return  AttackBoxs
				.Where(n => n.attackInputInfo.keyCodes.Count == info.keyCodes.Count)
				.Where(n=>n.attackInputInfo.applyPhase==ApplyPhase.Both||n.attackInputInfo.applyPhase == info.applyPhase)
				.Where(n => n.attackInputInfo.commandType == info.commandType)
				.ToList()
				.Find(n =>n.attackInputInfo.keyCodes.OrderBy(v=>v)
					.SequenceEqual(info.keyCodes.OrderBy(w=>w)));
		}
	}
}