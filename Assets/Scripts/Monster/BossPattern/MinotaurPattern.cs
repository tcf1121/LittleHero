using System.Collections;
using UnityEngine;

public class MinotaurPattern : MonoBehaviour, IBossPattern
{
    private Monster _monster;
    private bool _hasExpelled = false;
    private Color _angerColor;

    void Awake()
    {
        _monster = GetComponent<Monster>();
        _angerColor = new Color(1f, 0.4f, 0.4f);
    }

    void Update()
    {

    }

    public void OnSpawn()
    {
        _hasExpelled = false;
        StartCoroutine(MinotaurRushRoutine());
    }

    public void OnDespawn()
    {
        StopAllCoroutines();
    }

    private IEnumerator MinotaurRushRoutine()
    {
        _monster.SetMoveSpeed(1f);
        yield return new WaitForSeconds(4f);
        _monster.SpriteRenderer.color = _angerColor;
        yield return new WaitForSeconds(0.5f);
        _monster.SpriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.2f);

        _monster.SpriteRenderer.color = _angerColor;
        if (_monster != null) _monster.SetMoveSpeed(3f);

        // 충돌 전까지 대기 (OnCollisionEnter2D에서 속도와 색상을 바꿀 것이므로)
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 스크립트가 활성화 상태이고 아직 발동 전일 때만 실행
        if (enabled && !_hasExpelled && collision.gameObject.CompareTag("Player"))
        {
            _hasExpelled = true;

            collision.gameObject.transform.position = new Vector3(-7, 0, 0);

            // 충돌 후 미노타우로스 속도를 1로 변경
            _monster.SetMoveSpeed(1f);
            _monster.SpriteRenderer.color = Color.white;
        }
    }
}
