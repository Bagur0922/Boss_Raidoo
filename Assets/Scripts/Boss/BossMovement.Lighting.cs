using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� 22-12-27
// BossMovementŬ���� partial �� ��
public partial class BossMovement : MonoBehaviour
{
    [Header("BossMovement.Lighting")]
    // ����� Prefab
    public GameObject prefab_Lighting_Big;
    public GameObject prefab_Lighting_Small;
    public Transform LightingSpawnPos;

    // ���� 22-12-27
    // BossMovementŬ������ �ִ����� �̰����� �̰�
    void SpecialSkillStart()
    {
        // ����������� ������ �̶�� return
        if (isPlayspecialSkill)
            return;

        stopUpdate = true;
        specialSkillUsed = true;
        hitBoxCol.enabled = false;

        // ����� �ڷ�ƾ ���� üũ
        isPlayspecialSkill = true;
        StartCoroutine(SpecialSkill());
    }

    bool isPlayspecialSkill = false;
    public IEnumerator SpecialSkill()
    {
        // ������ ���� ���ð� 1��
        anim.SetBool("isWalking", true);
        yield return new WaitForSeconds(1f);

        // ���߿� �ߴ°��� ����� ������ �� �ֵ��� ����
        ForceMoveInit(transform.position, new Vector3(0f, transform.position.y, transform.position.z), 2f);
        // �����Ҷ����� ���

        yield return new WaitUntil(() => isForceMoveStart == false);

        anim.SetTrigger("specialStart");
        anim.SetBool("Force", true);
        anim.SetBool("isWalking", false);
        // ������ ���� ���ð� 1��
        yield return new WaitForSeconds(1f);

        // ���� ū ����� ��ȯ
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

        // ū ������� ����������� ���
        yield return new WaitUntil(() => big == null);

        // �����ö����� ���
        anim.SetTrigger("specialEnd");
        yield return new WaitForSeconds(2f);

        // ����
        stopUpdate = false;
        hitBoxCol.enabled = true;
        anim.SetBool("Force", false);
    }
}
