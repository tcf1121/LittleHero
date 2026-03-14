using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 로비 관리
public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;
    [SerializeField] private AudioClip _lobbyBGM;
    [SerializeField] private Button _gameStartBtn;
    [SerializeField] private TMP_Text _stageNum;

    void Start()
    {
        // 배경음 재생
        SoundManager.Instance.PlayBGM(_lobbyBGM);
        _gameStartBtn.onClick.AddListener(GoIngameScene);
        _stageNum.text = GameManager.Instance.CurrentStage + 1 + "F";
    }

    private void GoIngameScene()
    {
        SceneManager.LoadScene(2);
    }

}
