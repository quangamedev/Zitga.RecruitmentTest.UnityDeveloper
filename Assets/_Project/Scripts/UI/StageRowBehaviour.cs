using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageRowBehaviour : MonoBehaviour, IPoolable<StageRowBehaviour>
{
    [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;
    [SerializeField] private StageButtonBehaviour _stageButtonPrefab;

    private ObjectPool<StageButtonBehaviour> _stageButtonPool;

    private void Awake()
    {
        _stageButtonPool = new ObjectPool<StageButtonBehaviour>(_stageButtonPrefab);
    }

    public void Init(int stageCount, StageRowAlignment stageRowAlignment = StageRowAlignment.Left)
    {
        if (stageRowAlignment == StageRowAlignment.Right)
        {
            _horizontalLayoutGroup.childAlignment = TextAnchor.MiddleRight;
            _horizontalLayoutGroup.reverseArrangement = true;
        }

        for (int i = 0; i < stageCount; i++)
        {
            var stageButton = _stageButtonPool.Pull(transform);
            
            StageController.Instance.InstantiatedStagesCount++;
            
            if (i == 3)
            {
                stageButton.Init(connectUpward:true);
                return;
            }
            
            stageButton.Init(StageController.Instance.InstantiatedStagesCount == 1);
        }
    }

    private void OnDisable()
    {
        ReturnToPool();
    }

    private Action<StageRowBehaviour> returnToPool;

    public void Initialize(Action<StageRowBehaviour> returnAction)
    {
        returnToPool = returnAction;
    }

    public void ReturnToPool()
    {
        returnToPool?.Invoke(this);
    }
}

public enum StageRowAlignment
{
    Left,
    Right
}
