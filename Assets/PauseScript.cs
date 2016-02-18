using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {

    public bool canPause, isPaused;
    public GameObject pauseMenu;

	void Update () {
	    if (Input.GetButtonDown("Pause") && canPause)
            Pause();
	}

    void Pause()
    {
        Time.timeScale = 1f - Time.timeScale;
        isPaused = !isPaused;
        print("pause menu is " + pauseMenu.activeSelf);
        pauseMenu.SetActive(isPaused);
        print("after, pause menu is " + pauseMenu.activeSelf);
    }
}
