using Systems;
using UnityEngine;
using UnityEngine.UI;

public class ModeTxt : MonoBehaviour {
    private void Start() {
        var gsm = GameStateManager.instance;
        if(gsm != null)GetComponent<Text>().text = "MODE:" + gsm.mode;
    }
}
