using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThunderFlash", menuName = "Skills/ThunderFlash")]
public class Skill_ThunderFlash : SkillBase
{
    private Vector3 rangePos = new Vector3(0, 1f, 0);
    private Vector3 rangescale = new Vector3(50f, 0.1f, 0);
    [SerializeField] AudioClip _readySound;

    public override IEnumerator Execute(PlayerController playerC)
    {
        Vector3 originPos = playerC.transform.position;

        Time.timeScale = 0f;

        // 대기
        playerC.GetOverlay().SetActive(true);
        SoundManager.Instance.PlaySFX(_readySound); // 번개 충전 소리
        playerC.transform.position = new Vector3(-7f, 0f, 0f);

        // 기 모으기
        yield return new WaitForSecondsRealtime(0.4f);
        playerC.SkillRange.transform.localPosition = rangePos;
        playerC.SkillRange.transform.localScale = rangescale;
        playerC.SkillRange.SetActive(true);

        // 돌진
        yield return new WaitForSecondsRealtime(0.4f);
        playerC.transform.position = new Vector3(7f, 0f, 0f);
        SoundManager.Instance.PlaySFX(ActivationSound);

        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1f;
        playerC.transform.position = originPos;
        playerC.SkillRange.SetActive(false);
        playerC.GetOverlay().SetActive(false);
        playerC.ShakeCam(Vector3.right, 1f);

        // 맵 전체의 몬스터에게 데미지
        List<Monster> targets = new List<Monster>(MonsterRegistry.ColliderMonster.Values);
        foreach (Monster target in targets)
        {
            target.GetDamage(playerC.Player.PlayerStats.Damage * (int)DamageMultiplier);
        }


    }
}
