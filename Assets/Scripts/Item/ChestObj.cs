using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
