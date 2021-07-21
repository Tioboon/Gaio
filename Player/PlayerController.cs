using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public Save save;
    public LungsController lungsController;
    public ATPConfig atpConfig;
    public OxygenConfig oxygenConfig;
    //Controls UI icons that display values
    private PlayerUIVariablesController playerUIController;
    //Control Text that display values
    public ATPController atpController;

    public GameObject textGO;
    public EffectController effectController;
    public DotGfxs dotGfxsForVariableChange;

    public GameObject canvasGo;

    public bool dead;

    private void Awake()
    {
        playerUIController = GameObject.Find("PlayerVariablesController").GetComponent<PlayerUIVariablesController>();
        StartCoroutine(FirstATPGetFromOx());
    }

    private void Update()
    {
        if (dead)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("SampleScene");
            }  
        }
    }

    private IEnumerator FirstATPGetFromOx()
    {
        yield return new WaitForSeconds(atpConfig.timeToGenerateFirstAtp);
        StartCoroutine(GetAtpFromOxygen());
    }

    private void Start()
    {
        canvasGo = GameObject.Find("Canvas");
        lungsController.AttGasStats(player.variables[(int)VariableName.oxygen].variableValue, player.variables[(int)VariableName.gases].variableValue, oxygenConfig.maxGas);
    }

    [Serializable]
    public struct ATPConfig
    {
        public float atpGenerated;
        public float oxygenUsed;
        public float timeToGenerateAtp;
        public float timeToGenerateFirstAtp;
    }
    
    [Serializable]
    public struct OxygenConfig
    {
        public float oxygenAbsorb;
        public float gasAbsorb;
        public float maxGas;
    }
    
    private IEnumerator GetAtpFromOxygen()
    {
        yield return new WaitForSeconds(atpConfig.timeToGenerateAtp);
        if (player.variables[(int)VariableName.oxygen].variableValue > 0)
        {
            player.variables[(int)VariableName.oxygen].variableValue -= atpConfig.oxygenUsed;
            player.variables[(int)VariableName.atp].variableValue += atpConfig.atpGenerated;
            lungsController.AttGasStats(player.variables[(int)VariableName.oxygen].variableValue, player.variables[(int)VariableName.gases].variableValue, oxygenConfig.maxGas);
            atpController.ActualizeAtpValue(player.variables[(int)VariableName.atp].variableValue);
            StartCoroutine(GetAtpFromOxygen());
        }
        else
        {
            Die("Morreu pro excesso de O2");
        }
    }

    public void InBreathe()
    {
        player.variables[(int)VariableName.oxygen].variableValue += oxygenConfig.oxygenAbsorb;
        player.variables[(int)VariableName.gases].variableValue += oxygenConfig.gasAbsorb;
        if (player.variables[(int)VariableName.gases].variableValue + player.variables[(int)VariableName.oxygen].variableValue > oxygenConfig.maxGas)
        {
            Die("Morreu sem soltar C02");
        }
        lungsController.AttGasStats(player.variables[(int)VariableName.oxygen].variableValue, player.variables[(int)VariableName.gases].variableValue, oxygenConfig.maxGas);
    }

    public void OutBreathe()
    {
        player.variables[(int)VariableName.gases].variableValue = 0;
        lungsController.AttGasStats(player.variables[(int)VariableName.oxygen].variableValue, player.variables[(int)VariableName.gases].variableValue, oxygenConfig.maxGas);
    }

    public void AttPlayerVars(DotVarChange eventVarChange)
    {
        StartCoroutine(AttPlayerVarsCoroutine(eventVarChange));
    }
    
    private IEnumerator AttPlayerVarsCoroutine(DotVarChange eventVarChange)
    {
        switch (eventVarChange.changeBandVar.boolChange)
        {
            case true:
                player.inBand = eventVarChange.changeBandVar.value;
                break;
        }

        switch (eventVarChange.changeSleepingVar.boolChange)
        {
            case true:
                player.sleeping = eventVarChange.changeSleepingVar.value;
                break;
        }

        for (int i = 0; i < eventVarChange.increaseVariables.Count; i++)
        {
            //Player variable that has the name setted to be changed, is changed to the also setted value.
            yield return new WaitForSeconds(eventVarChange.increaseVariables[i].timeToChangeValue);
            player.variables[(int)eventVarChange.increaseVariables[i].variableName].variableValue += eventVarChange.increaseVariables[i].variableValue;
            //DeathCondition
            switch (player.variables[(int)eventVarChange.increaseVariables[i].variableName].variableValue < 0)
            {
                case true:
                    switch (eventVarChange.increaseVariables[i].variableName)
                    {
                        case VariableName.hungry:
                            Die("Morreu de fome.");
                            break;
                    }
                    break;
            }

            switch (player.variables[(int)eventVarChange.increaseVariables[i].variableName].variableValue > 100)
            {
                case true:
                    switch (eventVarChange.increaseVariables[i].variableName)
                    {
                        case VariableName.lust:
                            Die("Você não conseguiu perpetuar sua espécie.");
                            break;
                        case VariableName.slepiness:
                            Die("Você desmaia e não acorda mais.");
                            break;
                        case VariableName.excrement:
                            Die("Você está tão cheio que não tem mais forças para se mover");
                            break;
                    }
                    break;
            }
        }

        switch (eventVarChange.increaseVariables.Count > 0)
        {
            case true:
                dotGfxsForVariableChange.gfxDotList[0].targetUIElement = ChangeTargetIcon(eventVarChange);
                break;
        }
        switch (dotGfxsForVariableChange.gfxDotList[0].targetUIElement != UIElement.None)
        {
            case true:
                effectController.ReproduceEffect(dotGfxsForVariableChange);
                break;
        }

        playerUIController.AttIcons(player.variables[(int)VariableName.slepiness].variableValue, player.variables[(int)VariableName.lust].variableValue,
            player.variables[(int)VariableName.excrement].variableValue, player.variables[(int)VariableName.hungry].variableValue, oxygenConfig.maxGas);
    }

    private UIElement ChangeTargetIcon(DotVarChange eventVarChange)
    {
        switch (eventVarChange.increaseVariables[0].variableName)
        {
            case VariableName.hungry:
                return UIElement.StomachIcon;
            case VariableName.excrement:
                return UIElement.ExcrementIcon;
            case VariableName.lust:
                return UIElement.LustIcon;
            case VariableName.slepiness:
                return UIElement.SleepIcon;
            default:
                return UIElement.None;
        }
    }

    private void Die(string message)
    {
        var gameObj = Instantiate(textGO);
        var textMeshPro = gameObj.GetComponent<TextMeshProUGUI>();
        var rectTrans = textMeshPro.GetComponent<RectTransform>();
        textMeshPro.text = message;
        textMeshPro.transform.SetParent(canvasGo.transform);
        textMeshPro.gameObject.SetActive(true);
        rectTrans.anchorMin = Vector2.zero;
        rectTrans.anchorMax = Vector2.one;
        rectTrans.offsetMin = Vector2.zero;
        rectTrans.offsetMax = Vector2.zero;
        rectTrans.localScale = Vector3.one;
        gameObj.transform.GetChild(0).gameObject.SetActive(true);
        dead = true;
    }

    
    //Recover save infos
    private void OnEnable()
    {
        player.variables[(int)VariableName.atp].variableValue = PlayerPrefs.GetFloat("atpvalue", 0);
        player.variables[(int)VariableName.excrement].variableValue = PlayerPrefs.GetFloat("excrementvalue", 35);
        player.variables[(int)VariableName.gases].variableValue = PlayerPrefs.GetFloat("gasesvalue", 0);
        player.variables[(int)VariableName.hp].variableValue = PlayerPrefs.GetFloat("hpvalue", 3);
        player.variables[(int) VariableName.hungry].variableValue = PlayerPrefs.GetFloat("hungryvalue", 75);
        player.variables[(int)VariableName.lust].variableValue = PlayerPrefs.GetFloat("lustvalue", 15);
        player.variables[(int)VariableName.oxygen].variableValue = PlayerPrefs.GetFloat("oxygenvalue", 60);
        player.variables[(int)VariableName.slepiness].variableValue = PlayerPrefs.GetFloat("sleepinessvalue", 5);
        switch (PlayerPrefs.GetInt("bandbool", 1) == 1)
        {
            case true:
                player.inBand = true;
                break;
            case false:
                player.inBand = false;
                break;
        }

        switch (PlayerPrefs.GetInt("sleepbool", 0) == 1)
        {
            case true:
                player.sleeping = true;
                break;
            case false:
                player.sleeping = false;
                break;
        }
    }
    
    //Save infos
    private void OnDisable()
    {
        PlayerPrefs.SetFloat("atpvalue", player.variables[(int)VariableName.atp].variableValue);
        PlayerPrefs.SetFloat("excrementvalue", player.variables[(int)VariableName.excrement].variableValue);
        PlayerPrefs.SetFloat("gasesvalue", player.variables[(int)VariableName.gases].variableValue);
        PlayerPrefs.SetFloat("hpvalue", player.variables[(int)VariableName.hp].variableValue);
        PlayerPrefs.SetFloat("hungryvalue", player.variables[(int)VariableName.hungry].variableValue);
        PlayerPrefs.SetFloat("lustvalue", player.variables[(int)VariableName.lust].variableValue);
        PlayerPrefs.SetFloat("oxygenvalue", player.variables[(int)VariableName.oxygen].variableValue);
        PlayerPrefs.SetFloat("sleepinessvalue", player.variables[(int)VariableName.slepiness].variableValue);
        switch (player.inBand)
        {
            case true:
                PlayerPrefs.SetInt("bandbool", 1);
                break;
            case false:
                PlayerPrefs.SetInt("bandbool", 0);
                break;
        }

        switch (player.sleeping)
        {
            case true:
                PlayerPrefs.SetInt("sleepbool", 1);
                break;
            case false:
                PlayerPrefs.SetInt("sleepbool", 0);
                break;
        }
    }
}
