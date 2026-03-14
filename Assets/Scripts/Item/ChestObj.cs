using System.Collections;
using UnityEngine;


// 상자 오브젝트에 관련된 코드
public class ChestObj : MonoBehaviour
{
    private Coroutine _disappearCor;
    private bool _spwan;
    void Awake()
    {
        _spwan = false;
    }


    void OnEnable()
    {
        if (_spwan)
            _disappearCor = StartCoroutine(DisappearCoroutine());
    }

    // 오브젝트 풀링으로 생성할 때는 코루틴이 실행되지 않게
    void OnDisable()
    {
        if (!_spwan) _spwan = true;
        if (_disappearCor != null)
            StopCoroutine(_disappearCor);
    }

    private IEnumerator DisappearCoroutine()
    {
        InGameManager.Instance.DropChest();
        yield return new WaitForSeconds(1.5f);
        ChestPool.Instance.ReturnToPool(gameObject);
    }
}
