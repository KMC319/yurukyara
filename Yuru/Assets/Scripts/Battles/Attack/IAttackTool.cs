using System;
using Battles.Players;
using doma;
using UniRx;
using UnityEngine;

namespace Battles.Attack{
	//用途はInterFaceだけどシリアライズのためにabstructになってます
	public abstract class IAttackTool:MonoBehaviour{
		public abstract void On();
		public abstract void Off();
	}
}