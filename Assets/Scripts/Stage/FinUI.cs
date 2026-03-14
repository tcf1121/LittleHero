using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 스테이지가 끝났을 때 UI
public class FinUI : MonoBehaviour
{
    [SerializeField] private GameObject _clearUI;
    [SerializeField] private GameObject _overUI;
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _retryBtn;
    [SerializeField] private Button _homeBtn;

    // Start is called before the first frame update
    void Start()
    {
        _nextBtn.onClick.AddListener(GoIngameScene);
        _retryBtn.onClick.AddListener(GoIngameScene);
        _homeBtn.onClick.AddListener(GoHome);
    }


    public void SetFin(bool clear)
    {
        _clearUI.SetActive(clear);
        _overUI.SetActive(!clear);
    }

    private void GoIngameScene()
    {
        SceneManager.LoadScene(2);
    }

    private void GoHome()
    {
        SceneManager.LoadScene(1);
    }
}
