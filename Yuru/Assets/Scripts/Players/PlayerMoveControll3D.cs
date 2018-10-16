using System;
using Animations;
using doma;
using doma.Inputs;
using UniRx;
using UnityEngine;
using Zenject;

namespace Players{
	public class PlayerMoveControll3D :PlayerMove{
		public override void Move(){
			if (Math.Abs(HorizontalMovement) < 0.01f && Math.Abs(VerticalMovement) < 0.01f){
				Stop();
				return;
			}

			var z = lookTarget.forward * VerticalMovement * speed;
			var x = lookTarget.right * HorizontalMovement * speed;
			rigid.velocity = new Vector3(0, rigid.velocity.y, 0) + z + x;
			transform.LookAt(transform.position + z + x);
			if (!InJumping){
				PlayMotion(motionAnimControll.MyDic.RunName);
			}
		}
	}
}