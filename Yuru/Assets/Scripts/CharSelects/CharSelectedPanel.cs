using System;
using Systems.Chars;
using doma;
using doma.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace CharSelects{
	public abstract class CharSelectedPanel : MonoBehaviour,ISelectablePanel{

		public CharName charName{ get; private set; }
		private Image img;

		private void Start(){
			try{
				charName = this.transform.parent.GetComponent<CharIconPanel>().MyName;
			}catch (Exception e){
				DebugLogger.LogError(e+"MyParent isnt CharIconPanel ");
				throw;
			}

			img = this.GetComponent<Image>();
		}

		public void OnSelect(){
			img.enabled = true;
		}

		public void RemoveSelect(){
			img.enabled = false;
		}

		public void Submit(){
		
		}

		public bool IsActive{ get; set; }
	}
}