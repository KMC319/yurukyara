using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleBack : MonoBehaviour {
    private float time = 2;
    [SerializeField] private Image image;

    private void Update() {
        if (Input.GetButton("Cancel")) time -= Time.deltaTime;
        else {
            time = Mathf.Clamp(time + Time.deltaTime, 0, 2);
        }

        image.fillAmount = 1 - (time / 2f);

        if (time < 0) SceneManager.LoadScene("Start");
    }
}
