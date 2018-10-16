using UnityEngine;

namespace Systems.Chars{
	public class CharTag : MonoBehaviour{
		[SerializeField] private CharName charName;
		public CharName GetCharName => charName;
	}
}
