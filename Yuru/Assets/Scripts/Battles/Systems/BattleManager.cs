using System.Collections;
using Battles.Players;
using doma;
using UnityEngine;
using UniRx;

namespace Battles.Systems{
	public class BattleManager : MonoBehaviour{
		[SerializeField] private float battleTime;

		[SerializeField] private Timer timer;
		[SerializeField] private CenterMesseage centerMesseage;

		private IPlayerBinder[] iplayerBinders;
		
		private void Start(){
			iplayerBinders = this.transform.GetComponentsInChildren<IPlayerBinder>();
			if (iplayerBinders.Length != 2){
				DebugLogger.LogError("I didnt get playerBinder,char generate error");
			}

			StartCoroutine(StartIvent());

			timer.EndStream.Subscribe(n =>StartCoroutine(EndIvent()));
		}

		private IEnumerator StartIvent(){
			foreach (var item in iplayerBinders){
				item.SetInputEnable(false);
			}
			centerMesseage.Diaply("Fight!!");
			timer.Set(battleTime);
			yield return new WaitForSeconds(1);
			centerMesseage.Diaply("");
			timer.TimerStart();
			foreach (var item in iplayerBinders){
				item.SetInputEnable(true);
			}
		}

		private IEnumerator EndIvent(){
			yield return null;
		}
	}
}