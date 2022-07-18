using System.Collections;
using DesignPatterns.Singleton;
using UnityEngine;
using UnityEngine.UI;

public class StageController : Singleton<StageController>
{
    [Header("Component References")]
    [SerializeField] private Transform _stageRowParent;

    [SerializeField] private StageRowBehaviour _stageRowPrefab;

    [SerializeField] private Button _resetButton;

    [Header("Stage settings")]
    [SerializeField] private int _stageCount = 999;

    [SerializeField] private IntVariable _stageStarCount;

    private int _instantiatedStagesCount;
    private int _lastUnlockedStage;
    private static ObjectPool<StageRowBehaviour> _stageRowPool;

    public int InstantiatedStagesCount
    {
        get => _instantiatedStagesCount;
        set => _instantiatedStagesCount = value;
    }

    public int LastUnlockedStage => _lastUnlockedStage;

    private void Start()
    {
        _stageStarCount.Value = 0;
        
        _lastUnlockedStage = Random.Range(1, 1000);

        _stageRowPool = new ObjectPool<StageRowBehaviour>(_stageRowPrefab);
        
        StartCoroutine(SpawnStageButtons());
    }

    private IEnumerator SpawnStageButtons()
    {
        int rows = Mathf.CeilToInt(_stageCount / 4f);
        for (int i = 1; i <= rows; i++)
        {
            if (i % 6 == 0) yield return null; //time slicing so not everything runs in 1 frame

            //right alignment is i is even
            StageRowAlignment alignment = i % 2 == 0 ? StageRowAlignment.Right : StageRowAlignment.Left;
            var stageRow = _stageRowPool.Pull(_stageRowParent);
            stageRow.Init(Mathf.Min(_stageCount - _instantiatedStagesCount, 4), alignment);
        }

        _resetButton.interactable = true;
    }

    public void ResetStageButtons()
    {
        _resetButton.interactable = false;
        
        foreach (Transform child in _stageRowParent)
        {
            child.gameObject.SetActive(false);
        }
        
        _stageStarCount.Value = 0; 
        _instantiatedStagesCount = 0;
        
        _lastUnlockedStage = Random.Range(1, 1000);

        StartCoroutine(SpawnStageButtons());
    }
}