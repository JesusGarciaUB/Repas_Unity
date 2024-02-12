using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private const string MoneyLabelTranslationKey = "money_label";

    private void OnEnable()
    {
        SystemManager sm = SystemManager.Instance;
        UpdateTextWithCurrentMoney(sm.Money);
        sm.OnMoneyChange += UpdateTextWithCurrentMoney;
    }
    private void UpdateTextWithCurrentMoney(int money)
    {
        //string text = TranslationManager.GetString(MoneyLabelTranslationKey, money.ToString());
        string text = "Money: " + money.ToString();
        _text.text = text;
    }

    private void OnDisable()
    {
        SystemManager sm = SystemManager.Instance;
        sm.OnMoneyChange -= UpdateTextWithCurrentMoney;
    }
}
