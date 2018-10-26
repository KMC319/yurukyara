using UnityEngine;

namespace CharSelects{
	public class CharSelectedPanelP2 : CharSelectedPanel{
		public override void OnSelect() {
			base.OnSelect();
			WorldImg.OnSelect(charName,1);
		}

		public override void RemoveSelect() {
			base.RemoveSelect();
			WorldImg.Remove(charName, 1);
		}
		
	}
}