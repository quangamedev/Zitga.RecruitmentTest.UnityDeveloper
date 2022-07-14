using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageRowBehaviour : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;
    [SerializeField] private StageButtonBehaviour _stageButtonPrefab;

    public void Init(int stageCount, StageRowAlignment stageRowAlignment = StageRowAlignment.Left)
    {
        if (stageRowAlignment == StageRowAlignment.Right)
        {
            _horizontalLayoutGroup.childAlignment = TextAnchor.MiddleRight;
            _horizontalLayoutGroup.reverseArrangement = true;
        }

        for (int i = 0; i < stageCount; i++)
        {
            var stageButton = Instantiate(_stageButtonPrefab, transform);
            
            StageController.Instance.InstantiatedStagesCount++;
            
            if (i == 3)
            {
                stageButton.Init(connectUpward:true);
                return;
            }
            
            stageButton.Init(StageController.Instance.InstantiatedStagesCount == 1);
        }
    }
}

public enum StageRowAlignment
{
    Left,
    Right
}
