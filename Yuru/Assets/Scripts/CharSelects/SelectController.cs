using System.Collections.Generic;
using Chars;
using doma;
using doma.Inputs;
using doma.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharSelects{
	public struct FlowChar{
		public CharName? charName;
		public int num;

		public FlowChar(CharName? char_name, int num){
			charName = char_name;
			this.num = num;
		}
	}
	
	public abstract class SelectController{
		private Subject<FlowChar> charStream=new Subject<FlowChar>();
		public IObservable<FlowChar> CharStream => charStream;

		protected InputRelayPoint inputRelayPoint;
		
		protected CharSelectControll cSc1;
		protected CharSelectControll cSc2;

		protected InterfaceEventSystem iEs1;
		protected InterfaceEventSystem iEs2;

		public SelectController(GameObject root,InputRelayPoint input_relay_point,Image[] imgs){
			if (imgs.Length != 2){
				DebugLogger.LogError("Images is insufficient");
				return;
			}
			inputRelayPoint = input_relay_point;
			iEs1 = root.AddComponent<InterfaceEventSystem>();
			iEs1.CreateActiveSelectableList<SelectedPanelP1>(ListCreateOption.Horizontal);
			cSc1=new CharSelectControll(iEs1,imgs[0]);
			iEs1.Launch();

			
			iEs2 = root.AddComponent<InterfaceEventSystem>();
			iEs2.CreateActiveSelectableList<SelectedPanelP2>(ListCreateOption.Horizontal);
			cSc2=new CharSelectControll(iEs2,imgs[1]);
			iEs2.Launch();

			cSc1.MyStream.Subscribe(n =>Flow(n,0));
			cSc2.MyStream.Subscribe(n =>Flow(n,1));
		}

		protected virtual void Flow(CharName? name, int index){
			charStream.OnNext(new FlowChar(name, index));
		}
	}
}