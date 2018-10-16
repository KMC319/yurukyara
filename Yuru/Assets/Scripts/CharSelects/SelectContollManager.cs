using System;
using Systems;
using Systems.Chars;
using doma;
using doma.Inputs;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace CharSelects{
	public class SelectContollManager : MonoBehaviour{
		[SerializeField] private ModeName mode;
		[SerializeField] private Image p1Img;
		[SerializeField] private Image p2Img;
		
		private CharName?[] selectedChars=new CharName?[]{null,null};

		[Inject]private InputRelayPoint inputRelayPoint;

		private void Start(){
			var imgs = new Image[]{p1Img, p2Img};
			var m = mode;
			if (GameStateManager.instance != null){
				m = GameStateManager.instance.mode;
			}
			SelectController select_controller;
			switch (m){
				case ModeName.Arcade:
					throw new ArgumentOutOfRangeException();
					break;
				case ModeName.VsCom:
					select_controller=new SoloSelect(gameObject,inputRelayPoint,imgs);
					break;
				case ModeName.Practice:
					select_controller=new SoloSelect(gameObject,inputRelayPoint,imgs);
					break;
				case ModeName.Multi:
					select_controller=new MultiSelect(gameObject,inputRelayPoint,imgs);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			select_controller.CharStream.Subscribe(SetChar).AddTo(this);
		}

		private void SetChar(FlowChar flow_char){
			try{
				selectedChars[flow_char.num] = flow_char.charName;
			}
			catch (Exception e){
				DebugLogger.LogError(e);
				throw;
			}
			DebugLogger.Log(selectedChars[0]+","+selectedChars[1]);
			if (selectedChars[0] != null && selectedChars[1] != null){
				GameStateManager.instance.player1 = (CharName)selectedChars[0];
				GameStateManager.instance.player2 = (CharName)selectedChars[1];
				SceneManager.LoadScene("TestDoma");
			}
		}

	}
}