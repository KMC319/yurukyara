using System.Collections;
using System.Linq;
using Battles.Systems;
using Cinemachine;
using UnityEngine;

namespace Battles.Camera {
    public class VSPlayerCameraManager : CameraManagerBase{
        public override void ChangePhase(Phase changedPhase) {
            switch (changedPhase) {
                case Phase.P2D:
                    vcams[1].SetActive(false);
                    vcams[2].SetActive(false);
                    switch (LastAttackPlayerNum) {
                        case 0:
                            CoroutineHandler.StartStaticCoroutine(LerpCamView(0, new Rect(0, 0, 1, 1), 0.25f));
                            CoroutineHandler.StartStaticCoroutine(LerpCamView(1, new Rect(1, 0, 0, 1), 0.25f));
                            break;
                        case 1:
                            CoroutineHandler.StartStaticCoroutine(LerpCamView(0, new Rect(0, 0, 0, 1), 0.25f));
                            CoroutineHandler.StartStaticCoroutine(LerpCamView(1, new Rect(0, 0, 1, 1), 0.25f));
                            break;
                    }

                    break;
                case Phase.P3D:
                    vcams[1].SetActive(true);
                    vcams[2].SetActive(true);
                    CoroutineHandler.StartStaticCoroutine(LerpCamView(0, new Rect(0, 0, 0.5f, 1), 0.25f));
                    CoroutineHandler.StartStaticCoroutine(LerpCamView(1, new Rect(0.5f, 0, 0.5f, 1), 0.25f));
                    break;
            }
        }

        public override void Init() {
            var cams = Object.FindObjectsOfType<UnityEngine.Camera>();
            cameras[0] = cams.First(i => i.name == "P1cam").GetComponent<UnityEngine.Camera>();
            cameras[1] = cams.First(i => i.name == "P2cam").GetComponent<UnityEngine.Camera>();
            var v = Object.FindObjectsOfType<CinemachineVirtualCamera>();
            vcams[0] = v.First(i => i.gameObject.name == "vcam 2d").gameObject;
            vcams[1] = v.First(i => i.gameObject.name == "vcam P1 3d").gameObject;
            vcams[2] = v.First(i => i.gameObject.name == "vcam P2 3d").gameObject;
        }
        
        IEnumerator LerpCamView(int camNum, Rect endRect, float time) {
            var startRect = cameras[camNum].rect;
            var a = 0f;
            while (a + Time.deltaTime < time) {
                cameras[camNum].rect = new Rect(Mathf.Lerp(startRect.x, endRect.x, a / time), Mathf.Lerp(startRect.y, endRect.y, a / time), Mathf.Lerp(startRect.width, endRect.width, a / time), Mathf.Lerp(startRect.height, endRect.height, a / time));
                a += Time.deltaTime;
                yield return null;
            }

            cameras[camNum].rect = endRect;
        }
    }
}
