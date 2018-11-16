using System.Linq;
using doma;
using doma.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Start{
	public class KeyDisplay : MonoBehaviour,IDisplayPanel{
		private Image myimg;
		[SerializeField]private Image childimg;

		private void Awake() {
			myimg = this.GetComponent<Image>();
			myimg.enabled = false;
			childimg.enabled = false;
		}

		public void OnSelect(){
			myimg.enabled = true;
		}

		public void RemoveSelect(){
			myimg.enabled = false;
		}

		public void Submit(){
		}

		public bool IsActive{ get; set; }
		
		public void Launch(){
			//起動処理
			DebugLogger.Log("launch");
			childimg.enabled = true;
		}

		public void Finish(){
			//終了処理
			DebugLogger.Log("finish");
			childimg.enabled = false;
		}
	}
}