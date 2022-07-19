using System.Collections;
using System.Collections.Generic;
using DesignPatterns.Singleton;
using Systems.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

public class StageController : Singleton<StageController>, ISaveable
{
    [Header("Component References")]
    [SerializeField] private Transform _stageRowParent;
    [SerializeField] private StageRowBehaviour _stageRowPrefab;
    [SerializeField] private Button _resetButton;

    [Header("Stage settings")]
    [SerializeField] private int _stageCount = 999;

    public int StageCount => _stageCount;

    [SerializeField] private IntVariable _stageStarCount;

    private int _instantiatedStagesCount;
    private int _lastUnlockedStage = 0;
    public List<int> StagesStar { get; private set; } = new List<int>();

    private static ObjectPool<StageRowBehaviour> _stageRowPool;

    public int InstantiatedStagesCount
    {
        get => _instantiatedStagesCount;
        set => _instantiatedStagesCount = value;
    }

    public int LastUnlockedStage => _lastUnlockedStage;

    private void Start()
    {
        //loading goes here
        SaveManager.Instance.Load();
        
        _stageStarCount.Value = 0;

        if (_lastUnlockedStage == 0)
            _lastUnlockedStage = Random.Range(1, 1000);

        _stageRowPool = new ObjectPool<StageRowBehaviour>(_stageRowPrefab);
        
        StartCoroutine(SpawnStageButtons());
    }

    private IEnumerator SpawnStageButtons()
    {
        _resetButton.interactable = false;
        
        int rows = Mathf.CeilToInt(_stageCount / 4f);
        for (int i = 1; i <= rows; i++)
        {
            if (i % 6 == 0) yield return null; //time slicing so not everything runs in 1 frame

            //right alignment is i is even
            StageRowAlignment alignment = i % 2 == 0 ? StageRowAlignment.Right : StageRowAlignment.Left;
            var stageRow = _stageRowPool.Pull(_stageRowParent);
            stageRow.Init(Mathf.Min(_stageCount - _instantiatedStagesCount, 4), alignment);
        }

        SaveManager.Instance.Save();

        _resetButton.interactable = true;
    }

    public void ResetStageButtons()
    {
        foreach (Transform child in _stageRowParent)
        {
            child.gameObject.SetActive(false);
        }
        
        StagesStar.Clear();
        _stageStarCount.Value = 0;
        _instantiatedStagesCount = 0;
        
        _lastUnlockedStage = Random.Range(1, 1000);

        StartCoroutine(SpawnStageButtons());
    }

    public object SaveState()
    {
        return new SaveData()
        {
            lastUnlockedStage = _lastUnlockedStage,
            stagesStar = StagesStar
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData) state;
        _lastUnlockedStage = saveData.lastUnlockedStage;
        StagesStar = saveData.stagesStar;
    }
    
    /// <summary>
    /// This struct is used to define what to save
    /// </summary>
    [System.Serializable]
    private struct SaveData
    {
        public int lastUnlockedStage;
        public List<int> stagesStar;
    }
}