using UnityEngine;

namespace Systems {
    public enum Mode {
        VS_PLAYER, VS_CPU
    }
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour {
        public static GameManager Instance {
            get {
                if (Instance == null) {
                    GameObject o = new GameObject("GameManager");
                    Instance = o.AddComponent<GameManager>();
                }

                return Instance;
            }
            private set { Instance = value; }
        }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        public Mode GameMode;
    }
}
