using System.Collections.Generic;
using System.Linq;
using test;
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
        private Transform[] players;
        private GameObject child;
        public static PhaseManager Instance;

        private void Awake() {
            list = transform.parent.GetComponentsInChildren<IChangePhase>().ToList();
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            players = GameObject.FindGameObjectsWithTag("Player").Select(i => i.transform).ToArray();
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
            PointMove();
        }

        private void LateUpdate() {
            if (NowPhase == Phase.P3D) transform.LookAt(transform.position + players[0].Find("LookTarget").right * 5);
        }

        void PointMove() {
            var avePoint = players.Aggregate(new Vector3(), (current, player) => current + player.position) / players.Length;
            transform.position = avePoint + transform.forward * (Mathf.Clamp(Vector3.Distance(players[0].position, players[1].position), 5f, 1000f)) + new Vector3(0, 2, 0);
            child.transform.position = new Vector3(avePoint.x, 1, avePoint.z);
        }
    }
}
