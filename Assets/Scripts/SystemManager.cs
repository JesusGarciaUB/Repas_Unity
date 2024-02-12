using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager
{
    private static SystemManager instance;
    public static SystemManager Instance
    {
        get
        {
            if (instance == null) instance = new SystemManager();
            return instance;
        }
    }

    private const string MoneyKey = "Money";
    private int _money = 0;

    public delegate void MoneyChange(int currentMoney);
    public event MoneyChange OnMoneyChange;

    public int Money
    {
        get => _money;
    }

    public bool ModifyMoney(int value)
    {
        if (Money + value < 0) return false;

        _money += value;
        OnMoneyChange?.Invoke(_money);

        return true;
    }

    private SystemManager() 
    {
        LoadData();
    }

    private void LoadData()
    {
        LoadMoney();
    }

    private void LoadMoney()
    {
        _money = PlayerPrefs.GetInt(MoneyKey, 0);
    }
}
