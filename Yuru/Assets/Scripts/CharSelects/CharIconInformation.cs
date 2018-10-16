using System;
using Chars;
using UnityEngine;

namespace CharSelects{
	[Serializable]
	public struct CharIconInformation{
		[SerializeField] public CharName charName;
		[SerializeField] public Sprite CharImg;
		[SerializeField] public Sprite IconImg;
	}
}