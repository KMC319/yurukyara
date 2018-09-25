using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace doma{
	public class GaugeControll : MonoBehaviour{

		[SerializeField] private float movement=0.05f;
		[SerializeField]private float entityAmount=1;
		[SerializeField]private float tempAmount=1;

		private Image tempImg;
		private Image entityImg;

		private bool Down;
		
		private void Awake(){
			var myimg = this.GetComponent<Image>();
			var childs_imgs = transform.GetComponentsInChildren<Image>().ToList();
			childs_imgs.RemoveAll(n => myimg != null && n == myimg);
			tempImg = childs_imgs.Find(n => n.name.Contains("temp"));
			entityImg = childs_imgs.Find(n => !n.name.Contains("temp"));

			entityAmount = tempImg.fillAmount;
			tempAmount = entityAmount;
			tempImg.fillAmount=tempAmount;
		}

		public void SetAmount(float amount){
			if (!CheckAmount(amount)){ return; }
			entityAmount = amount;
			tempAmount = amount;
			entityImg.fillAmount = amount;
			tempImg.fillAmount = amount;
		}

		public void TempUpdate(float amount){
			if (!CheckAmount(amount)){ return; }

			if (tempAmount == amount) return;
			tempAmount = amount;
			
			if (entityAmount > tempAmount){//down
				entityImg.fillAmount = tempAmount;
				tempImg.fillAmount = entityAmount;
				Down = true;
			} else{//up
				entityImg.fillAmount = entityAmount;
				tempImg.fillAmount = tempAmount;
				Down = false;
			}
		}

		public void EntityUpdate(){
			if (tempAmount == entityAmount) return;
			var enumerator = Down ? DownUpdate() : UpUpdate();
			StartCoroutine(enumerator);
		}

		public void Reset(){
			tempAmount = entityAmount;	
			entityImg.fillAmount = entityAmount;
			tempImg.fillAmount = entityAmount;
		}

		private bool CheckAmount(float amount){
			if (amount > 1){
				DebugLogger.LogError("Amount Error.this num is over 1");
				return false;
			}
			return true;
		}

		IEnumerator UpUpdate(){
			yield return new WaitForSeconds(0.1f);
			while (entityImg.fillAmount <=tempAmount){
				entityImg.fillAmount += movement;
				yield return null;
			}
			entityAmount = tempAmount;
		}

		IEnumerator DownUpdate(){
			yield return new WaitForSeconds(0.1f);
			while (tempImg.fillAmount >= tempAmount){
				tempImg.fillAmount -= movement;
				yield return null;
			}
			entityAmount = tempAmount;
		}
	}
}