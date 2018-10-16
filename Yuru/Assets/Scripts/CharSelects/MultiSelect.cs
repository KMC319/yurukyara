using Systems;
using doma;
using doma.Inputs;
using doma.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CharSelects{
	public class MultiSelect:SelectController{
		
		public MultiSelect(GameObject root,InputRelayPoint input_relay_point,Image[] images) : base(root,input_relay_point,images){
			inputRelayPoint.ChangeReciever(iEs1);
			inputRelayPoint.IsActive = true;
			
			var inst_input = root.AddComponent<InstantUiInput>();
			inst_input.iUiKyReciever = iEs2;
			inst_input.IsActive = true;
		}
	}
}