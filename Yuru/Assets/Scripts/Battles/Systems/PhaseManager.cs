using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Battles.Systems {
    public enum Phase {
        P3D, P2D
    }

    [DefaultExecutionOrder(-10)]
    public class PhaseManager : MonoBehaviour {
        private List<IChangePhase> list;
        public Phase NowPhase;
        private Vector3 basePoint;
        private Transform[] players;
        private GameObject child;
        public static PhaseManager Instance;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            players = GameObject.FindGameObjectsWithTag("Player").Select(i => i.transform).ToArray();
            list = transform.parent.GetComponentsInChildren<IChangePhase>().ToList();
            var key = players.Select(i => i.transform.position.z).ToArray();
            Array.Sort(key, players); 
            NowPhase = Phase.P3D;
            child = transform.Find("child").gameObject;
            Observable.EveryUpdate()
                .FirstOrDefault()
                .Subscribe(n => this.ObserveEveryValueChanged(x => x.NowPhase)
                    .Subscribe(_ => {
                        foreach (var item in list) {
                            item.ChangePhase(NowPhase);
                        }
                    }));
        }

        private void Update() {
            if(NowPhase == Phase.P3D) UpdateBasePoint();
            PointMove();
        }

        private void LateUpdate() {
            transform.LookAt(transform.position + Vector3.Cross(Vector3.up, new Vector3(players[1].transform.position.x, 1, players[1].transform.position.z) - child.transform.position));
        }

        void PointMove() {
            var avePoint = players.Aggregate(new Vector3(), (current, player) => current + player.position) / players.Length;
            transform.position = avePoint + transform.forward * (Mathf.Clamp(Vector3.Distance(players[0].position, players[1].position), 5f, 1000f)) + new Vector3(0, 2, 0);
            child.transform.position = new Vector3(avePoint.x, 1, avePoint.z);
        }

        void UpdateBasePoint() {
            basePoint = players[1].position + (players[0].position - players[1].position).normalized * 50f;
        }
    }
}
