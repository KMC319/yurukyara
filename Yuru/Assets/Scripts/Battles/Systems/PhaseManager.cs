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
        private int basePlayerNum;

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
            if (NowPhase == Phase.P3D) UpdateBasePoint();
            else if (Vector3.SqrMagnitude(XZVector3(players[0].position) - XZVector3(players[1].position)) > 1) UpdateBasePoint(basePlayerNum);
            PointMove();
        }

        private void LateUpdate() {
            int rightPlayer = Vector3.SqrMagnitude(players[0].position - basePoint) > Vector3.SqrMagnitude(players[1].position - basePoint) ? 0 : 1;
            if (NowPhase == Phase.P3D || Vector3.SqrMagnitude(XZVector3(players[0].position) - XZVector3(players[1].position)) > 1 || Mathf.Abs(players[0].position.y - players[1].position.y) < 0.5f) {
                transform.LookAt(transform.position + Vector3.Cross(Vector3.up, XZVector3(players[rightPlayer].position) + new Vector3(0, 1, 0) - child.transform.position));
            }
        }

        void PointMove() {
            var avePoint = players.Aggregate(new Vector3(), (current, player) => current + player.position) / players.Length;
            transform.position = avePoint + transform.forward * (Mathf.Clamp(Vector3.Distance(players[0].position, players[1].position), 5f, 1000f)) + new Vector3(0, 2, 0);
            child.transform.position = new Vector3(avePoint.x, 1, avePoint.z);
        }

        void UpdateBasePoint(int _base = 1) {
            basePlayerNum = _base;
            var target = _base == 1 ? 0 : 1;
            if (NowPhase == Phase.P2D && Vector3.SqrMagnitude(XZVector3(players[target].position) - basePoint) > Vector3.SqrMagnitude(XZVector3(players[_base].position) - basePoint)) {
                _base = target;
                target = basePlayerNum;
                basePlayerNum = _base;
            }

            basePoint = XZVector3(players[_base].position) + XZVector3(players[target].position - players[_base].position).normalized * 50f;
        }

        Vector3 XZVector3(Vector3 vector3) {
            return new Vector3(vector3.x, 0, vector3.z);
        }
    }
}
