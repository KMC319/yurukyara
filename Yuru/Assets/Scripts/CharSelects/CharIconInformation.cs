using System;
using Systems.Chars;
using UnityEngine;

namespace CharSelects{
	[Serializable]
	public struct CharIconInformation{
		[SerializeField] public CharName charName;
		[SerializeField] public GameObject CharObj;
		[SerializeField] public Sprite IconImg;
	}
}