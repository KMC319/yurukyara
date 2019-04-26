using Battles.Players;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Systems{
	public class HealthManagersControll : MonoBehaviour{
		[SerializeField]protected float charHealth;
		[SerializeField] protected float stackAbleHeatlh;
		[SerializeField]protected HealthManager[] healthManagers=new HealthManager[2];
		[SerializeField]protected GaugeControll[] gaugeControlls=new GaugeControll[2];

		protected readonly Subject<PlayerRoot> deadPlayerStream=new Subject<PlayerRoot>();
		public IObservable<PlayerRoot> DeadPlayerStream => deadPlayerStream;

		private float stack;
		
		protected virtual void Start(){
			var p1 = transform.parent.GetComponentInChildren<FirstPlayerBinder>().PlayerRoot;
			var hoo=new HealthManager(charHealth,stackAbleHeatlh,gaugeControlls[0]);
			healthManagers[0] = hoo;
			hoo.DeadStream.First().Subscribe(n => deadPlayerStream.OnNext(p1));
			p1.DamageControll.DamageStream.Subscribe(n => hoo.DamageRecieve(n));

			var p2 = transform.parent.GetComponentInChildren<SecondPlayerBinder>().PlayerRoot;
			var foo=new HealthManager(charHealth,stackAbleHeatlh,gaugeControlls[1]);
			healthManagers[1] = foo;
			foo.DeadStream.First().Subscribe(n => deadPlayerStream.OnNext(p2));
			p2.DamageControll.DamageStream.Subscribe(n => foo.DamageRecieve(n));
		}

		public PlayerNum CheckMoreHealthPlayer(){
			PlayerNum res;
			if (healthManagers[0].health > healthManagers[1].health){
				res = PlayerNum.P1;
			}else if (healthManagers[0].health < healthManagers[1].health){
				res = PlayerNum.P2;
			}else{
				res = PlayerNum.None;
			}
			return res;
		}
	}
}