using System;
using Animations;
using doma;
using doma.Inputs;
using UniRx;
using UnityEngine;
using Zenject;

namespace Players{
	public class PlayerMoveControll3D : MonoBehaviour,IPlayerMove{

		[SerializeField] private float speed;
		[SerializeField] private float jumpPower;
		[SerializeField] private float fallDouble=1f;
		
		private Rigidbody rigid;
		private Transform lookTarget;

		private MotionAnimControll motionAnimControll;


		public float HorizontalMovement{ get; set; }
		public float VerticalMovement{ get; set; }
		public bool InJumping{ get; set; }
		private bool inFall;
		
		
		private void Start() {
			rigid = GetComponent<Rigidbody>();
			lookTarget = transform.Find("LookTarget");

			motionAnimControll = this.transform.GetComponentInChildren<MotionAnimControll>();
		}

		// Update is called once per frame


		 public void Move() {
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

		public void Stop() {
			if (!InJumping){
				PlayMotion(motionAnimControll.MyDic.WaitName);
				transform.rotation = lookTarget.rotation;
			}

			rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
			rigid.angularVelocity = Vector3.zero;
		}

		public void Cancel(){
			rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
			rigid.angularVelocity = Vector3.zero;
		}

		public void Jump(){
			if (InJumping) return;
			InJumping = true;
			PlayMotion(motionAnimControll.MyDic.JumpName);
			rigid.AddForce(Vector3.up*jumpPower,ForceMode.Impulse);
		}

		public void FallCheck(bool in_attack){
			if(!InJumping)return;
			if (!inFall&&rigid.velocity.y < -0.1f){
				rigid.AddForce(Vector3.down*jumpPower*fallDouble,ForceMode.Impulse);
				inFall = true;
			}

			if (inFall && !in_attack){
				PlayMotion(motionAnimControll.MyDic.FallName);
			}

			if (inFall && Math.Abs(rigid.velocity.y) < 0.01f){
				inFall = false;
				InJumping = false;
			}
		}

		private void PlayMotion(string name){
			motionAnimControll.ChangeAnim(name);
		}
	}
}