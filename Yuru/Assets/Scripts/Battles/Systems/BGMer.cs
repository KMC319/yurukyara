using UnityEngine;

namespace Battles.Systems {
    //とりあえずシ－ンまたぐためだけスクリプト
    public class BGMer : MonoBehaviour {
        public static BGMer Instance;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public void Delete() {
            Destroy(gameObject);
        }
    }
}
