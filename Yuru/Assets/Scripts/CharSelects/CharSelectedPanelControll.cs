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
		public IObservable<CharName?> MyStream=>myStream;

		private Image myImg;
		
		public CharSelectedPanelControll(InterfaceEventSystem interface_event_system,Image img){
			interfaceEventSystem = interface_event_system;
			myImg = img;
			
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
			myImg.sprite = FindCharImg(char_panel.charName);
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
		
		private Sprite FindCharImg(CharName char_name){
			return ParameterTable.Instance.CharIconInformations.Find(n => n.charName == char_name).CharImg;
		}
	}
}