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

    private int _stageNumber;
    
    public void Init(bool isTutorial = false)
    {
        _stageNumber = StageController.Instance.InstantiatedStagesCount;
        _stageNumberText.text = _stageNumber.ToString();
        
        if (isTutorial)
        {
            Instantiate(_tutorialSpriteGameObject, transform.GetChild(0));
            _webOverlay.SetActive(false);
            _button.interactable = true;
            _stageNumberText.enabled = false;
        }
        else if (_stageNumber <= StageController.Instance.LastUnlockedStage)
        {
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
}
