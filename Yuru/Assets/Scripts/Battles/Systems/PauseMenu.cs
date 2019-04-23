using System.Linq;
using doma;
using doma.Inputs;
using doma.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Battles.Systems {
    public class PauseMenu : MonoBehaviour {
        [SerializeField] private PauseSelectedPanel[] pauseSelectedPanels;
        private int currentNum;
        private float interval = 0;

        private void Start() {
            foreach (var pauseSelectedPanel in pauseSelectedPanels) {
                pauseSelectedPanel.RemoveSelect();
            }
            pauseSelectedPanels[currentNum].OnSelect();
        }

        private void Update() {
            Move();
            Enter();
        }

        private void Move() {
            var y = Input.GetAxis("Vertical");
            if (y != 0) {
                if (interval <= 0) {
                    if (y > 0) {
                        var preNum = currentNum;
                        currentNum = Mathf.Clamp(--currentNum, 0, 3);
                        pauseSelectedPanels[preNum].RemoveSelect();
                        pauseSelectedPanels[currentNum].OnSelect();
                    }

                    if (y < 0) {
                        var preNum = currentNum;
                        currentNum = Mathf.Clamp(++currentNum, 0, 3);
                        pauseSelectedPanels[preNum].RemoveSelect();
                        pauseSelectedPanels[currentNum].OnSelect();
                    }
                    interval = 0.2f;
                }
                interval -= Time.deltaTime;
            } else {
                interval = 0;
            }
        }
        
        private void Enter() {
            if (Input.GetButtonDown("Submit")) pauseSelectedPanels[currentNum].Submit();
        }
        

        /*使おうと思ったけどよくわからんかったから今はパス
        [Inject] private InputRelayPoint inputRelayPoint;

        private void Start() {
            var interface_event_system = this.GetComponent<InterfaceEventSystem>();

            inputRelayPoint.ChangeReciever(interface_event_system);

            interface_event_system.CreateActiveSelectableList<PauseSelectedPanel>(ListCreateOption.Vertical);
            interface_event_system.Launch();
        }

        private void OnEnable() {
            inputRelayPoint.IsActive = true;
        }

        private void OnDisable() {
            inputRelayPoint.IsActive = false;
        }*/
    }
}
