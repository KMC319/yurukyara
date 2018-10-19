using System;
using Systems;
using Battles.Systems;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace Battles.Camera {
    public class CameraMaster : MonoBehaviour, IChangePhase {
        [SerializeField] private UnityEngine.Camera[] cameras;
        private CameraManagerBase cameraManager;

        private void Awake() {
            var temp = GameStateManager.instance;
            if (temp != null) {
                Lunch(GameStateManager.instance.mode);
            } else {
                Lunch();
            }
        }

        private void Lunch(ModeName mode = ModeName.Multi) {
            var cams = new UnityEngine.Camera[2];
            switch (mode) {
                case ModeName.Arcade:
                case ModeName.VsCom:
                case ModeName.Practice:
                    cameraManager = new SoloCameraManager();
                    cams[0] = Instantiate(cameras[0]);
                    cams[0].transform.parent = transform;
                    cams[0].rect = new Rect(0, 0, 1, 1);
                    break;
                case ModeName.Multi:
                    cameraManager = new VSPlayerCameraManager();
                    cams[0] = Instantiate(cameras[0]);
                    cams[0].transform.parent = transform;
                    cams[1] = Instantiate(cameras[1]);
                    cams[1].transform.parent = transform;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            cameraManager.Init(cams);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.P)) {
                cameraManager.LastAttackPlayerNum = 0;
                PhaseManager.Instance.NowPhase = (Phase) ((int) (PhaseManager.Instance.NowPhase + 1) % 2);
            }

            if (Input.GetKeyDown(KeyCode.O)) {
                cameraManager.LastAttackPlayerNum = 1;
                PhaseManager.Instance.NowPhase = (Phase) ((int) (PhaseManager.Instance.NowPhase + 1) % 2);
            }
        }

        public void ChangePhase(Phase changedPhase) {
            cameraManager.ChangePhase(changedPhase);
        }
    }
}
