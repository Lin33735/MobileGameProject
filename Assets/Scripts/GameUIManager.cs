using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public Slider sliderEHP;

    [Header("StatPanel")]
    public TMP_Text statPoints;
    public TMP_Text ATKStat;
    public TMP_Text critRStat;
    public TMP_Text critDMGStat;

    [SerializeField] private RectTransform statPanel;
    [SerializeField] private float aniDur = 0.3f;
    [SerializeField] private AnimationCurve aniCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private Vector2 hiddenPos;
    [SerializeField] private Vector2 shownPos;
    [SerializeField] private bool statPanelOpen = false;
    private Coroutine statPanAniCoroutine;

    [Header("StatPanelRefs")]
    [SerializeField] private GameObject statATK;

    [Header("Damage Text Dispaly")]
    public TMP_Text DMGTextPF;
    public GameObject DMGTextDisplayLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateStatDisplay();

        if (shownPos == Vector2.zero && hiddenPos == Vector2.zero)
        {
            shownPos = statPanel.anchoredPosition;
            hiddenPos = new Vector2(shownPos.x, -statPanel.rect.height);
        }
        statPanel.anchoredPosition = hiddenPos;

        DMGTextPF.text = Math.Round(this.GetComponent<GameManager>().DMGCalc(), MidpointRounding.AwayFromZero).ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayEnemyHPBar(bool full, float current)
    {
        if (full)
        {
            sliderEHP.maxValue = current;
        }
        sliderEHP.value = current;
        Debug.Log($"{sliderEHP.value}, {sliderEHP.maxValue}");
    }

    public void ToggleStatPanel()
    {
        if (statPanelOpen)
        {
            statPanelOpen = false;
            GetComponent<GameManager>().ToggleOnTouch(false);

            StartAnimation(hiddenPos);
        }
        else
        {
            statPanelOpen = true;
            GetComponent<GameManager>().ToggleOnTouch(true);
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

    public void InstantiateDMGText(float dmg)
    {
        Instantiate(DMGTextPF, DMGTextDisplayLocation.transform);
        DMGTextPF.text = Math.Round(dmg, MidpointRounding.AwayFromZero).ToString();
    }

    public void UpdateStatDisplay()
    {
        statPoints.text = "Points = " + GetComponent<GameManager>().pStatPoint.ToString();
        ATKStat.text = "ATK = " + GetComponent<GameManager>().pBATK.ToString();
        critRStat.text = "Crit Rate = " + (GetComponent<GameManager>().pCRITR * 100).ToString() + "%";
        critDMGStat.text = "Crit DMG = " + (GetComponent<GameManager>().pCRITDMG + 1).ToString() + "x";
    }
}
