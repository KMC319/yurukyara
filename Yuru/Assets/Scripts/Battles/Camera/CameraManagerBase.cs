using Battles.Systems;
using UnityEngine;

namespace Battles.Camera {
    public abstract class CameraManagerBase : IChangePhase {
        protected UnityEngine.Camera[] cameras = new UnityEngine.Camera[2];
        protected GameObject[] vcams = new GameObject[3];
        public int LastAttackPlayerNum = 0; //1P = 0, 2P = 1
        public virtual void Init() { }
        public virtual void ChangePhase(Phase changedPhase) { }
    }
}
