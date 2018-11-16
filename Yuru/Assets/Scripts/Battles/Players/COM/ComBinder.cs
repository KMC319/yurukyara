using Systems;
using Battles.Systems;
using doma.Inputs;
using UnityEngine;

namespace Battles.Players {
    public class ComBinder : SecondPlayerBinder {
        
        private ComBrain comBrain;

        public override void SetInputEnable(bool en) {
            comBrain.IsActive = en;
        }

        private new void Awake() {
            PlayerRoot = this.GetComponent<PlayerRoot>();
            comBrain = gameObject.AddComponent<ComBrain>();
            comBrain.iBattleKeyReciever=this.GetComponent<IBattleKeyReciever>();
        }
    }
}
