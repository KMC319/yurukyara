using doma.Inputs;
using UnityEngine;

namespace Battles.Players {
	public class ComBrain : MonoBehaviour {
		public IBattleKeyReciever iBattleKeyReciever;
		[SerializeField] private bool isActive;public bool IsActive{set{ isActive = value; }}

		private float minInterval = 0.1f;
		private float maxInterval = 0.5f;
		private int jump = 0;
		private int rangeAt = 50;
		private int weakAt = 50;
		private int strongAt = 50;
		private int guard = 50;
		private float interval;
		private float count;
		private float horizontal;
		private float vertical;
		private bool getButton;

		void Start () {
		}
	
		void Update () {
			if (!isActive) return;
			count += Time.deltaTime;
			if (count > interval) {
				count = 0f;
				interval = Random.Range(minInterval, maxInterval);
				UpdateMove();
			}
			iBattleKeyReciever.ChangeHorizontalAxis(horizontal);
			iBattleKeyReciever.ChangeVerticalAxis(vertical);

			if (getButton) iBattleKeyReciever.GuardKey();
		}

		private void UpdateMove() {
			horizontal = Random.Range(-1f, 1f);
			var rand = Random.Range(0, 100);
			vertical = rand < 20 ? -1 : rand < 40 ? 0 : 1;
			if (Random.Range(0, 100) < jump) iBattleKeyReciever.JumpKey();
			if (Random.Range(0, 100) < rangeAt) iBattleKeyReciever.RangeAtKey();
			if (Random.Range(0, 100) < weakAt) iBattleKeyReciever.WeakAtKey();
			if (Random.Range(0, 100) < strongAt) iBattleKeyReciever.StrongAtKey();
			getButton = Random.Range(0, 100) < guard;
		}
	}
}
