using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingMove : MonoBehaviour
{
    public bool attacking = true;

    // Move Values
    public float movet = 0f;
    public float moveSpeed = 1f;
    public Vector3 startPoint;
    public Vector3 endPoint;

    public bool isMoveStart = false;
    public void MoveStart(Vector3 start, Vector3 end)
    {
        startPoint = start;
        endPoint = end;

        isMoveStart = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveStart == false)
            return;

        movet += (Time.deltaTime * moveSpeed);

        if (movet >= 1.0f)
        {
            movet = 1.0f;

            Destroy(gameObject);
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
