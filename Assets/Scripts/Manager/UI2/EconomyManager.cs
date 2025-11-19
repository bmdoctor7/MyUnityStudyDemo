using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EconomyManager : SingletonMonoBase<EconomyManager>
{
    private EconomyManager(){}
    
    public TextMeshProUGUI coinText;
    public int coins = 200;
    
    
    private void Update()
    {
        coinText.text = coins.ToString();
    }

    public bool CanAfford(int cost) => coins >= cost; //检查是否有足够的金币支付费用
    
    // 尝试花费金币（创造建筑），成功则返回true，否则返回false
    public bool Spend(int cost)
    {
        if (!CanAfford(cost)) return false;
        coins -= cost;
        return true;
    }
    
    
    
}
