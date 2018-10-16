using Systems.Chars;
using doma.Inputs;
using UnityEngine;
using UnityEngine.UI;

namespace CharSelects{
	public class SoloSelect:SelectController{
		public SoloSelect(GameObject root,InputRelayPoint input_relay_point,Image[] images) : base(root,input_relay_point,images){
			SetFirstCtl();
		}

		protected override void Flow(CharName? name, int index){
			if (name != null && index == 0){
				SetNextCtl();
			}else if (name == null && index == 1){
				SetFirstCtl();
				base.Flow(null,0);
			}
			base.Flow(name, index);
		}

		private void SetFirstCtl(){
			inputRelayPoint.ChangeReciever(iEs1);
			inputRelayPoint.IsActive = true;
			iEs2.Freeze();
			iEs1.ReBoot();
		}
		
		private void SetNextCtl(){
			inputRelayPoint.ChangeReciever(iEs2);
			inputRelayPoint.IsActive = true;
			iEs1.Freeze();
			iEs2.ReBoot();
		}
	}
}