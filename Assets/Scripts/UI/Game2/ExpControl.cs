using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpControl : MonoBehaviour
{
    public PlayController playController;
    public TextMeshProUGUI expText;
    public Slider expSlider;
    

    private void Awake()
    {
        playController = FindObjectOfType<PlayController>();
    }

    private void Update()
    {
        if (!playController) return;
        float currentExp = playController.currentExp;
        float nextLevelExp = playController.currentMaxExp;
        expText.text = $"{currentExp} / {nextLevelExp}";
        expSlider.value = (float)currentExp / nextLevelExp;
    }
}
