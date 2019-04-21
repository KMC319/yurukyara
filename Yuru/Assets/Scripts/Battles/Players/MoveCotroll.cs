using System;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Players{
	public abstract class MoveCotroll:MonoBehaviour,IPlayerCancelProcess{
		[SerializeField] protected float speed;
		[SerializeField] private float jumpPower;
		[SerializeField] private float fallDouble=1f;

		public bool IsActive{ get; set; }
		protected Rigidbody rigid;
		public Transform lookTarget;

		protected MotionAnimControll motionAnimControll;

		public bool LookLock { get; set; }

		public float HorizontalMovement{ get; set; }
		public float VerticalMovement{ get; set; }
		public bool InJumping{ get; set; }
		private bool inFall;

		protected bool jumpAble = true;
		
		protected virtual void Start() {
			rigid = GetComponent<Rigidbody>();
			lookTarget = transform.Find("LookTarget");

			motionAnimControll = this.transform.GetComponentInChildren<MotionAnimControll>();

			motionAnimControll.ResponseStream
				.Where(n=>this.IsActive)
				.Where(n => n == AnimResponce.JumpLaunch)
				.Subscribe(n => {
					Jump();
			});
		}


		public abstract void Move();
		
		public virtual void Stop() {
			if (!InJumping&&jumpAble){
				PlayMotion(motionAnimControll.MyDic.WaitName);
				transform.rotation = lookTarget.rotation;
			}

			rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
			rigid.angularVelocity = Vector3.zero;
		}

		public virtual void Pause(){
			transform.rotation = lookTarget.rotation;
			rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
			rigid.angularVelocity = Vector3.zero;
		}

		public virtual void JumpStart(){
			if (InJumping||!jumpAble) return;
			jumpAble = false;
			var name = motionAnimControll.MyDic.JumpStartName;
			if (name == ""){
				Jump();
			} else{
				motionAnimControll.ChangeAnim(name);
				rigid.velocity = new Vector3(0, rigid.velocity.y, 0);	
			}
		}

		protected virtual void Jump(){
			if(InJumping)return;
			InJumping = true;
			rigid.AddForce(Vector3.up*jumpPower,ForceMode.Impulse);
		}

		public virtual void ForceFall(){
			rigid.velocity=new Vector3(rigid.velocity.x,0,rigid.velocity.z);
			rigid.AddForce(Vector3.down*jumpPower*fallDouble,ForceMode.Impulse);
		}

		public virtual void FallCheck(bool other_motion){
			if(!InJumping)return;
			if (!inFall&&rigid.velocity.y > 0.1f&&!other_motion){
				PlayMotion(motionAnimControll.MyDic.JumpName);
			}
			if (!inFall&&rigid.velocity.y < -0.1f){
				rigid.AddForce(Vector3.down*jumpPower*fallDouble,ForceMode.Impulse);
				inFall = true;
			}

			if (inFall && !other_motion){
				PlayMotion(motionAnimControll.MyDic.FallName);
			}

			if (inFall && Math.Abs(rigid.velocity.y) < 0.01f){
				inFall = false;
				InJumping = false;
				jumpAble = true;
			}
		}

		protected void PlayMotion(string name){
			motionAnimControll.ChangeAnim(name);
		}

		public void Cancel(){
			inFall = false;
			InJumping = false;
			jumpAble = true;
			ForceFall();
		}
	}
}