using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battles.Players;
using doma;
using UnityEngine;
using UniRx;

namespace Battles.Systems{
	public class BattleManager : MonoBehaviour{
		[SerializeField] private float battleTime;

		[SerializeField] private Timer timer;
		[SerializeField] private CenterMesseage centerMesseage;

		private List<IPlayerBinder> iplayerBinders;

		private HealthManagersControll healthManagersControll;
		
		private void Start(){
			iplayerBinders = this.transform.GetComponentsInChildren<IPlayerBinder>().ToList();
			healthManagersControll = this.transform.GetComponentInChildren<HealthManagersControll>();
			
			if (iplayerBinders.Count != 2){
				DebugLogger.LogError("I didnt get playerBinder,char generate error");
			}

			StartCoroutine(StartIvent());

			timer.EndStream.Subscribe(n =>StartCoroutine(TimeOver()));
			healthManagersControll.DeadPlayerStream.Subscribe(n => StartCoroutine(BreakOut(n)));

		}

		private void ChangeInputEnable(bool f){
			foreach (var item in iplayerBinders){
				item.SetInputEnable(f);
			}
		}

		private void WinnerProcess(PlayerNum player_num){
			string res;
			if (player_num == PlayerNum.None){
				res = "Draw";
			}else{
				res = "Winner-" + player_num;
			}
			centerMesseage.Display(res);
		}

		private IEnumerator StartIvent(){
			ChangeInputEnable(false);
			centerMesseage.Display("Fight!!");
			timer.Set(battleTime);
			yield return new WaitForSeconds(1);
			centerMesseage.Display("");
			timer.TimerStart();
			ChangeInputEnable(true);
		}

		private IEnumerator TimeOver(){
			WinnerProcess(healthManagersControll.CheckMoreHealthPlayer());
			ChangeInputEnable(false);

			yield return null;
		}

		private IEnumerator BreakOut(PlayerRoot player_root){
			WinnerProcess(iplayerBinders.Find(n => n.TargetPlayerRoot ==player_root).PlayerNum);
			ChangeInputEnable(false);
			timer.Pause();
			yield return null;
		}
	}
}