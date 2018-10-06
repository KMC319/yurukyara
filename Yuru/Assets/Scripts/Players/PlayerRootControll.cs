using System;
using Battles.Systems;
using doma.Inputs;
using UnityEngine;
using UniRx;

namespace Players{
	public class PlayerRootControll : MonoBehaviour,IBattleKeyReciever,IChangePhase{

		private IPlayerMove currentPlayerMove;
		private IPlayerMove playerMoveControll3D;
		private IPlayerMove playerMoveControll2D;
		private PlayerAttackControll playerAttackControll;
		
		
	
		private void Start(){
			playerAttackControll = this.GetComponent<PlayerAttackControll>();
			playerMoveControll3D = this.GetComponent<PlayerMoveControll3D>();
			playerMoveControll2D = this.GetComponent<PlayerMoveControll2D>();
			
			
			currentPlayerMove = playerMoveControll3D;
		}

		private void Update() {
			currentPlayerMove.FallCheck(playerAttackControll.InAttack);
		}

		private void FixedUpdate(){
			if (!playerAttackControll.InAttack){
				currentPlayerMove.Move();
			}else{
				currentPlayerMove.Cancel();
			}
		}
		

		public void StartInputRecieve(){}

		public void EndInputRecieve(){
			currentPlayerMove.HorizontalMovement = 0;
			currentPlayerMove.VerticalMovement= 0;
			currentPlayerMove.Stop();
		}

		public void ChangeHorizontalAxis(float delta){
			currentPlayerMove.HorizontalMovement = delta;
		}
		public void ChangeVerticalAxis(float delta){
			currentPlayerMove.VerticalMovement = delta;
		}

		public void JumpKey(){
			currentPlayerMove.Jump();
		}

		public void RangeAtKey(){
		}

		public void WeakAtKey(){
			playerAttackControll.WeakAttack(currentPlayerMove.InJumping);
		}

		public void StrongAtKey(){
			playerAttackControll.StrongAttack();
		}

		public void GuardKey(){}
		
		public void ChangePhase(Phase changedPhase){
			switch (changedPhase){
				case Phase.P3D:
					currentPlayerMove = playerMoveControll3D;
					break;
				case Phase.P2D:
					currentPlayerMove = playerMoveControll2D;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(changedPhase), changedPhase, null);
			}
		}
	}
}