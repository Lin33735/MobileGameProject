using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject UI;

    public Slider sliderEHP;

    public bool gamePaused;

    [Header("StatPanel")]
    [SerializeField] private RectTransform statPanel;
    [SerializeField] private float aniDur = 0.3f;
    [SerializeField] private AnimationCurve aniCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private Vector2 hiddenPos;
    [SerializeField] private Vector2 shownPos;
    private bool statPanelOpen = false;
    private Coroutine statPanAniCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (shownPos == Vector2.zero && hiddenPos == Vector2.zero)
        {
            shownPos = statPanel.anchoredPosition;
            hiddenPos = new Vector2(shownPos.x, -statPanel.rect.height);
        }
        statPanel.anchoredPosition = hiddenPos;
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
    
    public void DisplayEnemyHPBar (bool full, float current)
    {
        if (full)
        {
            sliderEHP.maxValue = current;
        }
        sliderEHP.value = current;
        Debug.Log($"{ sliderEHP.value}, { sliderEHP.maxValue}");
    }

    public void ToggleStatPanel()
    {
        if (statPanelOpen)
        {
            statPanelOpen = false;
            this.GetComponent<GameManager>().ToggleOnTouch(false);

            StartAnimation(hiddenPos);
        }
        else
        {
            statPanelOpen = true;
            this.GetComponent<GameManager>().ToggleOnTouch(true);
            StartAnimation(shownPos);
        }
    }

    private void StartAnimation(Vector2 targetPosition)
    {
        if (statPanAniCoroutine != null)
        {
            StopCoroutine(statPanAniCoroutine);
        }
        statPanAniCoroutine = StartCoroutine(AnimatePanel(targetPosition));
    }

    private IEnumerator AnimatePanel(Vector2 targetPosition)
    {
        Vector2 startPosition = statPanel.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < aniDur)
        {
            elapsed += Time.deltaTime;
            float t = aniCurve.Evaluate(elapsed / aniDur);
            statPanel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        statPanel.anchoredPosition = targetPosition;
    }
}
