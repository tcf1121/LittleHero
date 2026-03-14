using UnityEngine;


// 플레이어가 데미지를 입었을 경우 나오는 UI
public class DamageUI : MonoBehaviour
{
    [SerializeField] private Player player;

    // 패링 버튼을 누를 때 까지 멈춤
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Resume();
        }
    }

    // 나오면 일시 정지 상태와 같아짐
    void OnEnable()
    {
        Time.timeScale = 0f;
    }

    // 사라지면서 다시 시작되며 패링을 한다.
    void OnDisable()
    {
        Time.timeScale = 1f;
        player.PlayerController.Parrying();
    }

    // 버튼을 누르면 UI가 사라짐
    private void Resume()
    {
        gameObject.SetActive(false);
    }


}
