using UnityEngine;
using static Pathfinding.Util.RetainedGizmos;

public class Obstacle4 : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 1.0f;
    public float speedUpFactor = 2.0f;
    public float detectionRange = 10.0f;
    Vector2 dst;
    private bool movingToB = true;
    public LayerMask mask;
    bool dashed = false;
    void Start()
    {
        transform.position = pointA;

        MoveToB();
    }

    void Update()
    {
        if (movingToB)
            dst = (pointB - pointA).normalized;
        else
            dst = (pointA - pointB).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dst, detectionRange, mask);

        if (hit && !dashed)
        {
            dashed = true;
            LeanTween.cancel(gameObject);
            LeanTween.move(gameObject, movingToB ? pointB : pointA, Vector3.Distance(transform.position, movingToB ? pointB : pointA) / (speed * speedUpFactor)).setDelay(0.4f).setOnComplete(SwitchDirection);
        }
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(transform.position, dst * detectionRange, color);
    }


    void MoveToB()
    {
        LeanTween.move(gameObject, pointB, Vector3.Distance(transform.position, pointB) / speed)
        .setOnComplete(SwitchDirection);
    }

    void MoveToA()
    {
        LeanTween.move(gameObject, pointA, Vector3.Distance(transform.position, pointA) / speed)
        .setOnComplete(SwitchDirection);
    }

    void SwitchDirection()
    {
        dashed = false;
        movingToB = !movingToB;

        if (movingToB)
        {
            MoveToB();
        }
        else
        {
            MoveToA();
        }
    }

}
