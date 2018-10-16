using doma;
using Players;
using UniRx;
using UnityEngine;

namespace Battles.Systems{
	public class HealthManagersControll : MonoBehaviour{
		[SerializeField]private float charHealth;
		[SerializeField] private float stackAbleHeatlh;
		[SerializeField]private HealthManager[] healthManagers=new HealthManager[2];
		[SerializeField]private GaugeControll[] gaugeControlls=new GaugeControll[2];

		private float stack;
		
		private void Start(){
			var p1 = transform.parent.GetComponentInChildren<PlayerFirstBinder>().PlayerRootControll;
			var hoo=new HealthManager(charHealth,stackAbleHeatlh,gaugeControlls[0]);
			healthManagers[0] = hoo;
			p1.playerDamageControll.DamageStream.Subscribe(n => hoo.DamageRecieve(n));

			var p2 = transform.parent.GetComponentInChildren<PlayerSecondBinder>().PlayerRootControll;
			var foo=new HealthManager(charHealth,stackAbleHeatlh,gaugeControlls[1]);
			healthManagers[1] = foo;
			p2.playerDamageControll.DamageStream.Subscribe(n => foo.DamageRecieve(n));
		}
	}
}