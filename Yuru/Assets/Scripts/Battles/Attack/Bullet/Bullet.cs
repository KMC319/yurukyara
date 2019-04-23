using Battles.Systems;
using UniRx;
using UnityEngine;

namespace Battles.Attack {
    public abstract class Bullet : MonoBehaviour ,IPauseObserver{
        protected readonly Subject<Collider> hitStream = new Subject<Collider>();
        public IObservable<Collider> HitStream => hitStream;

        public bool Initialized;

        public abstract void Setup(BulletFactory mom, GameObject targetObj);

        private void OnTriggerEnter(Collider other) {
            hitStream.OnNext(other);
        }

        public abstract void Pause();
        public abstract void Resume();
    }
}
