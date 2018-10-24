using System;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    public class BeamFactory : BulletFactory{
        protected override void Create() {
            foreach (var t in Bullets) {
                if (t.Bullet.GetComponent<Beam>() != null) {
                    Observable.Timer(TimeSpan.FromSeconds(t.Delay))
                        .Subscribe(n => {
                            var a = Instantiate(t.Bullet, transform.position + transform.forward * t.SpawnLocalPos.z + transform.right * t.SpawnLocalPos.x + transform.up * t.SpawnLocalPos.y, Quaternion.Euler(transform.rotation.eulerAngles + t.SpawnLocalRota));
                            a.GetComponent<Beam>().Setup(this, Target.transform.position + new Vector3(0, 1, 0));
                            currentBurret.Add(a.gameObject);
                        });
                }
            }
        }
    }
}
