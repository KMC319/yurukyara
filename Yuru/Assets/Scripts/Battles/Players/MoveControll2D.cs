using System;
using UnityEngine;

namespace Battles.Players{
	public class MoveControll2D :MoveCotroll {
		private GameObject vcam2d;

		protected override void Start() {
			base.Start();
			vcam2d = GameObject.Find("vcam 2d");
		}

		public override void Move(){
			if (Math.Abs(HorizontalMovement) < 0.01f ){
				Stop();
				return;
			}

			var z = vcam2d.transform.right * HorizontalMovement * speed;
			rigid.velocity = new Vector3(0, rigid.velocity.y, 0) + z ;
			transform.LookAt(transform.position + z );
			if (!InJumping&&jumpAble){
				PlayMotion(motionAnimControll.MyDic.RunName);
			}
		}
	}
}