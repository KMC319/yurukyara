using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour {
    [SerializeField] private Camera[] cameras;
    private PhaseManager phaseManager;

    // Use this for initialization
    void Start() {
        phaseManager = GameObject.Find("TestPhaseManager").GetComponent<PhaseManager>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            ChangePhase(0);
        }

        if (Input.GetKeyDown(KeyCode.O)) {
            ChangePhase(1);
        }
    }

    void ChangePhase(int playerNum) {
        switch (phaseManager.NowPhase) {
            case Phase.P3D:
                switch (playerNum) {
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
            case Phase.P2D:
                StartCoroutine(LerpCamView(0, new Rect(0, 0, 0.5f, 1), 0.25f));
                StartCoroutine(LerpCamView(1, new Rect(0.5f, 0, 0.5f, 1), 0.25f));
                break;
        }
    }

    IEnumerator LerpCamView(int camNum, Rect endRect, float time) {
        var startRect = cameras[camNum].rect;
        var a = 0f;
        while (a < time) {
            cameras[camNum].rect = new Rect(Mathf.Lerp(startRect.x, endRect.x, a / time), Mathf.Lerp(startRect.y, endRect.y, a / time), Mathf.Lerp(startRect.width, endRect.width, a / time), Mathf.Lerp(startRect.height, endRect.height, a / time));
            a += Time.deltaTime;
            yield return null;
        }

        cameras[camNum].rect = endRect;
    }
}
