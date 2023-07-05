using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 场景切换过度界面
/// </summary>
public class UISceneSwitch : UIWindowBase
{
    public Slider progressSlider;
    public Text progressPercent;

    public void Init()
    {
        progressSlider.value = 0;
        progressPercent.text = "0%";
    }

    public void UpdateProgress(float progress)
    {
        progressSlider.value = progress;
        progressPercent.text = $"{progress * 100}%";
    }
}
