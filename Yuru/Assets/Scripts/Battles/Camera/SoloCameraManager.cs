using System.Collections;
using System.Linq;
using Battles.Systems;
using Cinemachine;
using UnityEngine;


namespace Battles.Camera {
    public class SoloCameraManager : CameraManagerBase {
        public override void ChangePhase(Phase changedPhase) {
            if(!Initialized) return;
            switch (changedPhase) {
                case Phase.P2D:
                    vcams[1].SetActive(false);
                    break;
                case Phase.P3D:
                    vcams[1].SetActive(true);
                    break;
            }
        }

        public override void Init(UnityEngine.Camera[] cams) {
            cameras[0] = cams[0];
            var v = Object.FindObjectsOfType<CinemachineVirtualCamera>();
            vcams[0] = v.First(i => i.gameObject.name == "vcam 2d").gameObject;
            vcams[1] = v.First(i => i.gameObject.name == "vcam P1 3d").gameObject;
            Initialized = true;
        }
    }
}
