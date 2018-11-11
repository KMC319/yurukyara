using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    public class ReleasableBulletFactory : BulletFactory {
        [SerializeField] private float releaseTime;

        private IEnumerator myProcess;

        protected override void Create(BulletInfo t) {
            if (!isActive) return;
            var a = Instantiate(t.Bullet, transform.position + transform.forward * t.SpawnLocalPos.z + transform.right * t.SpawnLocalPos.x + transform.up * t.SpawnLocalPos.y, Quaternion.Euler(transform.rotation.eulerAngles + t.SpawnLocalRota));
            RegisterBullet(a.gameObject);
            a.GetComponent<ReleasableBullet>().SetParent(gameObject);
            myProcess = Hold(a);
            StartCoroutine(myProcess);
        }

        IEnumerator Hold(Bullet bullet) {
            var time = 0f;
            while (time < releaseTime) {
                yield return null;
                time += Time.deltaTime;
            }
            if (!isActive) yield break;
            bullet.gameObject.transform.parent = null;
            bullet.GetComponent<Bullet>().Setup(this, Target.gameObject);
        }

        public override void Pause() {
            base.Pause();
            if(myProcess != null) StopCoroutine(myProcess);
        }

        public override void Resume() {
            base.Resume();
            if(myProcess != null) StartCoroutine(myProcess);
        }
    }
}


