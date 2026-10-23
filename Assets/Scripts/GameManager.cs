using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class GameManager : MonoBehaviour
{
    [Header("Control")]
    [SerializeField] private InputActionReference OnTouch;
    private bool isHolding;
    private float holdTimer;

    public float roundNum = 1;

    [Header("PlayerStat")]
    public GameObject player;
    
    public int pLvl = 1;
    public int pSP = 1;

    public bool pHoldEnable = false;
    public bool pBurnEnable = false;

    public float pBATK = 10f;
    public float pDMGMULT = 1f;
    public float pBurnDMGScale = 0.01f;
    public float pBurnDur = 5f;
    public float pCRITR = 0.05f;
    public float pCRITDMG = 0.5f;

    public float pStatPoint;

    [Header("EnemyStat")]
    public GameObject enemy;

    public bool isElite = false;
    public float eliteChance = 0.2f;
    public float eBHP = 100f;
    public float eHP;

    private void OnEnable()
    {
        OnTouch.action.performed += OnPreformed;
        OnTouch.action.canceled += OnCanceled;
        OnTouch.action.Enable();
    }

    private void OnDisable()
    {
        OnTouch.action.performed -= OnPreformed;
        OnTouch.action.canceled -= OnCanceled;
        OnTouch.action.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
        }

        //Spawn Enemy When Killed
        if (eHP <= 0)
        {
            roundNum++;
            if (isElite)
            {
                pStatPoint += 3;
            }
            else
            {
                pStatPoint ++;
            }
            GetComponent<GameUIManager>().UpdateStatDisplay();
            SpawnEnemy();
        }
    }

    private void OnCanceled (InputAction.CallbackContext ctx)
    {
        if (isHolding)
        {
            isHolding = false;
            Debug.Log($"HOLD released after{holdTimer:F2}s");
            OnHoldRelease(holdTimer);
            holdTimer = 0;
        }
    }

    private void OnPreformed(InputAction.CallbackContext ctx)
    {
        switch (ctx.interaction)
        {
            case TapInteraction:
                Debug.Log("TAP");
                OnTap(ctx);
                break;
            
            case HoldInteraction:
                if (!pHoldEnable)
                {
                    OnTap(ctx);
                    return;
                }
                isHolding = true;
                holdTimer = 0f;
                Debug.Log("HOLD Started");
                OnHoldStart();
                break;
        }
    }

    private void OnTap(InputAction.CallbackContext ctx)
    {
        eHP -= DMGCalc();
        GetComponent<GameUIManager>().InstantiateDMGText(DMGCalc());
        GetComponent<GameUIManager>().DisplayEnemyHPBar(false,eHP);
    }

    private void OnHoldStart()
    {

    }

    private void OnHoldRelease(float heldDuration)
    {
        Debug.Log($"Trigger hold action with charge: {heldDuration:F2}s");
    }

    public float DMGCalc()
    {
        float CritAmount = Mathf.Floor(pCRITR);
        if (UnityEngine.Random.value <= pCRITR % 1)
        {
            CritAmount++;
        }
        Debug.Log(CritAmount);
        Debug.Log(pCRITDMG);
        return pBATK * (1 + CritAmount * pCRITDMG) * pDMGMULT;
    }

    public void SpawnEnemy()
    {
        if (roundNum == 1)
        {
            eHP = eBHP;
            enemy.GetComponent<SpriteRenderer>().color = new Color(1f, 0.44f, 0.44f, 1f);
            GetComponent<GameUIManager>().DisplayEnemyHPBar(true, eHP);
            return;
        }
        //Check is spawn is Elite
        if (UnityEngine.Random.value <= eliteChance && roundNum > 4)
        {
            eHP = (1 + roundNum / 5) * eBHP;
            isElite = true;
            enemy.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.44f, 1f, 1f);
        }
        else
        {
            eHP = (1 + roundNum / 10) * eBHP;
            isElite= false;
            enemy.GetComponent<SpriteRenderer>().color = new Color(1f, 0.44f, 0.44f, 1f);
        }
        //HP Scaling
        eBHP = (1 + roundNum / 40) * eBHP;
        GetComponent<GameUIManager>().DisplayEnemyHPBar(true, eHP);
    }
    
    public void ToggleOnTouch(bool t)
    {
        if (t == true)
        {
            OnTouch.action.Disable();
        }
        else
        {
            OnTouch.action.Enable();
        }
    }

    public void IncrementATK()
    {
        if (pStatPoint > 0)
        {
            pBATK += 5;
            pStatPoint--;
            GetComponent<GameUIManager>().UpdateStatDisplay();
        }
    }

    public void IncrementCritR()
    {
        if (pStatPoint > 0)
        {
            pCRITR += 0.01f;
            pStatPoint--;
            GetComponent<GameUIManager>().UpdateStatDisplay();
        }
    }

    public void IncrementCritDMG()
    {
        if (pStatPoint > 0)
        {
            pCRITDMG += 0.1f;
            pStatPoint--;
            GetComponent<GameUIManager>().UpdateStatDisplay();
        }
    }
}