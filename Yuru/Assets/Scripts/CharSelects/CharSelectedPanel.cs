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
		private Text txt;
		protected WorldImg WorldImg;

		private void Start(){
			try{
				charName = this.transform.parent.GetComponent<CharIconPanel>().MyName;
			}catch (Exception e){
				DebugLogger.LogError(e+"MyParent isnt CharIconPanel ");
				throw;
			}
			WorldImg = transform.parent.parent.parent.parent.GetComponentInChildren<WorldImg>();
			img = this.GetComponent<Image>();
			txt = GetComponentInChildren<Text>();
		}

		public virtual void OnSelect(){
			img.enabled = true;
			txt.enabled = true;
		}

		public virtual void RemoveSelect(){
			img.enabled = false;
			txt.enabled = false;
		}

		public void Submit(){
		
		}

		public bool IsActive{ get; set; }
	}
}