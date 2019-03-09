using Battles.Players;
using Battles.Systems;
using UniRx;
using UnityEngine;

namespace Tutorial {
    public class TutorialFlgCollider : MonoBehaviour {
        private Subject<Unit> hitStream = new Subject<Unit>();
        public IObservable<Unit> HitStream => hitStream;

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<IPlayerBinder>() == null) return;
            if (other.GetComponent<IPlayerBinder>().PlayerNum == PlayerNum.P1) {
                hitStream.OnNext(Unit.Default);
                Destroy(gameObject);
            }
        }
    }
}
