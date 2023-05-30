using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject optionsMenuUI;
    public AudioListener playerAudioListener;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //To do add start button
  //      if(Input.GetKeyDown(KeyCode.Escape))
		//{
  //          if(GameIsPaused)
		//	{
  //              Resume();
		//	}
  //          else
		//	{
  //              Pause();
		//	}
		//}
    }

    void Resume()
	{
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
	{
        optionsMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void HandleStereoSpeaker()
	{
        Transform optionMenu = transform.GetChild(0);
        Transform toggleStereoSpeaker = optionMenu.Find("ToggleStereoSpeaker");
  //      if(toggleStereoSpeaker.GetComponent<Toggle>().isOn)
		//{
  //          playerAudioListener.GetComponent<ResonanceAudioListener>().stereoSpeakerModeEnabled = enabled;
		//}
  //      else
		//{
  //          playerAudioListener.GetComponent<ResonanceAudioListener>().stereoSpeakerModeEnabled = !enabled;
  //      }
	}
}
