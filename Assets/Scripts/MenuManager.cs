using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject playButton, levels;

    private void Awake()
    {
        playButton.SetActive(true);
        levels.SetActive(false);
    }

    public void PlayPressed()
    {
        playButton.SetActive(false);
        levels.SetActive(true);
    }

    public void LevelSelected()
    {
        int level = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
        PlayerPrefs.SetInt("Level", level);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
