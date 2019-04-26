using Battles.Players;
using Battles.Systems;
using UniRx;

namespace Tutorial {
    public class TutorialHealthManagersControl : HealthManagersControll {
        protected override void Start() {
            var p1 = transform.parent.GetComponentInChildren<FirstPlayerBinder>().PlayerRoot;
            var hoo=new HealthManager(charHealth,stackAbleHeatlh,gaugeControlls[0]);
            healthManagers[0] = hoo;
            hoo.DeadStream.Subscribe(n => deadPlayerStream.OnNext(p1));
            p1.DamageControll.DamageStream.Subscribe(n => hoo.DamageRecieve(n));

            var p2 = transform.parent.GetComponentInChildren<SecondPlayerBinder>().PlayerRoot;
            var foo=new HealthManager(charHealth,stackAbleHeatlh,gaugeControlls[1]);
            healthManagers[1] = foo;
            foo.DeadStream.Subscribe(n => deadPlayerStream.OnNext(p2));
            p2.DamageControll.DamageStream.Subscribe(n => {
                var temp = n;
                temp.damage = 0;
                foo.DamageRecieve(temp);
            });
        }
    }
}
