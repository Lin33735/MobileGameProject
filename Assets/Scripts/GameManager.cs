using System.CodeDom.Compiler;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class GameManager : MonoBehaviour
{
    [Header("Control")]
    [SerializeField] private InputActionReference OnTouch;
    private bool isHolding;
    private float holdTimer;

    public float roundNum;

    [Header("PlayerStat")]
    public GameObject player;
    
    public int pLvl;
    public int pSP;

    public bool pHoldEnable;
    public bool pBurnEnable;

    public float pBATK;
    public float pDMGMULT;
    public float pBurnDMGScale;
    public float pBurnDur;

    [Header("EnemyStat")]
    public float eBHP;
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
        roundNum = 1;
        
        eBHP = 100f;

        pLvl = 1;
        pSP = 1;
        pBATK = 10f;
        pHoldEnable = false;
        pDMGMULT = 1f;
        pBurnEnable = false;
        pBurnDMGScale = 0.01f;
        pBurnDur = 5f;

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
            roundNum ++;
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
        this.GetComponent<UIManager>().DisplayEnemyHPBar(false,eHP);
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
        return pBATK * pDMGMULT;
    }

    public void SpawnEnemy()
    {
        //Check is spawn is Elite
        if (UnityEngine.Random.Range(0,100) >= 90 && roundNum > 4)
        {
            eHP = (1 + roundNum / 5) * eBHP;
        }
        else
        {
            eHP = (1 + roundNum / 10) * eBHP;
        }
        eBHP = (1 + roundNum / 10) * eBHP;
        this.GetComponent<UIManager>().DisplayEnemyHPBar(true, eHP);
    }
}