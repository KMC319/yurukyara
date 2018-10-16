using Chars;
using Players;
using UnityEngine;

namespace Systems{
	public class GameStateManager : MonoBehaviour{

		public ModeName mode;
		public CharName player1;
		public CharName player2;
		public static GameStateManager instance;

		private void Awake(){
			if (instance == null){
				instance = this;
				DontDestroyOnLoad(gameObject);
			}else{
				Destroy(gameObject);	
			}
		}
	}
}