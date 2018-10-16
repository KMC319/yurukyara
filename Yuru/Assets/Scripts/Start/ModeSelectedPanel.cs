using Systems;
using doma.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Start{
	public class ModeSelectedPanel : MonoBehaviour,ISelectablePanel{
		[SerializeField] private ModeName modeName;public ModeName GetModeName => modeName;

		private Image myimg;

		private void Awake(){
			myimg = this.GetComponent<Image>();
			transform.GetComponentInChildren<Text>().text = modeName.ToString();
			myimg.enabled = false;
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
	}
}