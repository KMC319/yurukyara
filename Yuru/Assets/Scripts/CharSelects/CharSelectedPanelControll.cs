using System;
using System.Collections.Generic;
using Systems.Chars;
using doma;
using doma.Inputs;
using doma.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CharSelects{
	public class CharSelectedPanelControll{
		private InterfaceEventSystem interfaceEventSystem;
		private Subject<CharName?> myStream=new Subject<CharName?>();
		public UniRx.IObservable<CharName?> MyStream=>myStream;

		private List<GameObject> myObjs;
		
		public CharSelectedPanelControll(InterfaceEventSystem interface_event_system,List<GameObject> objs){
			interfaceEventSystem = interface_event_system;
			myObjs = objs;
			
			interface_event_system.FocusSelectable.Subscribe(Focus);
			interface_event_system.EnterSelectable.Subscribe(Submit);
			interface_event_system.CancelKey.Subscribe(n=>Cancel());
		}

		private void Focus(ISelectablePanel i_selectable_panel){
			if (!(i_selectable_panel is CharSelectedPanel)){
				DebugLogger.LogError(i_selectable_panel+" is should not be selecting");
				return;
			}
			var char_panel=(CharSelectedPanel) i_selectable_panel;
			var index = FindCharIndex(char_panel.charName);
			foreach (var obj in myObjs) {
				obj.SetActive(myObjs.IndexOf(obj) == index);
			}
		}

		private void Submit(ISelectablePanel i_selectable_panel){
			if (!(i_selectable_panel is CharSelectedPanel)){
				DebugLogger.LogError(i_selectable_panel+" is should not be selecting");
				return;
			}
			var char_panel=(CharSelectedPanel) i_selectable_panel;
			myStream.OnNext(char_panel.charName);
		}

		private void Cancel(){
			myStream.OnNext(null);
			interfaceEventSystem.ReBoot();
		}
		
		private int FindCharIndex(CharName char_name) {
			return ParameterTable.Instance.CharIconInformations.FindIndex(n => n.charName == char_name);
		}
	}
}