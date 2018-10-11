using System;
using UnityEngine;

namespace Players{
	public class PlayerMoveControll2D :PlayerMove{
		public override void Move(){
			if (Math.Abs(HorizontalMovement) < 0.01f && Math.Abs(VerticalMovement) < 0.01f){
				Stop();
				return;
			}

			var z = lookTarget.forward * HorizontalMovement * speed;
			rigid.velocity = new Vector3(0, rigid.velocity.y, 0) + z ;
			transform.LookAt(transform.position + z );
			if (!InJumping){
				PlayMotion(motionAnimControll.MyDic.RunName);
			}
		}
	}
}