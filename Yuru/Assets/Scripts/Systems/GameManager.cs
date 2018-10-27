using UnityEngine;

namespace Systems {
    public class GameManager : MonoBehaviour {
        private static GameManager instance;
        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        }
    }
}
