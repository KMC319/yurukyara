using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial {
    public class CheckListManager : MonoBehaviour {
        private Image[] imgs;
        [SerializeField] private CheckList[] checkList;

        private Text[] texts;

        private void Start() {
            imgs = GetComponentsInChildren<Image>().OrderByDescending(i => i.transform.position.y).ToArray();
            texts = imgs.Select(i => i.GetComponentInChildren<Text>()).ToArray();
            gameObject.SetActive(false);
        }

        public void DisplayCheckList(int state) {
            foreach (var text in texts) {
                text.transform.parent.gameObject.SetActive(false);
            }
            for (int i = 0; i < checkList[state].CheckableList.Count; i++) {
                texts[i].text = checkList[state].CheckableList[i];
                texts[i].transform.parent.gameObject.SetActive(true);
            }
            gameObject.SetActive(true);
        }
        
        [Serializable]
        private struct CheckList {
            public List<string> CheckableList;
        }
    }
}
