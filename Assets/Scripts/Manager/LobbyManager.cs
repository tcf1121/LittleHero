using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;
    [SerializeField] private Button _gameStartBtn;
    [SerializeField] private TMP_Text _stageNum;

    void Start()
    {
        _gameStartBtn.onClick.AddListener(GoIngameScene);
        _stageNum.text = GameManager.Instance.CurrentStage + 1 + "F";
    }

    private void GoIngameScene()
    {
        SceneManager.LoadScene(2);
    }

}
