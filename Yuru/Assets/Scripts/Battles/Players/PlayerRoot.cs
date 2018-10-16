using System;
using Battles.Attack;
using Battles.Systems;
using doma.Inputs;
using UnityEngine;

namespace Battles.Players{
	public class PlayerRoot : MonoBehaviour,IBattleKeyReciever,IChangePhase{
		private bool moveAble => !attackControll.InAttack &&
		                         !guardControll.InGuard &&
		                         !DamageControll.InDamage;

		public MoveCotroll CurrentMoveCotroll { get; private set; }
		private MoveCotroll moveCotrollControll3D;
		private MoveCotroll moveCotrollControll2D;
		private AttackControll attackControll;
		private GuardControll guardControll;
		
		public DamageControll DamageControll{ get; private set; }

		private void Awake(){
			attackControll = this.GetComponent<AttackControll>();
			moveCotrollControll3D = this.GetComponent<MoveCotroll3D>();
			moveCotrollControll2D = this.GetComponent<MoveControll2D>();
			DamageControll = this.GetComponent<DamageControll>();
			guardControll = this.GetComponent<GuardControll>();
		}

		private void Start(){
			CurrentMoveCotroll = moveCotrollControll3D;
			attackControll.iMoveCotroll = CurrentMoveCotroll;
		}

		private void Update() {
			CurrentMoveCotroll.FallCheck(attackControll.InAttack||guardControll.InGuard);
		}

		private void FixedUpdate(){
			if (moveAble){
				CurrentMoveCotroll.Move();
			}
		}
		

		public void StartInputRecieve(){}

		public void EndInputRecieve(){
			CurrentMoveCotroll.HorizontalMovement = 0;
			CurrentMoveCotroll.VerticalMovement= 0;
			CurrentMoveCotroll.Stop();
		}

		public void ChangeHorizontalAxis(float delta){
			CurrentMoveCotroll.HorizontalMovement = delta;
			if (delta > 0){
				attackControll.InputKey(PlayerKeyCode.RArrow);
			}else if (delta < 0){
				attackControll.InputKey(PlayerKeyCode.LArrow);
			}
		}
		
		public void ChangeVerticalAxis(float delta){
			CurrentMoveCotroll.VerticalMovement = delta;
			if (delta > 0){
				attackControll.InputKey(PlayerKeyCode.UArrow);
			}else if (delta < 0){
				attackControll.InputKey(PlayerKeyCode.DArrow);
			}
		}

		public void JumpKey(){
			if(!attackControll.InAttack)CurrentMoveCotroll.Jump();
			attackControll.InputKey(PlayerKeyCode.A);
		}

		public void RangeAtKey(){
			attackControll.InputKey(PlayerKeyCode.B);
		}

		public void WeakAtKey(){
			attackControll.InputKey(PlayerKeyCode.X);
		}

		public void StrongAtKey(){
			attackControll.InputKey(PlayerKeyCode.Y);
		}

		public void GuardKey(){
			guardControll.GuardCommand();
			CurrentMoveCotroll.Pause();
		}
		
		public void ChangePhase(Phase changedPhase){
			switch (changedPhase){
				case Phase.P3D:
					CurrentMoveCotroll = moveCotrollControll3D;
					break;
				case Phase.P2D:
					CurrentMoveCotroll = moveCotrollControll2D;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(changedPhase), changedPhase, null);
			}

			attackControll.iMoveCotroll = CurrentMoveCotroll;
		}
	}
}