using UnityEngine;
using UnityEngine.UI;

namespace CharSelects{
	public class IconFactory : MonoBehaviour{
		[SerializeField] private CharIconPanel charIconPanel;
		
		private void Awake() {
			var imgs = GetComponentsInChildren<Image>();
			var num = 0;
			foreach (var item in ParameterTable.Instance.CharIconInformations) {
				var temp = Instantiate(charIconPanel, imgs[num].transform);
				temp.SetUp(item.charName,item.IconImg);
				temp.transform.localScale = Vector3.one;
				temp.transform.localPosition = Vector3.zero;
				num++;
			}
		}
	}
}