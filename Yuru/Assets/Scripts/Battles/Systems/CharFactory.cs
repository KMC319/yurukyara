using System.Collections.Generic;
using System.Linq;
using Chars;
using doma;
using Players;
using UnityEngine;
using Zenject;

namespace Battles.Systems{
	public class CharFactory : MonoBehaviour{
		[SerializeField] private List<CharName> debugChars;
		[SerializeField] private List<CharTag> chars;
		[SerializeField] private Vector3 p1Position;
		[SerializeField] private Vector3 p2Position;

		
		
		private void Awake(){
			if (debugChars.Count == 2){
				Launch(debugChars[0],debugChars[1]);	
			}
		}

		public void Launch(CharName char1,CharName char2){
			var p1=Instantiate(chars.Find(n=>n.GetCharName==char1).gameObject,transform.position,transform.rotation);
			p1.transform.parent = transform.parent;
			p1.transform.position = p1Position;
			var p2=Instantiate(chars.Find(n=>n.GetCharName==char2).gameObject,transform.position,transform.rotation);
			p2.transform.parent = transform.parent;
			p2.transform.position = p2Position;
			
			GetComponent<PlayerDIContainer>().Launch(p1,p2);
		}
	}
}