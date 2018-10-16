using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Start{
	public class FlashingText : MonoBehaviour{
		[SerializeField] private float interval;
	
		private Text txt;
		private WaitForSeconds seconds;
		
		private void Start(){
			txt = this.GetComponent<Text>();
			StartCoroutine(Flash());
			seconds=new WaitForSeconds(interval);
		}

		private IEnumerator Flash(){
			var defo_color = txt.color;
			while (true){
				var temp = txt.color;
				temp.a = 0;
				txt.color = temp;
				yield return seconds;
				txt.color = defo_color;
				yield return seconds;

			}
		}
	}
}