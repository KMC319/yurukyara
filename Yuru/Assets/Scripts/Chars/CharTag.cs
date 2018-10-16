using Chars;
using UnityEngine;

namespace Players{
	public class CharTag : MonoBehaviour{
		[SerializeField] private CharName charName;
		public CharName GetCharName => charName;
	}
}
