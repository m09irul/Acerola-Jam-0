using UnityEngine;

public class Obstacle3 : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 1.0f;
    private bool movingToB = true;

    void Start()
    {
        transform.position = pointA;

        MoveToB();
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
