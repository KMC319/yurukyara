using UnityEngine;

namespace Battles.Attack {
    public class Rotation : MonoBehaviour {
        [SerializeField] private Vector3 rota;
        private Bullet bullet;

        private void Start() {
            bullet = GetComponentInParent<Bullet>();
        }

        private void Update() {
            if (!bullet.Initialized) return;
            transform.Rotate(rota * Time.deltaTime);
        }
    }
}
