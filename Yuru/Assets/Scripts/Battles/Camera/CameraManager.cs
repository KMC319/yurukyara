using System.Collections;
using Battles.Systems;
using UnityEngine;

namespace Battles.Camera {
    public class CameraManager : MonoBehaviour, IChangePhase {
        [SerializeField] private UnityEngine.Camera[] cameras;
        [SerializeField] private GameObject[] vcams;
        private int lastAttackPlayerNum;

        // Use this for initialization
        void Start() { }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.P)) {
                lastAttackPlayerNum = 0;
                PhaseManager.Instance.NowPhase = (Phase) ((int) (PhaseManager.Instance.NowPhase + 1) % 2);
            }

            if (Input.GetKeyDown(KeyCode.O)) {
                lastAttackPlayerNum = 1;
                PhaseManager.Instance.NowPhase = (Phase) ((int) (PhaseManager.Instance.NowPhase + 1) % 2);
            }
        }

        public void ChangePhase(Phase phase) {
            switch (phase) {
                case Phase.P2D:
                    vcams[1].SetActive(false);
                    vcams[2].SetActive(false);
                    switch (lastAttackPlayerNum) {
                        case 0:
                            StartCoroutine(LerpCamView(0, new Rect(0, 0, 1, 1), 0.25f));
                            StartCoroutine(LerpCamView(1, new Rect(1, 0, 0, 1), 0.25f));
                            break;
                        case 1:
                            StartCoroutine(LerpCamView(0, new Rect(0, 0, 0, 1), 0.25f));
                            StartCoroutine(LerpCamView(1, new Rect(0, 0, 1, 1), 0.25f));
                            break;
                    }

                    break;
                case Phase.P3D:
                    vcams[1].SetActive(true);
                    vcams[2].SetActive(true);
                    StartCoroutine(LerpCamView(0, new Rect(0, 0, 0.5f, 1), 0.25f));
                    StartCoroutine(LerpCamView(1, new Rect(0.5f, 0, 0.5f, 1), 0.25f));
                    break;
            }
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
