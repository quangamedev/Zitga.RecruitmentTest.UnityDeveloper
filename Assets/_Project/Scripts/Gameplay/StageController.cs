using System.Collections;
using DesignPatterns.Singleton;
using UnityEngine;

public class StageController : Singleton<StageController>
{
    [Header("Component References")]
    [SerializeField] private Transform _stageRowParent;

    [SerializeField] private StageRowBehaviour _stageRowPrefab;

    [Header("Stage settings")]
    [SerializeField] private int _stageCount = 999;

    [SerializeField] private IntVariable _stageStarCount;

    private int _instantiatedStagesCount;
    private int _lastUnlockedStage;

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
            var stageRow = Instantiate(_stageRowPrefab, _stageRowParent);
            stageRow.Init(Mathf.Min(_stageCount - _instantiatedStagesCount, 4), alignment);
        }
    }
}