using UnityEngine;

namespace Battles.Attack {
    public class Rotation : MonoBehaviour {
        [SerializeField] private Vector3 rota;
        private void Update() {
            transform.Rotate(rota * Time.deltaTime);
        }
    }
}
