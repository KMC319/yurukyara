using System;
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

		private InterfaceEventSystem interfaceEventSystem;
		
		private void Start(){
			interfaceEventSystem = this.GetComponent<InterfaceEventSystem>();
			
			inputRelayPoint.ChangeReciever(interfaceEventSystem);
			
			interfaceEventSystem.CreateActiveSelectableList<ISelectablePanel>(ListCreateOption.Vertical);
			interfaceEventSystem.Launch();

			panelRoot.SetActive(false);
			interfaceEventSystem.EnterSelectable.Subscribe(Submit);

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
			if (i_selectable_panel is ModeSelectedPanel){
				var mode_select_panel = (ModeSelectedPanel) i_selectable_panel;
				GameStateManager.instance.mode = mode_select_panel.GetModeName;
				DebugLogger.Log(mode_select_panel.GetModeName);
				SceneManager.LoadScene("CharSelect");
			}else if (i_selectable_panel is IDisplayPanel){
				var display_panel = (IDisplayPanel)i_selectable_panel;
				display_panel.Launch();
				Observable.Timer(TimeSpan.FromSeconds(0.1f))
					.Subscribe(n => {
						interfaceEventSystem.SubmitKey
							.First()
							.Subscribe(m => {
								display_panel.Finish();
								interfaceEventSystem.ReBoot();
							}).AddTo(this);
					});
			}else{
				DebugLogger.LogError(i_selectable_panel+" is should not be selecting");
				return;
			}

		}
	}
}