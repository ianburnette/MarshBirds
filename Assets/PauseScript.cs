using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {

    public bool canPause, isPaused;
    public GameObject pauseMenu;
    public PlayerDisabler disabler;

	void Update () {
	    if (Input.GetButtonDown("Pause") && canPause)
            Pause();
	}

    public void Pause()
    {
        Time.timeScale = 1f - Time.timeScale;
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        disabler.TogglePlayerScripts(!isPaused);
    }
}
