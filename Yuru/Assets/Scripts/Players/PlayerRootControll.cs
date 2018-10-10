using System;
using Battles.Systems;
using doma.Inputs;
using UnityEngine;
using UniRx;

namespace Players{
	public class PlayerRootControll : MonoBehaviour,IBattleKeyReciever,IChangePhase{
		private bool moveAble => !playerAttackControll.InAttack &&
		                         !playerGuardControll.InGuard &&
		                         !playerDamageControll.InDamage;

		public IPlayerMove currentPlayerMove { get; private set; }
		private IPlayerMove playerMoveControll3D;
		private IPlayerMove playerMoveControll2D;
		private PlayerAttackControll playerAttackControll;
		private PlayerGuardControll playerGuardControll;
		
		public PlayerDamageControll playerDamageControll{ get; private set; }

		private void Awake(){
			playerAttackControll = this.GetComponent<PlayerAttackControll>();
			playerMoveControll3D = this.GetComponent<PlayerMoveControll3D>();
			playerMoveControll2D = this.GetComponent<PlayerMoveControll2D>();
			playerDamageControll = this.GetComponent<PlayerDamageControll>();
			playerGuardControll = this.GetComponent<PlayerGuardControll>();
		}

		private void Start(){
			currentPlayerMove = playerMoveControll3D;
			playerAttackControll.iPlayerMove = currentPlayerMove;
		}

		private void Update() {
			currentPlayerMove.FallCheck(playerAttackControll.InAttack);
		}

		private void FixedUpdate(){
			if (moveAble){
				currentPlayerMove.Move();
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
			if (delta > 0){
				playerAttackControll.InputKey(PlayerKeyCode.RArrow);
			}else if (delta < 0){
				playerAttackControll.InputKey(PlayerKeyCode.LArrow);
			}
		}
		
		public void ChangeVerticalAxis(float delta){
			currentPlayerMove.VerticalMovement = delta;
			if (delta > 0){
				playerAttackControll.InputKey(PlayerKeyCode.UArrow);
			}else if (delta < 0){
				playerAttackControll.InputKey(PlayerKeyCode.DArrow);
			}
		}

		public void JumpKey(){
			if(!playerAttackControll.InAttack)currentPlayerMove.Jump();
			playerAttackControll.InputKey(PlayerKeyCode.A);
		}

		public void RangeAtKey(){
			playerAttackControll.InputKey(PlayerKeyCode.B);
		}

		public void WeakAtKey(){
			playerAttackControll.InputKey(PlayerKeyCode.X);
		}

		public void StrongAtKey(){
			playerAttackControll.InputKey(PlayerKeyCode.Y);
		}

		public void GuardKey(){
			playerGuardControll.GuardCommand();
		}
		
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

			playerAttackControll.iPlayerMove = currentPlayerMove;
		}
	}
}