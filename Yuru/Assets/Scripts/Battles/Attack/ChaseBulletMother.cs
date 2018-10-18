using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    public class ChaseBulletMother : Bullet {
        [Serializable]
        struct ChildInfo {
            public GameObject Bullet;
            public Vector3 SpawnLocalPos;
            public Vector3 SpawnLocalRota;
            public float Delay;
        }

        [SerializeField] private ChildInfo[] bullets;
        private GameObject target;
        public Vector3 MyPos;

        private void Start() {
            target = GameObject.FindGameObjectsWithTag("Player").OrderBy(i => Vector3.Distance(i.transform.position, MyPos)).Last();
            foreach (var t in bullets) {
                if (t.Bullet.GetComponent<ChaseBullet>() != null) {
                    Observable.Timer(TimeSpan.FromSeconds(t.Delay))
                        .Subscribe(n => {
                            var a = Instantiate(t.Bullet, transform.position + transform.forward * t.SpawnLocalPos.z + transform.right * t.SpawnLocalPos.x + transform.up * t.SpawnLocalPos.y, Quaternion.Euler(transform.rotation.eulerAngles + t.SpawnLocalRota));
                            a.GetComponent<ChaseBullet>().Setup(this, target);
                        });
                }
            }

            Destroy(gameObject, 6f);
        }

        public void Hit(Collider other) {
            hitStream.OnNext(other);
        }
    }
}
