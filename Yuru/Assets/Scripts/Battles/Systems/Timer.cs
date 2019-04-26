using System;
using doma;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Battles.Systems{
	public class Timer : MonoBehaviour{
		private float currentTime=0;

		private Text txt;
		private bool moveAble;

		private readonly Subject<Unit> endStream=new Subject<Unit>();
		public UniRx.IObservable<Unit> EndStream => endStream;
		
		private void Start(){
			txt = this.GetComponent<Text>();
			txt.text = "";
		}

		private void Update(){
			if (currentTime >= 0 && moveAble){
				currentTime -= Time.deltaTime;
				txt.text = ((int) currentTime).ToString();
				if (currentTime < 0){
					moveAble = false;
					endStream.OnNext(Unit.Default);
				}
			}
		}

		public void Set(float time){
			if (moveAble){
				DebugLogger.LogError("Timer is running!!");
				return;
			}
			currentTime = time;
		}

		public void TimerStart(){
			moveAble = true;
		}

		public void ReverseMoveAble(){
			moveAble = !moveAble;
		}
		public void Pause(){
			moveAble = false;
		}
	}
}