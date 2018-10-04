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

		private AttackBox currentAttack;
		
		private float verticalMovement;
		private float horizontalMovement;
		[SerializeField]private bool attackRigor;
		
		private Rigidbody rigid;
		private Transform lookTarget;

		private BoxContainer boxContainer;
		private PlayerAnimControll playerAnimControll;


		private bool inAttack;
    
		private void Start() {
			rigid = GetComponent<Rigidbody>();
			lookTarget = transform.Find("LookTarget");

			boxContainer = this.GetComponent<BoxContainer>();
			playerAnimControll = this.transform.GetComponentInChildren<PlayerAnimControll>();
			
			playerAnimControll.ResponseStream.Subscribe(RecieveResponce);
		}

		// Update is called once per frame
		void Update() {
        
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
			playerAnimControll.ChangeAnim(boxContainer.FindAnim(playerAnimControll.myDic.RunName));
		}

		private void Stop() {
			playerAnimControll.ChangeAnim(boxContainer.FindAnim(playerAnimControll.myDic.WaitName));
			rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
			rigid.angularVelocity = Vector3.zero;
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

		public void JumpKey(){}

		public void RangeAtKey(){
		}

		public void WeakAtKey(){
			Attack(playerAnimControll.myDic.WeakName);
		}

		public void StrongAtKey(){}

		public void GuardKey(){}
	}
}