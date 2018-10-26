using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Battles.Systems{
	public class WinnerCounterDisplay : MonoBehaviour{
		[SerializeField] private PlayerNum playerNum;
		public PlayerNum GetPlayerNum => playerNum;
		[SerializeField]private Image[] imgs;
		public int Count{ get; set; }

		private void Awake() {
			this.ObserveEveryValueChanged(n => n.Count)
				.Subscribe(n => ApplyImg());
			ApplyImg();
		}

		private void ApplyImg() {
			if (Count < 1) return;
			imgs[Count - 1].enabled = true;
		}
	}
}