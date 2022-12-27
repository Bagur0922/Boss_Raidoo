using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 깻잎 22-12-27
// BossMovement클래스 partial 로 분
public partial class BossMovement : MonoBehaviour
{
    [Header("BossMovement.Lighting")]
    // 원기옥 Prefab
    public GameObject prefab_Lighting_Big;
    public GameObject prefab_Lighting_Small;
    public Transform LightingSpawnPos;

    // 깻잎 22-12-27
    // BossMovement클래스에 있던것을 이곳으로 이관
    void SpecialSkillStart()
    {
        // 원기옥패턴이 진행중 이라면 return
        if (isPlayspecialSkill)
            return;

        stopUpdate = true;
        specialSkillUsed = true;
        hitBoxCol.enabled = false;

        // 원기옥 코루틴 시작 체크
        isPlayspecialSkill = true;
        StartCoroutine(SpecialSkill());
    }

    bool isPlayspecialSkill = false;
    public IEnumerator SpecialSkill()
    {
        // 떠오를 동안 대기시간 1초
        anim.SetBool("isWalking", true);
        yield return new WaitForSeconds(1f);

        // 공중에 뜨는것을 가운데서 시작할 수 있도록 수정
        float walk_time_calc = 0.2f * Mathf.Abs(transform.position.x); // 위치에 따른 걸어가는 시간 비례 계산
        ForceMoveInit(transform.position, new Vector3(0f, transform.position.y, transform.position.z), walk_time_calc);
        // 도착할때까지 대기

        yield return new WaitUntil(() => isForceMoveStart == false);

        anim.SetTrigger("specialStart");
        anim.SetBool("Force", true);
        anim.SetBool("isWalking", false);
        // 떠오를 동안 대기시간 1초
        yield return new WaitForSeconds(1f);

        // 가장 큰 원기옥 소환
        CameraShake.I.DoShake(2f);
        GameObject big = Instantiate(prefab_Lighting_Big);
        big.transform.position = ConstantValue.lighting_big_origin_pos;

        yield return new WaitForSeconds(2f);

        for (int a = 0; a < 15; a++)
        {
            GameObject small = Instantiate(prefab_Lighting_Small);

            small.transform.position = big.transform.position; 

            LightingMove script = small.GetComponent<LightingMove>();
            Vector3 calcEndPos = new Vector3(
                ConstantValue.lighting_small_end_pos.x + UnityEngine.Random.Range(-ConstantValue.lighting_small_witdh_x, +ConstantValue.lighting_small_witdh_x),
                ConstantValue.lighting_small_end_pos.y,
                ConstantValue.lighting_small_end_pos.z);
            script.MoveStart(big.transform.position, calcEndPos);

            yield return new WaitForSeconds(1f);
        }
        anim.SetTrigger("specialHandMotion");
        yield return new WaitForSeconds(1.0f);

        Vector3 BigcalcEndPos = new Vector3(
                player.transform.position.x,
                ConstantValue.lighting_small_end_pos.y,
                player.transform.position.z);
        LightingMove script_big = big.GetComponent<LightingMove>();
        script_big.MoveStart(big.transform.position, BigcalcEndPos);

        // 큰 원기옥이 사라질때까지 대기
        yield return new WaitUntil(() => big == null);

        // 내려올때까지 대기
        anim.SetTrigger("specialEnd");
        yield return new WaitForSeconds(2f);

        // 종료
        stopUpdate = false;
        hitBoxCol.enabled = true;
        anim.SetBool("Force", false);
    }
}
