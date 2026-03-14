using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestObj : MonoBehaviour
{
    private Coroutine _disappearCor;

    void OnEnable()
    {
        _disappearCor = null;
    }

    void OnDisable()
    {
        StopCoroutine(_disappearCor);
    }

    private IEnumerator DisappearCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        ChestPool.Instance.ReturnToPool(gameObject);
    }
}
