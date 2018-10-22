using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Battles.Systems{
	public class WinnerCounter : MonoBehaviour{
		public int Count{ get; set; }

		private Text txt;

		private void Awake(){
			txt = this.GetComponent<Text>();
			txt.text = Count.ToString();
			this.ObserveEveryValueChanged(n => n.Count)
				.Subscribe(n => txt.text = Count.ToString());
		}
	}
}