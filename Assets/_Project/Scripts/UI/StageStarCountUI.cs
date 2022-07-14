using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageStarCountUI : MonoBehaviour
{
    [SerializeField] private IntVariable _starCount;
    [SerializeField] private TextMeshProUGUI _starCountText;

    private void Start()
    {
        _starCountText.text = _starCount.Value.ToString();
    }

    public void OnStarCountUpdated()
    {
        _starCountText.text = _starCount.Value.ToString();
    }
}
