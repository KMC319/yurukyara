using System.Collections.Generic;
using UnityEngine;

namespace CharSelects {
    public class ObjFactory : MonoBehaviour {
        public List<GameObject> CharList { get; } = new List<GameObject>();
        private void Awake() {
            foreach (var item in ParameterTable.Instance.CharIconInformations) {
                var temp = Instantiate(item.CharObj, transform.position, transform.rotation);
                temp.SetActive(false);
                temp.transform.parent = transform;
                CharList.Add(temp);
            }
        }
    }
}
