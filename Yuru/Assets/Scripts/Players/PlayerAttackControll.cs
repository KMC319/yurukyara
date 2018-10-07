using System;
using System.Linq;
using System.Security.Cryptography;
using Animations;
using Battles.Attack;
using Battles.Health;
using UniRx;
using UnityEngine;

namespace Players{
	public class PlayerAttackControll : MonoBehaviour{
		private AttackBox currentAttack;
		private string currentRootAttackName;
		private PlayerAnimControll playerAnimControll;
		private PlayerRootControll taregtPlayer;
		
		private bool attackRigor;
		public bool InAttack{ get; private set; }
		private bool hitEnable;

		private void Start(){
			playerAnimControll = this.transform.GetComponentInChildren<PlayerAnimControll>();
			taregtPlayer = this.GetComponent<IPlayerBinder>().TargetPlayerRootControll;
			
			playerAnimControll.ResponseStream.Subscribe(RecieveResponce);

			Observable.Merge(transform.GetComponentsInChildren<AttackCollider>().Select(n => n.HitStream))
				.Subscribe(RecieveHit);
		}

		private void RecieveHit(Collider collider){
			if(!(InAttack&&hitEnable))return;
			if(collider.gameObject!=taregtPlayer.gameObject)return;
			taregtPlayer.playerDamageControll.Hit(currentAttack.attackDamageBox);
			hitEnable = false;
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
			if (responce == AnimResponce.AttackEnd){
				currentAttack = null;
				InAttack = false;
				hitEnable = false;
				currentRootAttackName="";
			}
		}


		public void StrongAttack(){
			Attack(playerAnimControll.MyDic.StrongName);
		}

		private void Attack(string name){
			if (attackRigor) return;
			InAttack = true;
			attackRigor = true;
			if (currentRootAttackName!=name){
				currentRootAttackName = name;
				currentAttack = (AttackBox)playerAnimControll.ChangeAnim(name);
			}else if (currentAttack.nextAttack != null && 
				    currentAttack.nextAttack.Length==1&&
				    currentAttack.nextAttack[0].clip != null){	
					currentAttack = currentAttack.nextAttack[0];
					playerAnimControll.ChangeAnim(currentAttack);
			}
			Observable.Timer(TimeSpan.FromSeconds(currentAttack.rigorTime))
				.Subscribe(_ => {
					attackRigor = false;
				});
			Observable.Timer(TimeSpan.FromSeconds(currentAttack.enableTime))
				.Subscribe(_ => { hitEnable = true; });
		}
	}
}