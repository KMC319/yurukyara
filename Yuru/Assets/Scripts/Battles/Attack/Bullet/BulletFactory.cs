using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battles.Systems;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    public abstract class BulletFactory : AttackToolEntity, IPauseObserver {
        [Serializable]
        protected struct BulletInfo {
            public Bullet Bullet;
            public Vector3 SpawnLocalPos;
            public Vector3 SpawnLocalRota;
            public float Delay;
        }

        [SerializeField] protected BulletInfo[] Bullets;

        protected List<GameObject> currentBurret = new List<GameObject>();

        protected bool isActive;

        private readonly List<IEnumerator> shotProceses = new List<IEnumerator>();

        public override void On() {
            currentBurret.Clear();

            isActive = true;
            foreach (var t in Bullets) {
                var coroutine = Shot(t);
                shotProceses.Add(coroutine);
                StartCoroutine(coroutine);
            }
        }

        public override void Off(bool cancel = false){
            isActive = false;
            if (cancel) {
                foreach (var item in currentBurret) {
                    if(item==null)continue;
                    Destroy(item);
                }
                currentBurret.Clear();
            }
        }

        protected abstract void Create(BulletInfo t);

        public void Hit(Collider other) {
            if (isActive) hitStream.OnNext(other.gameObject);
        }

        protected void RegisterBullet(GameObject bullet) {
            currentBurret.Add(bullet);
            bullet.transform.SetParent(My.transform.parent);
        }


        private IEnumerator Shot(BulletInfo t) {
            var time = 0f;
            var num = 0;
            while (time < t.Delay) {
                yield return null;
                time += Time.deltaTime;
            }
            Create(t);
        }

        public virtual void Pause() {
            foreach (var s in shotProceses) {
                StopCoroutine(s);
            }

            foreach (var item in currentBurret) {
                if(item != null) item.GetComponent<Bullet>().Pause();
            }
        }

        public virtual void Resume() {
            foreach (var s in shotProceses) {
                StartCoroutine(s);
            }

            foreach (var item in currentBurret) {
                if(item != null) item.GetComponent<Bullet>().Resume();
            }
        }
    }
}
