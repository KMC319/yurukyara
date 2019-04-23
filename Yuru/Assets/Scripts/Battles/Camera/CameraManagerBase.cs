using Battles.Systems;
using UnityEngine;

namespace Battles.Camera {
    public abstract class CameraManagerBase : IChangePhase {
        protected UnityEngine.Camera[] cameras = new UnityEngine.Camera[2];
        protected GameObject[] vcams = new GameObject[3];
        protected bool Initialized;
        public int LastAttackPlayerNum = 0; //1P = 0, 2P = 1
        public virtual void Init(UnityEngine.Camera[] cams) { }
        public virtual void ChangePhase(Phase changedPhase) { }
    }
}
