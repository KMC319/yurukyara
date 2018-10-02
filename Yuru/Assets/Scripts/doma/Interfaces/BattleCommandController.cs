/*
using System;
using Interfaces.Commands;
using UniRx;
using UnityEngine;
using Zenject;

namespace Interfaces.Battles{
	/// <summary>
	/// this class is controll command in battle.
	/// when recieve Skill or Item command,This bypass selected panel to each controller 
	/// </summary>
	public class BattleCommandController : MonoBehaviour,ICommandControll,IBattleManagers{
		[SerializeField] private CommandState nowstate;


		public Vector2 LastSelected{ get; set; }
		
		private ICommandControll currentCommandControll;
		private ICommandSubControll currentCommandSubControll;
		private IRockOnCursoleControll currentRockOnControll;

		[Inject]private AttackCommandSubController attackCommandSubController;
		[Inject]private SkillCommandSubController skillCommandSubController;
		[Inject]private ItemCommandSubController itemCommandSubController;
		[Inject]private EscapeCommandManager escapeCommandManager;
		
		[Inject]private PlayerBattle playerbattle;
		[Inject]private RockOnCursole rockoncursole;
		
	
		private CommandContainer commandContainer;

		[Inject] private InterfaceEventSystem interfaceEventSystem;
		[Inject] private BattleLog battleLog;
		[Inject] private InterfacePanelControll interfacePanelControll;
		
		void Start (){
			commandContainer = this.GetComponent<CommandContainer>();

			
			playerbattle.TurnStart.Subscribe(n=>Open()).AddTo(gameObject);
			playerbattle.TurnEnd.Subscribe(n =>Close() ).AddTo(gameObject);
			
			rockoncursole.EnterSubject.Subscribe(n=>currentRockOnControll?.TargetSelect(n)).AddTo(gameObject);


			interfaceEventSystem.EnterSelectable.Delay(System.TimeSpan.FromMilliseconds(1)).Subscribe(n=>currentCommandControll?.CommandSubmit(n)).AddTo(gameObject);
			interfaceEventSystem.FocusSelectable.Subscribe(n => currentCommandControll?.CommandSelect(n)).AddTo(gameObject);
			interfaceEventSystem.OverSubject.Subscribe(n=>currentCommandControll?.CursoleOverCheck(n)).AddTo(gameObject);
			interfaceEventSystem.CancelKey.Subscribe(n => Cancel());
			interfaceEventSystem.BootNotification.Subscribe(n => Cancel());
		}

		public void CommandSubmit(ISelectablePanel iselectablepanel){
			//DebugLogger.Log("currentcommandcontroll="+currentCommandControll);
			if (!(iselectablepanel is CommandPanel)){
				DebugLogger.LogError(iselectablepanel+" is should not be selecting");
				return;
			}
			var commandpanel=(CommandPanel) iselectablepanel;
			nowstate = commandpanel.GetMyCommand;
			battleLog.LogReset();
			switch (nowstate){
				case CommandState.Attack:
					LaunchSubControll(attackCommandSubController);
					break;
				case CommandState.Skill:
					commandContainer.PickUpCommand(1);
					LaunchSubControll(skillCommandSubController);
					break;
				case CommandState.Guard:
					playerbattle.GuardCommand();
					break;
				case CommandState.Item:
					commandContainer.PickUpCommand(3);
					LaunchSubControll(itemCommandSubController);
					break;
				case CommandState.Escape:
					escapeCommandManager.GoEscape();
					break;
			}
		}

		public void CommandSelect(ISelectablePanel iselectablepanel){
			//Empty
		}

		public void CursoleOverCheck(CursoleOver cursoleOver){
			//Empty
		}
		
		
		public void BattleStart(){
			LastSelected=Vector2.zero;
			commandContainer.SetChildImgEnable(true);
			commandContainer.SetAttackCommands();
			interfacePanelControll.OpenBase();
		}

		public void BattleEnd(bool escape = false){
			commandContainer.SetChildImgEnable(false);
			currentCommandControll = null;
			//interfacePanelControll.CloseAll();
		}


		public void Cancel(){
			if (currentCommandSubControll!=null){
				currentCommandControll.LastSelected = interfaceEventSystem.GetSelectedPoint;
				currentCommandSubControll.Finish();
				commandContainer.ReOpen();
				PresetControll();
			} else{
				DebugLogger.Log("now command panel");
			}
		}

		public void Open(){
			commandContainer.SetChildImgEnable(true);
			interfaceEventSystem.Freeze();
			commandContainer.Open();
			PresetControll();
		}

		public void Close(){
			commandContainer.SetChildImgEnable(false);
			if(currentCommandControll!=null)currentCommandControll.LastSelected = interfaceEventSystem.GetSelectedPoint;
			commandContainer.Close();
			currentCommandSubControll?.Finish();
			currentCommandControll = null;
			currentCommandSubControll = null;
			currentRockOnControll = null;
			interfaceEventSystem.Freeze();
		}

		private void LaunchSubControll<type>(type subcontroller){
			LastSelected=interfaceEventSystem.GetSelectedPoint;
			interfaceEventSystem.CreateActiveSelectableList<ChildCommandPanel>(ListCreateOption.Vertical);
		
			if (subcontroller is ICommandControll){
				currentCommandControll = subcontroller as ICommandControll;
			}
			if (subcontroller is ICommandSubControll){
				currentCommandSubControll = subcontroller as ICommandSubControll;
			}
			currentCommandSubControll?.Launch();
			if (subcontroller is IRockOnCursoleControll){
				currentRockOnControll = subcontroller as IRockOnCursoleControll;
			}

			Observable.Timer(TimeSpan.FromSeconds(0.15f)).Subscribe(n => {
				if (currentCommandControll != null){
					if (currentCommandControll.LastSelected.y != -1){
						interfaceEventSystem.Launch(0, (int) currentCommandControll.LastSelected.y);
					}
				}
			}).AddTo(gameObject);
		}

		private void PresetControll(){
			currentCommandControll = this;
			currentCommandSubControll = null;
			currentRockOnControll = null;
			interfaceEventSystem.CreateActiveSelectableList<CommandPanel>(ListCreateOption.Vertical);

			Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(n => {
				interfaceEventSystem.Launch(0, LastSelected.y);
			});
		}
	}
}*/
