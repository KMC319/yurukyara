using Battles.Systems;
using UnityEngine;
using Zenject;

namespace Battles.Camera {
    public class CameraMaster : MonoBehaviour ,IChangePhase{
        [Inject] private CameraManagerBase cameraManager;

        private void Start() {
            cameraManager.Init();
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
