using UnityEngine;

namespace CharSelects{
	public class IconFactory : MonoBehaviour{
		[SerializeField] private CharIconPanel charIconPanel;
		
		private void Awake() {
			foreach (var item in ParameterTable.Instance.CharIconInformations) {
				var temp = Instantiate(charIconPanel);
				temp.SetUp(item.charName,item.IconImg);
				temp.transform.parent = transform;
				temp.transform.localScale = Vector3.one;
				temp.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
			}
		}
	}
}