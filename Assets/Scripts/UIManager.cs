using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject UI;

    public bool gamePaused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        UI.SetActive(false);
        gamePaused = true;
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        UI.SetActive(true);
        gamePaused = false;
    }

    public void ChangeSceneByName(string name)
    {
        if (name !=null){
            SceneManager.LoadScene(name);
        }
    }
}
