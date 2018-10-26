using UnityEngine;

namespace CharSelects{
	public class CharSelectedPanelP1 : CharSelectedPanel{
		public override void OnSelect() {
			base.OnSelect();
			WorldImg.OnSelect(charName,0);
		}

		public override void RemoveSelect() {
			base.RemoveSelect();
			WorldImg.Remove(charName, 0);
		}
	}
}