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
	public class BattleManager : MonoBehaviour{

		
		[SerializeField] private float battleTime;
	
		
		private static int round=1;

		[SerializeField] private Timer timer;
		[SerializeField] private CenterMesseage centerMesseage;
		[SerializeField] private WinnerCounterDisplay[] countersDisplay;
		[SerializeField] private GameObject endGameMenu;

		private Pausable pausable;
		
		private List<IPlayerBinder> iplayerBinders;

		private HealthManagersControll healthManagersControll;
		private WinerReferee winerReferee;

		private bool inFight;
		
		
		private void Start(){
			iplayerBinders = this.transform.GetComponentsInChildren<IPlayerBinder>().ToList();
			pausable = this.GetComponent<Pausable>();
			healthManagersControll = this.transform.GetComponentInChildren<HealthManagersControll>();
			
			if (iplayerBinders.Count != 2){
				DebugLogger.LogError("I didnt get playerBinder,char generate error");
			}
			CountDisply();

			StartCoroutine(StartIvent());

			timer.EndStream.Subscribe(n =>StartCoroutine(TimeOver()));
			healthManagersControll.DeadPlayerStream.ThrottleFirst(TimeSpan.FromSeconds(1)).Subscribe(n => StartCoroutine(BreakOut(n)));
		}

		private void Update(){
			if(!inFight)return;
			if (Input.GetButtonDown("Start")){
				pausable.Submit();
				timer.ReverseMoveAble();
			}
		}

		private void ChangeInputEnable(bool f){
			foreach (var item in iplayerBinders){
				item.SetInputEnable(f);
			}
		}

		private void CountDisply(){
			foreach (var item in countersDisplay){
				item.Count = WinerReferee.GetCount(item.GetPlayerNum);
			}
		}

		private void WinnerDisplay(PlayerNum winner){	
			winerReferee=new WinerReferee(winner);
			
			string res;
			if (winner == PlayerNum.None){
				res = "Draw";
			}else{
				res = "Winner-" + winner;
			}
			CountDisply();
			centerMesseage.Display(res);
		}

		private void CheckRoundEnd(){
			ChangeInputEnable(false);
			var continue_fg=true;
			round++;

			var roundWinner=PlayerNum.None;

			try{
				roundWinner = winerReferee.GetRoundWinner();
			} catch (Exception e){
				centerMesseage.Display("Fight End!.Round Draw!");
				continue_fg = false;
			}
			
			if (roundWinner!=PlayerNum.None){
				centerMesseage.Display("Fight End!.Winner-"+roundWinner);
				continue_fg = false;
			}
			
			
			if (continue_fg){
				SceneManager.LoadScene("Battle");
			} else{
				endGameMenu.SetActive(true);
			}

		}
		

		private IEnumerator StartIvent(){
			ChangeInputEnable(false);
			centerMesseage.Display("Round-"+round+"- Fight!!");
			timer.Set(battleTime);
			yield return new WaitForSeconds(1);
			centerMesseage.Display("");
			timer.TimerStart();
			ChangeInputEnable(true);
			inFight = true;
		}

		private IEnumerator TimeOver(){
			inFight = false;
			ChangeInputEnable(false);
			WinnerDisplay(healthManagersControll.CheckMoreHealthPlayer());
			yield return new WaitForSeconds(1);
			CheckRoundEnd();
		}

		private IEnumerator BreakOut(PlayerRoot player_root){
			inFight = false;
			ChangeInputEnable(false);
			timer.Pause();
			WinnerDisplay(iplayerBinders.Find(n => n.TargetPlayerRoot ==player_root).PlayerNum);
			yield return new WaitForSeconds(1);
			CheckRoundEnd();
		}

		public void ReGame() {
			round = 1;
			WinerReferee.Reset();
			SceneManager.LoadScene("Battle");
		}

		public void BreakGame(string Scene) {
			round = 1;
			WinerReferee.Reset();
			SceneManager.LoadScene(Scene);
		}
	}
}