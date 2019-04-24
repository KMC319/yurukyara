using UnityEngine;

namespace Battles.Effects {
    public class HitEffectFactory : MonoBehaviour {
        public static HitEffectFactory Instance { get; private set; }
        [SerializeField] private GameObject defaultEffect;

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
        }

        public void InstantiateEffect(GameObject obj,Vector3 pos,Quaternion rota) {
            if (obj == null) obj = defaultEffect;
            Destroy(Instantiate(obj, pos, rota), 1f);
        }
    }
}
