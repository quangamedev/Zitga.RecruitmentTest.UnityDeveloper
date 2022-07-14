using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StageButtonBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialSpriteGameObject;
    [SerializeField] private GameObject _webOverlay;
    [SerializeField] private TextMeshProUGUI _stageNumberText;
    [SerializeField] private Button _button;
    [SerializeField] private Transform _starsHolder;
    [SerializeField] private IntVariable _stageStarCount;
    [SerializeField] private GameObject _verticalLine;

    private int _stageNumber;
    
    public void Init(bool isTutorial = false, bool connectUpward = false)
    {
        _stageNumber = StageController.Instance.InstantiatedStagesCount;
        _stageNumberText.text = _stageNumber.ToString();

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
}
