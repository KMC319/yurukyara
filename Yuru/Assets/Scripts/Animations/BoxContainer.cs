using System.Collections.Generic;
using Battles.Health;
using Players;
using UnityEngine;

namespace Animations{
	public class BoxContainer : MonoBehaviour{
		[SerializeField] private List<MotionBox> motionBoxs;
		public List<MotionBox> MotionBoxs => motionBoxs;
		[SerializeField] private List<AttackBox> attackBoxs;
		public List<AttackBox> AttackBoxs => attackBoxs;

		public MotionBox FindMotion(string name){
			return motionBoxs.Find(n=>n.clip.name==name); 
		}

		public AttackBox FindAttack(string name){
			return attackBoxs.Find(n => n.clip.name == name);
		}

		public AnimBox FindAnim(string name){
			var c = new List<AnimBox>();
			c.AddRange(motionBoxs);
			c.AddRange(attackBoxs);
			return c.Find(n=>n.clip.name==name);
		}
	}
}