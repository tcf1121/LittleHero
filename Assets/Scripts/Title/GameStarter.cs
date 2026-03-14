using UnityEngine;
using UnityEngine.SceneManagement;

// 타이틀 화면에서 실행되는 클래스
public class GameStarter : MonoBehaviour
{
    [SerializeField] private AudioClip _titleBGM;

    void Start()
    {
        // 배경음 재생
        SoundManager.Instance.PlayBGM(_titleBGM);
    }

    // 아무 키나 누르면 로비로 이동
    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(1);
        }
    }
}
