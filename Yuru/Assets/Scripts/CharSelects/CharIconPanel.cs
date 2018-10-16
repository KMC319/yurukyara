using Systems.Chars;
using UnityEngine;
using UnityEngine.UI;

namespace CharSelects{
	public class CharIconPanel : MonoBehaviour{
		[SerializeField] private CharName myName;public CharName MyName => myName;

		private Image img;

		private Image Img{
			get{
				if (img == null){
					img = this.GetComponent<Image>();
				}
				return img;
			}
		}

		public void SetUp(CharName char_name,Sprite sprite){
			myName = char_name;
			Img.sprite = sprite;
		}

	}
}