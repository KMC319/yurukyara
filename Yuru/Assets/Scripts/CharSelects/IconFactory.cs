using UnityEngine;

namespace CharSelects{
	public class IconFactory : MonoBehaviour{
		[SerializeField] private CharIconPanel charIconPanel;
		[SerializeField] private Vector2 startPoint;
		
		private void Awake(){
			foreach (var item in ParameterTable.Instance.CharIconInformations){
				var temp=Instantiate(charIconPanel);
				temp.SetUp(item.charName,item.IconImg);
				temp.transform.parent = transform;
			}
		}
	}
}