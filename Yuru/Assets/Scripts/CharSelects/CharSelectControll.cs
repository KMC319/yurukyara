using Chars;
using doma;
using doma.Inputs;
using doma.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace CharSelects{
	public class CharSelectControll{
		private InterfaceEventSystem interfaceEventSystem;
		private Subject<CharName?> myStream=new Subject<CharName?>();
		public IObservable<CharName?> MyStream=>myStream;

		public CharSelectControll(InterfaceEventSystem interface_event_system){
			interfaceEventSystem = interface_event_system;

			interface_event_system.EnterSelectable.Subscribe(Submit);
			interface_event_system.CancelKey.Subscribe(n=>Cancel());
		}

		private void Submit(ISelectablePanel i_selectable_panel){
			if (!(i_selectable_panel is SelectedPanel)){
				DebugLogger.LogError(i_selectable_panel+" is should not be selecting");
				return;
			}
			var char_panel=(SelectedPanel) i_selectable_panel;
			myStream.OnNext(char_panel.charName);
		}

		private void Cancel(){
			myStream.OnNext(null);
			interfaceEventSystem.ReBoot();
		}
	}
}