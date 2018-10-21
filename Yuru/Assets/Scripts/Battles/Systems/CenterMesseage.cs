using UnityEngine;
using UnityEngine.UI;

namespace Battles.Systems{
	public class CenterMesseage : MonoBehaviour{

		private Text txt;

		private void Awake(){
			txt = this.GetComponent<Text>();
		}

		public void Display(string mess){
			txt.text = mess;
		}
	}
}