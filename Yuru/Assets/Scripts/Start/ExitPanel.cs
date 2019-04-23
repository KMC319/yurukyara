using doma.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Start {
    public class ExitPanel : MonoBehaviour, IDisplayPanel {
        private Image myimg;

        private void Awake() {
            myimg = this.GetComponent<Image>();
            myimg.enabled = false;
        }

        public void OnSelect() {
            myimg.enabled = true;
        }

        public void RemoveSelect() {
            myimg.enabled = false;
        }

        public void Submit() {
            Application.Quit();
        }

        public bool IsActive { get; set; }

        public void Launch() { }

        public void Finish() { }
    }
}
