using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    public class ChaseBulletFactory : BulletFactory {
        protected override void Create(BulletInfo t) {
            if(!(t.Bullet is ChaseBullet))return;
            if(!isActive) return;
            var a = Instantiate(t.Bullet, transform.position + transform.forward * t.SpawnLocalPos.z + transform.right * t.SpawnLocalPos.x + transform.up * t.SpawnLocalPos.y, Quaternion.Euler(transform.rotation.eulerAngles + t.SpawnLocalRota));
            a.GetComponent<ChaseBullet>().Setup(this, Target.gameObject);
            RegisterBullet(a.gameObject);
        }
    }
}
