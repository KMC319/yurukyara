using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    public class ChaseBulletFactory : BulletFactory {
        protected override void Create() {
            foreach (var t in Bullets) {
                if (t.Bullet.GetComponent<ChaseBullet>() != null) {
                    Observable.Timer(TimeSpan.FromSeconds(t.Delay))
                        .Subscribe(n => {
                            var a = Instantiate(t.Bullet, transform.position + transform.forward * t.SpawnLocalPos.z + transform.right * t.SpawnLocalPos.x + transform.up * t.SpawnLocalPos.y, Quaternion.Euler(transform.rotation.eulerAngles + t.SpawnLocalRota));
                            a.GetComponent<ChaseBullet>().Setup(this, Target.gameObject);
                            currentBurret.Add(a.gameObject);
                        });
                }
            }
        }
    }
}
