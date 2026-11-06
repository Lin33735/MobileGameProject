using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject GameEndMenu;

    public TMP_Text endResult;
    public TMP_Text endScore;

    public bool gamePaused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gamePaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        UI.SetActive(false);
        GetComponent<GameManager>().ToggleOnTouch(true);
        gamePaused = true;
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        UI.SetActive(true);
        GetComponent<GameManager>().ToggleOnTouch(false);
        gamePaused = false;
    }

    public void ChangeSceneByName(string name)
    {
        if (name !=null){
            SceneManager.LoadScene(name);
        }
    }

    public void GameEnd(bool win)
    {
        if (!win)
        {
            endResult.text = "Game Over";
        }
        else
        {
            endResult.text = "Level Complete";
        }
        endScore.text = "Score: " + (GetComponent<GameManager>().roundNum - 1).ToString("F0");
        GameEndMenu.SetActive(true);
        UI.SetActive(false);
        GetComponent<GameManager>().ToggleOnTouch(true);
        gamePaused = true;
    }
}
