using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battles.Players;
using doma;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

namespace Battles.Systems{
	[Serializable]public class WinnerCount{
		public PlayerNum playerNum;
		public int count;

		public WinnerCount(PlayerNum player_num){
			playerNum = player_num;
			this.count = 0;
		}
	}
	public class BattleManager : MonoBehaviour{

		
		[SerializeField] private float battleTime;
		private static List<WinnerCount> winnerCounters=new List<WinnerCount>(){
			new WinnerCount(PlayerNum.P1),new WinnerCount(PlayerNum.P2)
		};
		
		private static int round=1;

		[SerializeField] private Timer timer;
		[SerializeField] private CenterMesseage centerMesseage;
		[SerializeField] private WinnerCounter[] counters;

		private List<IPlayerBinder> iplayerBinders;

		private HealthManagersControll healthManagersControll;

		private void Start(){
			iplayerBinders = this.transform.GetComponentsInChildren<IPlayerBinder>().ToList();
			healthManagersControll = this.transform.GetComponentInChildren<HealthManagersControll>();
			
			if (iplayerBinders.Count != 2){
				DebugLogger.LogError("I didnt get playerBinder,char generate error");
			}
			CountDisply();

			StartCoroutine(StartIvent());

			timer.EndStream.Subscribe(n =>StartCoroutine(TimeOver()));
			healthManagersControll.DeadPlayerStream.Subscribe(n => StartCoroutine(BreakOut(n)));
		}

		private void ChangeInputEnable(bool f){
			foreach (var item in iplayerBinders){
				item.SetInputEnable(f);
			}
		}

		private void CountDisply(){
			foreach (var i in Enumerable.Range(0,winnerCounters.Count)){
				counters[i].Count = winnerCounters[i].count;
			}
		}

		private bool WinnerProcess(PlayerNum player_num){
			round++;
			
			string res;
			if (player_num == PlayerNum.None){
				res = "Draw";
				foreach (var item in winnerCounters){
					item.count++;
				}
			}else{
				res = "Winner-" + player_num;
				winnerCounters.Find(n => n.playerNum == player_num).count++;
			}
			CountDisply();
			
			var winer=winnerCounters.Find(n=>n.count==2);
			if (winer != null){
				centerMesseage.Display("Fight End!.Winner-"+winer.playerNum);
				return false;
			}
			centerMesseage.Display(res);
			return true;
		}

		private IEnumerator StartIvent(){
			ChangeInputEnable(false);
			centerMesseage.Display("Round-"+round+"- Fight!!");
			timer.Set(battleTime);
			yield return new WaitForSeconds(1);
			centerMesseage.Display("");
			timer.TimerStart();
			ChangeInputEnable(true);
		}

		private IEnumerator TimeOver(){
			ChangeInputEnable(false);
			var continue_fg=WinnerProcess(healthManagersControll.CheckMoreHealthPlayer());
			yield return new WaitForSeconds(1);
			if (continue_fg){
				SceneManager.LoadScene("TestDoma");
			}
		}

		private IEnumerator BreakOut(PlayerRoot player_root){
			ChangeInputEnable(false);
			timer.Pause();
			var continue_fg=WinnerProcess(iplayerBinders.Find(n => n.TargetPlayerRoot ==player_root).PlayerNum);
			yield return new WaitForSeconds(1);
			if (continue_fg){
				SceneManager.LoadScene("TestDoma");
			}
		}
	}
}