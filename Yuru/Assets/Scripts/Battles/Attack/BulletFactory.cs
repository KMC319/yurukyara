using System;
using System.Collections.Generic;
using System.Linq;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    public abstract class BulletFactory : AttackTool {
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
        
        public override void On(){
            isActive = true;
            Create();
        }

        public override void Off(){
            isActive = false;
            foreach (var item in currentBurret){
                Destroy(item);
            }
            currentBurret.Clear();
        }

        protected virtual void Create() { }

        public void Hit(Collider other) {
           if(isActive) hitStream.OnNext(other.gameObject);
        }
    }
}
