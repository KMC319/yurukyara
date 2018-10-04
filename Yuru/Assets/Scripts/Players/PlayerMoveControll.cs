using System;
using Animations;
using doma;
using doma.Inputs;
using UniRx;
using UnityEngine;
using Zenject;

namespace Players{
	[RequireComponent(typeof(BoxContainer))]
	public class PlayerMoveControll : MonoBehaviour,IBattleKeyReciever{

		[SerializeField] private float speed;
		[SerializeField] private float jumpPower;
		[SerializeField] private float fallDouble=1f;

		[SerializeField] public PlayerAnimDictionary myDic;
		
		private AttackBox currentAttack;
		
		private float verticalMovement;
		private float horizontalMovement;
		private bool attackRigor;
		
		private Rigidbody rigid;
		private Transform lookTarget;

		private BoxContainer boxContainer;
		private PlayerAnimControll playerAnimControll;


		private bool inJumping;
		private bool inFall;
		private bool inAttack;
		
		private void Start() {
			rigid = GetComponent<Rigidbody>();
			lookTarget = transform.Find("LookTarget");

			boxContainer = this.GetComponent<BoxContainer>();
			playerAnimControll = this.transform.GetComponentInChildren<PlayerAnimControll>();
			
			playerAnimControll.ResponseStream.Subscribe(RecieveResponce);
		}

		// Update is called once per frame
		private void Update() {
			FallCheck();
		}

		private void FixedUpdate(){
			Move();
		}

		private void Move() {
			if(inAttack)return;;
			if (Math.Abs(horizontalMovement) < 0.01f && Math.Abs(verticalMovement) < 0.01f){
				Stop();
				return;
			}

			var z = lookTarget.forward * Input.GetAxis("Vertical") * speed;
			var x = lookTarget.right * Input.GetAxis("Horizontal") * speed;
			rigid.velocity = new Vector3(0, rigid.velocity.y, 0) + z + x;
			transform.LookAt(transform.position + rigid.velocity);
			if (!inJumping){
				PlayMotion(myDic.RunName);
			}
		}

		private void Stop() {
			if (!inJumping){
				PlayMotion(myDic.WaitName);
				
				transform.rotation = lookTarget.rotation;
			}

			rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
			rigid.angularVelocity = Vector3.zero;

		}

		private void Jump(){
			if (inJumping||inAttack) return;
			inJumping = true;
			PlayMotion(myDic.JumpName);
			rigid.AddForce(Vector3.up*jumpPower,ForceMode.Impulse);
		}

		private void FallCheck(){
			if(!inJumping)return;
			if (!inFall&&rigid.velocity.y < -0.1f){
				rigid.AddForce(Vector3.down*jumpPower*fallDouble,ForceMode.Acceleration);
				inFall = true;
			}

			if (inFall && !inAttack){
				//PlayMotion(myDic.FallName);
			}

			if (inFall && Math.Abs(rigid.velocity.y) < 0.01f){
				inFall = false;
				inJumping = false;
			}
		}

		private void PlayMotion(string name){
			playerAnimControll.ChangeAnim(boxContainer.FindAnim(name));
		}
		

		private void RecieveResponce(AnimResponce responce){
			switch (responce){
				case AnimResponce.Wait:
					break;
				case AnimResponce.AttackEnd:
					currentAttack = null;
					inAttack = false;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(responce), responce, null);
			}
		}

		private void Attack(string name){
			if (attackRigor) return;
			rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
			inAttack = true;
			attackRigor = true;
			if (currentAttack == null){
				currentAttack = boxContainer.FindAttack(name);
			}else{
				if (currentAttack.nextAttack != null && 
				    currentAttack.nextAttack.Length==1&&
				    currentAttack.nextAttack[0].clip != null){	
					currentAttack = currentAttack.nextAttack[0];
				}	
			}
			playerAnimControll.ChangeAnim(currentAttack);
			Observable.Timer(TimeSpan.FromSeconds(currentAttack.rigorTime))
				.Subscribe(_ => {
					attackRigor = false;
				});
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

		public void JumpKey(){
			Jump();
		}

		public void RangeAtKey(){
		}

		public void WeakAtKey(){
			if (inJumping){
				Attack(myDic.JumpAtName);
			}
			else{
				Attack(myDic.WeakName);
			}
		}

		public void StrongAtKey(){
			Attack(myDic.StrongName);
		}

		public void GuardKey(){}
	}
}