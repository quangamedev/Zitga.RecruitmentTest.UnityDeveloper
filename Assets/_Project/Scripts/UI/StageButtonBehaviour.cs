using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StageButtonBehaviour : MonoBehaviour, IPoolable<StageButtonBehaviour>
{
    [SerializeField] private GameObject _webOverlay;
    [SerializeField] private TextMeshProUGUI _stageNumberText;
    [SerializeField] private Button _button;
    [SerializeField] private Transform _starsHolder;
    [SerializeField] private IntVariable _stageStarCount;
    [SerializeField] private PoolObject _tutorialSpriteGameObject;
    [SerializeField] private PoolObject _verticalLine;

    private int _stageNumber;
    private static ObjectPool<PoolObject> _verticalLinePool;
    private static ObjectPool<PoolObject> _tutorialSpritePool;

    private void Awake()
    {
        _verticalLinePool ??= new ObjectPool<PoolObject>(_verticalLine);
        _tutorialSpritePool ??= new ObjectPool<PoolObject>(_tutorialSpriteGameObject);
    }

    public void Init(bool isTutorial = false, bool connectUpward = false)
    {
        //reset values
        _stageNumber = StageController.Instance.InstantiatedStagesCount;
        _stageNumberText.text = _stageNumber.ToString();
        _webOverlay.SetActive(true);
        _button.interactable = false;
        _stageNumberText.enabled = true;
        for (int i = 0; i < 3; i++)
        {
            _starsHolder.GetChild(i).gameObject.SetActive(false);
        }
        

        if (connectUpward)
        {
            Instantiate(_verticalLine, transform).transform.SetSiblingIndex(0);
        }
        
        if (isTutorial)
        {
            Instantiate(_tutorialSpriteGameObject, transform.GetChild(0));
            _stageNumberText.enabled = false;
        }

        if (_stageNumber > StageController.Instance.LastUnlockedStage) return;
        
        _webOverlay.SetActive(false);
        _button.interactable = true;

        if (_stageNumber == StageController.Instance.LastUnlockedStage) return;

        int rand = Random.Range(0, 3);

        for (int i = 0; i <= rand; i++)
        {
            _starsHolder.GetChild(i).gameObject.SetActive(true);
            _stageStarCount.Value++;
        }
    }

    private void OnDisable()
    {
        ReturnToPool();
    }

    private Action<StageButtonBehaviour> returnToPool;

    public void Initialize(Action<StageButtonBehaviour> returnAction)
    {
        returnToPool = returnAction;
    }

    public void ReturnToPool()
    {
        returnToPool?.Invoke(this);
    }
}
