using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battles.Systems;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    public abstract class BulletFactory : AttackTool{
        [Serializable]
        protected struct BulletInfo {
            public Bullet Bullet;
            public Vector3 SpawnLocalPos;
            public Vector3 SpawnLocalRota;
            public float Delay;
        }

        [SerializeField] protected BulletInfo[] Bullets;

        protected List<GameObject> currentBurret=new List<GameObject>();

        private bool isActive;

        private readonly List<IEnumerator> shotProceses=new List<IEnumerator>();
        
        public override void On(){
            isActive = true;
            foreach (var t in Bullets) {
                StartCoroutine(Shot(t));
            }
        }

        public override void Off(){
            isActive = false;
            foreach (var item in currentBurret){
                Destroy(item);
            }
            currentBurret.Clear();
        }

        protected abstract void Create(BulletInfo t);

        public void Hit(Collider other) {
           if(isActive) hitStream.OnNext(other.gameObject);
        }

        protected void RegisterBullet(GameObject bullet){
            currentBurret.Add(bullet);
            bullet.transform.SetParent(My.transform.parent);
        }
        
                
        private IEnumerator Shot(BulletInfo t){
            yield return new WaitForSeconds(t.Delay);
            Create(t);
        }

    }
}
