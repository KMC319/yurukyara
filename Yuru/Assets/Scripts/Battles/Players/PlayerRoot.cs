using System;
using Systems.Chars;
using Battles.Attack;
using Battles.Systems;
using doma.Inputs;
using UnityEngine;

namespace Battles.Players{
	[RequireComponent(typeof(MoveCotroll3D))]
	[RequireComponent(typeof(MoveControll2D))]
	[RequireComponent(typeof(AttackControll))]
	[RequireComponent(typeof(DamageControll))]
	[RequireComponent(typeof(GuardControll))]
	[RequireComponent(typeof(CharTag))]
	public class PlayerRoot : MonoBehaviour,IBattleKeyReciever,IChangePhase{
		private bool moveAble => !AttackControll.InAttack &&
		                         !guardControll.InGuard &&
		                         !DamageControll.InDamage;

		public MoveCotroll CurrentMoveCotroll { get; private set; }
		private MoveCotroll moveCotrollControll3D;
		private MoveCotroll moveCotrollControll2D;
		public AttackControll AttackControll{ get; private set; }
		private GuardControll guardControll;
		
		public DamageControll DamageControll{ get; private set; }

		private void Awake(){
			AttackControll = this.GetComponent<AttackControll>();
			moveCotrollControll3D = this.GetComponent<MoveCotroll3D>();
			moveCotrollControll2D = this.GetComponent<MoveControll2D>();
			DamageControll = this.GetComponent<DamageControll>();
			guardControll = this.GetComponent<GuardControll>();
		}

		private void Start(){
			CurrentMoveCotroll = moveCotrollControll3D;
			AttackControll.iMoveCotroll = CurrentMoveCotroll;
		}

		private void Update() {
			CurrentMoveCotroll.FallCheck(AttackControll.InAttack||guardControll.InGuard);
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
				AttackControll.InputKey(PlayerKeyCode.RArrow);
			}else if (delta < 0){
				AttackControll.InputKey(PlayerKeyCode.LArrow);
			}
		}
		
		public void ChangeVerticalAxis(float delta){
			CurrentMoveCotroll.VerticalMovement = delta;
			if (delta > 0){
				AttackControll.InputKey(PlayerKeyCode.UArrow);
			}else if (delta < 0){
				AttackControll.InputKey(PlayerKeyCode.DArrow);
			}
		}

		public void JumpKey(){
			if(!AttackControll.InAttack)CurrentMoveCotroll.JumpStart();
			AttackControll.InputKey(PlayerKeyCode.A);
		}

		public void RangeAtKey(){
			AttackControll.InputKey(PlayerKeyCode.B);
		}

		public void WeakAtKey(){
			AttackControll.InputKey(PlayerKeyCode.X);
		}

		public void StrongAtKey(){
			AttackControll.InputKey(PlayerKeyCode.Y);
		}

		public void GuardKey(){
			if(AttackControll.InAttack)return;
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

			AttackControll.iMoveCotroll = CurrentMoveCotroll;
		}
	}
}