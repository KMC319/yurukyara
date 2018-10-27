using System.Linq;
using Battles.Attack;
using Battles.Systems;
using UnityEngine;

namespace Battles.Effects {
    public class Shield : IAttackTool {
        [SerializeField] private Transform[] parentPositions;
        [SerializeField] private Vector3 localPos;
        [SerializeField] private LookTarget lookTarget;

        private void Update() {
            SetPosition();
        }

        public override void On() {
            SetPosition();
            gameObject.SetActive(true);
        }

        public override void Off() {
            gameObject.SetActive(false);
        }

        private void SetPosition() {
            var sum = new Vector3();
            if (parentPositions.Length > 1) {
                sum = parentPositions.Select(i => i.position)
                                     .Aggregate((current, vector3) => current + vector3);
            } else {
                sum = parentPositions[0].position;
            }

            transform.position = sum / parentPositions.Length + transform.forward * localPos.z;
        }
    }
}
