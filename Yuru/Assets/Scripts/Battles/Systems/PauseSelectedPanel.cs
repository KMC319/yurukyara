using System;
using doma.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Battles.Systems {
    public class PauseSelectedPanel : MonoBehaviour, ISelectablePanel {
        [SerializeField] private Pmenu menu;
        
        private Pausable pausable;
        private Text text;

        private void Awake() {
            text = GetComponent<Text>();
        }

        private void Start() {
            pausable = GameObject.Find("root").GetComponent<Pausable>();
        }

        public void OnSelect() {
            text.color = Color.green;
        }

        public void RemoveSelect() {
            text.color = Color.white;
        }

        public void Submit() {
            switch (menu) {
                case Pmenu.ToBattle:
                    BGMer.Instance.Delete();
                    GameObject.Find("root").GetComponent<BattleManager>().ReGame();
                    break;
                case Pmenu.ToChar:
                    BGMer.Instance.Delete();
                    SceneManager.LoadScene("CharSelect");
                    break;
                case Pmenu.ToTitle:
                    BGMer.Instance.Delete();
                    SceneManager.LoadScene("Start");
                    break;
                case Pmenu.EndGame:
                    Application.Quit();
                    break;
                case Pmenu.Resume:
                    pausable.Submit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsActive { get; set; }

        private enum Pmenu {
            ToBattle, ToChar, ToTitle,
            EndGame, Resume
        }
    }
}
