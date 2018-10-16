using UnityEngine;

namespace Systems{
	public class GameModeManager : MonoBehaviour{

		public ModeName mode;
		public static GameModeManager instance;

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