using System;
using UniRx;
using Zenject;

namespace doma.Inputs{
	public class InputRelayPoint{
		private readonly CurrentInputReciever currentInputReciever;
		
		private readonly IBattleKeySender iBattleKeySender;
		private readonly IUikeySender iUikeySender;


		private IInputReciever current;
		private IInputReciever before;
		
		private bool isActive = false;

		public bool IsActive{
			get{
				return isActive;
			}
			set{
				if (freeze){
					DebugLogger.LogError("Input is freeze now, current key is "+freezeKey);
					return;;
				}
				DebugLogger.Log("change active to "+value);
				isActive = value;
				if (value){
					currentInputReciever.currentReciever?.StartInputRecieve();
				}
				else{
					currentInputReciever.currentReciever?.EndInputRecieve();
				}
			}
		}

		private bool freeze=false;
		private System.Object freezeKey=null;


		private Subject<Unit> DelayForUndo=new Subject<Unit>();
		
		[Inject]
		public InputRelayPoint(IBattleKeySender i_battle_key_sender,IUikeySender i_uikey_of_event){
			iBattleKeySender = i_battle_key_sender;
			iUikeySender = i_uikey_of_event;
			currentInputReciever=new CurrentInputReciever();
			DefineSubscribe();

			/*DelayForUndo
				.Delay(TimeSpan.FromSeconds(1f))
				.Subscribe(n => ChangeReciever(before));*/
		}

		
		public void ChangeReciever(IInputReciever reciever){
			if (reciever == current){
				return;
			}
			
			if (!currentInputReciever.UpdateReciever(reciever)){
				DebugLogger.LogError(reciever+" isnt reciever class!!");
				return;
			}

			before = current;
			current = reciever;
			DebugLogger.Log("change input reciever to "+reciever);
		}

		public void UndoReciever(){
			if (before == null){
				DebugLogger.LogError("Canct Undo bc I havent before reciever");
				return;
			}
			DebugLogger.Log("run undo..... "+before);
			ChangeReciever(before);
		}

		public void RecieverClear(){
			currentInputReciever.Reset();
			before = current;
			current = null;
			DebugLogger.Log("------clear reciever------");
		}

		public void Freeze(System.Object trigger){
			if (freezeKey == null){
				freeze = true;
				freezeKey = trigger;
			} else{
				DebugLogger.LogError("Input freeze now. current key is "+freezeKey);
			}
		}

		public void UnFreeze(System.Object trigger){
			if (!freeze){
				DebugLogger.LogError("Input isnt freeze now");
				return;
			}
			
			if (freezeKey != null){
				if (System.Object.ReferenceEquals(freezeKey, trigger)){
					freeze = false;
					freezeKey = null;
				} else{
					DebugLogger.LogError(trigger + "isnt current key");
				}
			}
		}
		

		private void DefineSubscribe(){
			iBattleKeySender.HorizontalAxsis
				.Where(n => this.isActive)
				.Subscribe(n => currentInputReciever.IBattleKyReciever?.ChangeHorizontalAxis(n));
			iBattleKeySender.VerticalAxsis
				.Where(n => this.isActive)
				.Subscribe(n => currentInputReciever.IBattleKyReciever?.ChangeVerticalAxis(n));
			iBattleKeySender.JumpKey
				.Where(n => this.isActive)
				.Subscribe(n => currentInputReciever.IBattleKyReciever?.JumpKey());
			iBattleKeySender.RangeAtKey
				.Where(n => this.isActive)
				.Subscribe(n => currentInputReciever.IBattleKyReciever?.RangeAtKey());
			iBattleKeySender.WeakAtKey
				.Where(n => this.isActive)
				.Subscribe(n => currentInputReciever.IBattleKyReciever?.WeakAtKey());
			iBattleKeySender.StrongAtKey
				.Where(n => this.isActive)
				.Subscribe(n => currentInputReciever.IBattleKyReciever?.StrongAtKey());
			iBattleKeySender.GuardKey
				.Where(n => this.isActive)
				.Subscribe(n => currentInputReciever.IBattleKyReciever?.GuardKey());
			



			iUikeySender.UpKey
				.Where(n => this.isActive)
				.Subscribe(n => {
					currentInputReciever.iUikeyReciever?.UpKey();
				});
			iUikeySender.DownKey
				.Where(n => this.isActive)
				.Subscribe(n => currentInputReciever.iUikeyReciever?.DownKey());
			iUikeySender.RightKey
				.Where(n => this.isActive)
				.Subscribe(n => currentInputReciever.iUikeyReciever?.RightKey());
			iUikeySender.LeftKey
				.Where(n => this.isActive)
				.Subscribe(n => currentInputReciever.iUikeyReciever?.LeftKey());
			iUikeySender.EnterKey
				.Where(n => this.isActive)
				.Subscribe(n => currentInputReciever.iUikeyReciever?.EnterKey());
			iUikeySender.CancelKey
				.Where(n => this.isActive)
				.Subscribe(n => currentInputReciever.iUikeyReciever?.CancelKey());

			
		}
		
		
		
		public class CurrentInputReciever{		
			public IBattleKyReciever IBattleKyReciever{ get; set; }
			public IUikeyReciever iUikeyReciever{ get; set; }

			public IInputReciever currentReciever{
				get{
					if (IBattleKyReciever != null){
						return IBattleKyReciever;
					}

					if (iUikeyReciever != null){
						return iUikeyReciever;
					}

					return null;
				}
			}

			public CurrentInputReciever(){}
			
			public bool UpdateReciever(IInputReciever reciever){
				Reset();
				var res = false;
				
				if (reciever is IBattleKyReciever){
					IBattleKyReciever = reciever as IBattleKyReciever;
					res=true;
				}
				if (reciever is IUikeyReciever){
					iUikeyReciever=reciever as IUikeyReciever;
					res = true;
					//DebugLogger.Log("ui");
				}

				if (res){
					reciever.StartInputRecieve();
				}

				return res;

			}


			public void Reset(){
				var check_null_for_endaction =
					new Func<Action,IInputReciever,Action>((n,m) => {
						if (m == null){
							return n;
						}
						return m.EndInputRecieve;
					});
				
				Action end_action=null;
				end_action = check_null_for_endaction(end_action,IBattleKyReciever);
				end_action = check_null_for_endaction(end_action,iUikeyReciever);
			
				end_action?.Invoke();

				IBattleKyReciever = null;
				iUikeyReciever = null;

			}

		}
	}
}