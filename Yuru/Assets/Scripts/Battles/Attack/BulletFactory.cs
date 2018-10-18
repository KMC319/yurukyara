using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    public abstract class BulletFactory : Bullet {
        [Serializable]
        protected struct BulletInfo {
            public Bullet Bullet;
            public Vector3 SpawnLocalPos;
            public Vector3 SpawnLocalRota;
            public float Delay;
        }

        [SerializeField] protected BulletInfo[] Bullets;
        protected GameObject Target;
        public Vector3 MyPos;

        protected void Start() {
            Target = GameObject.FindGameObjectsWithTag("Player").OrderBy(i => Vector3.Distance(i.transform.position, MyPos)).Last();
            Create();
            Destroy(gameObject, 6f);
        }

        protected virtual void Create() { }

        public void Hit(Collider other) {
            hitStream.OnNext(other);
        }
    }
}
