﻿using Battles.Players;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace Battles.Systems{
	public class PlayerDIContainer : MonoBehaviour{
		[SerializeField] private CinemachineVirtualCamera p1Vcam;
		[SerializeField] private CinemachineVirtualCamera p2Vcam;

		[Inject] private DiContainer diContainer;
		
		public void Launch(GameObject p1,GameObject p2){
			var p1fb = p1.AddComponent<FirstPlayerBinder>();
			var p2fb = p2.AddComponent<SecondPlayerBinder>();
			p1fb.SetUp(p2fb);
			p2fb.SerUp(p1fb);
			
			diContainer.InjectGameObject(p1);
			

			var lt1 = p1.GetComponentInChildren<LookTarget>();
			var lt2 = p2.GetComponentInChildren<LookTarget>();

			lt1.target = lt2.gameObject;
			lt2.target = lt1.gameObject;

			p1Vcam.Follow = lt1.transform;
			p1Vcam.LookAt= lt2.transform;
			p2Vcam.Follow = lt2.transform;
			p2Vcam.LookAt = lt1.transform;
		}
	}
}