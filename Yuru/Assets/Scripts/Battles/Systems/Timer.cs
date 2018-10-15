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
			if (Math.Abs(currentTime) > 0.01f&&moveAble){
				currentTime -= Time.deltaTime;
				txt.text = ((int) currentTime).ToString();
				if (Math.Abs(currentTime) < 0.01f){
					endStream.OnNext(Unit.Default);
					moveAble = false;
				}
			}
		}

		public void Launch(float time){
			if (moveAble){
				DebugLogger.LogError("Timer is running!!");
				return;
			}
			currentTime = time;
			moveAble = true;
		}

		public void ReStart(){
			moveAble = true;
		}
		public void Pause(){
			moveAble = false;
		}
	}
}