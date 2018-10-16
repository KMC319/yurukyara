using Systems;
using CharSelects;
using doma;
using doma.Inputs;
using doma.Interfaces;
using UnityEngine;
using Zenject;
using UniRx;
using UnityEngine.SceneManagement;

namespace Start{
	public class ModePanelControll : MonoBehaviour{

		[SerializeField] private GameObject text;
		[SerializeField] private GameObject panelRoot;
		
		[Inject] private InputRelayPoint inputRelayPoint;
		
		
		private void Start(){
			var interface_event_system = this.GetComponent<InterfaceEventSystem>();
			
			inputRelayPoint.ChangeReciever(interface_event_system);
			
			interface_event_system.CreateActiveSelectableList<ModeSelectedPanel>(ListCreateOption.Vertical);
			interface_event_system.Launch();

			panelRoot.SetActive(false);
			interface_event_system.EnterSelectable.Subscribe(Submit);

			Observable.EveryUpdate()
				.Where(_ => Input.GetButtonDown("A1"))
				.First()
				.Subscribe(_ => Launch())
				.AddTo(this);
		}


		private void Launch(){
			Destroy(text);
			panelRoot.SetActive(true);
			inputRelayPoint.IsActive = true;
		}

		private void Submit(ISelectablePanel i_selectable_panel){
			if (!(i_selectable_panel is ModeSelectedPanel)){
				DebugLogger.LogError(i_selectable_panel+" is should not be selecting");
				return;
			}

			var mode_select_panel = (ModeSelectedPanel) i_selectable_panel;
			GameStateManager.instance.mode = mode_select_panel.GetModeName;
			DebugLogger.Log(mode_select_panel.GetModeName);
			SceneManager.LoadScene("CharSelect");
		}
	}
}