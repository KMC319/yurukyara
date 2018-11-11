using System;
using doma;
using UnityEngine;

namespace Battles.Players{
	public class MoveCotroll3D :MoveCotroll{
		public override void Move(){
			if (Math.Abs(HorizontalMovement) < 0.01f && Math.Abs(VerticalMovement) < 0.01f){
				Stop();
				return;
			}
			var z = lookTarget.forward * VerticalMovement * speed;
			var x = lookTarget.right * HorizontalMovement * speed;
			rigid.velocity = new Vector3(0, rigid.velocity.y, 0) + z + x;
			transform.LookAt(transform.position + z + x);
			if (!InJumping&&jumpAble){
				PlayMotion(motionAnimControll.MyDic.RunName);
			}
		}
	}
}