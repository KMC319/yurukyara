using System;
using doma.Inputs;
using UnityEngine;
using Zenject;

namespace Players{
	public class PlayerMoveControll : MonoBehaviour,IBattleKyReciever{

		[SerializeField] private float speed;
		[SerializeField] private PlayerAnimDictionary myDic;
		
		private float horizontalMovement;
		private float verticalMovement;
		
		private Rigidbody rigid;
		private Transform lookTarget;

		private PlayerAnimControll playerAnimControll;
    
		private void Start() {
			rigid = GetComponent<Rigidbody>();
			lookTarget = transform.Find("LookTarget");

			playerAnimControll = this.GetComponent<PlayerAnimControll>();
		}

		// Update is called once per frame
		void Update() {
        
		}

		private void FixedUpdate(){
			Move();
		}

		private void Move() {
			if (Math.Abs(horizontalMovement) < 0.01f && Math.Abs(verticalMovement) < 0.01f){
				Stop();
				return;
			}
			var z = lookTarget.forward * Input.GetAxis("Vertical") * speed;
			var x = lookTarget.right * Input.GetAxis("Horizontal") * speed;
			rigid.velocity = new Vector3(0, rigid.velocity.y, 0) + z + x;
			transform.LookAt(transform.position + rigid.velocity);
			playerAnimControll.ChangeAnim(myDic.RunName);
		}

		private void Stop() {
			playerAnimControll.ChangeAnim(myDic.WaitName);
			rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
			rigid.angularVelocity = Vector3.zero;
		}
		
		public void StartInputRecieve(){}

		public void EndInputRecieve(){
			horizontalMovement = 0;
			verticalMovement= 0;
			Stop();
		}

		public void ChangeHorizontalAxis(float delta){
			horizontalMovement = delta;
		}
		public void ChangeVerticalAxis(float delta){
			verticalMovement = delta;
		}

		public void JumpKey(){}

		public void RangeAtKey(){}

		public void WeakAtKey(){}

		public void StrongAtKey(){}

		public void GuardKey(){}
	}
}