using System;
using System.Security.Cryptography;
using Animations;
using UniRx;
using UnityEngine;

namespace Players{
	public class PlayerAttackControll : MonoBehaviour{
		private AttackBox currentAttack;
		private PlayerAnimControll playerAnimControll;
		
		private bool attackRigor;
		public bool InAttack{ get; private set; }

		private void Start(){
			playerAnimControll = this.transform.GetComponentInChildren<PlayerAnimControll>();

			playerAnimControll.ResponseStream.Subscribe(RecieveResponce);
		}

		public void WeakAttack(bool in_jumping){
			if (in_jumping){
				Attack(playerAnimControll.MyDic.JumpAtName);
			}
			else{
				Attack(playerAnimControll.MyDic.WeakName);
			}
		}
		
		private void RecieveResponce(AnimResponce responce){
			switch (responce){
				case AnimResponce.Wait:
					break;
				case AnimResponce.AttackEnd:
					currentAttack = null;
					InAttack = false;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(responce), responce, null);
			}
		}


		public void StrongAttack(){
			Attack(playerAnimControll.MyDic.StrongName);
		}

		private void Attack(string name){
			if (attackRigor) return;
			InAttack = true;
			attackRigor = true;
			if (currentAttack == null){
				currentAttack = (AttackBox)playerAnimControll.ChangeAnim(name);
			}else{
				if (currentAttack.nextAttack != null && 
				    currentAttack.nextAttack.Length==1&&
				    currentAttack.nextAttack[0].clip != null){	
					currentAttack = currentAttack.nextAttack[0];
					playerAnimControll.ChangeAnim(currentAttack);
				}	
			}
			Observable.Timer(TimeSpan.FromSeconds(currentAttack.rigorTime))
				.Subscribe(_ => {
					attackRigor = false;
				});
		}
	}
}