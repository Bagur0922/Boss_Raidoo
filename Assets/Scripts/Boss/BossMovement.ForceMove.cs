using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// BossMovement.ForceMove
public partial class BossMovement : MonoBehaviour
{
    // 깻잎 22-12-27
    // 연출을 위한 강제로 움직이기 변수
    [Header("BossMovement.ForceMove")]
    public float movet = 0f;
    public float moveSpeed = 1f;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public bool isForceMoveStart = false;

    public void ForceMoveInit(Vector3 start, Vector3 end, float time)
    {
        startPoint = start;
        endPoint = end;
        moveSpeed = 1f / time;
        isForceMoveStart = true;

        if (start.x - end.x > 0 && changedir)
        {
            Debug.Log("force 왼쪽");
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            direction = false;
        }
        else if(changedir)
        {
            Debug.Log("force 오른쪽");
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            direction = true;
        }
    }
    public void ForceMove()
    {
        if (isForceMoveStart == false)
            return;

        movet += (Time.deltaTime * moveSpeed);

        if (movet >= 1.0f)
        {
            movet = 1.0f;
            isForceMoveStart = false;
            return;
        }
        else
        {
            transform.position = GetPoint(startPoint, endPoint, movet);

#if UNITY_EDITOR
            Debug.DrawLine(transform.position, endPoint, Color.red);
#endif
        }
    }

    public static Vector3 GetPoint(Vector3 a, Vector3 b, float t)
    {
        return (1f - t) * a + t * b;
    }
}
