using System;
using UniRx;
using UnityEngine;

namespace Players{
	public abstract class PlayerMove:MonoBehaviour{
		[SerializeField] protected float speed;
		[SerializeField] private float jumpPower;
		[SerializeField] private float fallDouble=1f;
		[SerializeField]private float jumpBuffer;
		
		protected Rigidbody rigid;
		protected Transform lookTarget;

		protected MotionAnimControll motionAnimControll;


		public float HorizontalMovement{ get; set; }
		public float VerticalMovement{ get; set; }
		public bool InJumping{ get; set; }
		private bool inFall;
		
		
		private void Start() {
			rigid = GetComponent<Rigidbody>();
			lookTarget = transform.Find("LookTarget");

			motionAnimControll = this.transform.GetComponentInChildren<MotionAnimControll>();
		}

		

		public abstract void Move();
		
		public virtual void Stop() {
			if (!InJumping){
				PlayMotion(motionAnimControll.MyDic.WaitName);
				transform.rotation = lookTarget.rotation;
			}

			rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
			rigid.angularVelocity = Vector3.zero;
		}

		public virtual void Cancel(){
			transform.rotation = lookTarget.rotation;
			rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
			rigid.angularVelocity = Vector3.zero;
		}

		public virtual void Jump(){
			if (InJumping) return;
			Observable.Timer(TimeSpan.FromSeconds(jumpBuffer)).Subscribe(n=>InJumping = true);
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
			}
		}

		protected void PlayMotion(string name){
			motionAnimControll.ChangeAnim(name);
		}
	}
}