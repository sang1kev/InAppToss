using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private Vector3 minBoundary = new Vector3(-2f, 0f, 0f);
    [SerializeField] private Vector3 maxBoundary = new Vector3(2f, 0f, 0f);


    [SerializeField] private float damp = 10;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        Vector3 dest = target.position + offset;

        if (target.position.y > minBoundary.y)
        {
            minBoundary.y = target.position.y;
        }

        maxBoundary.y = target.position.y + 10f;

        Vector3 smoothPos = Vector3.Lerp(transform.position, dest, damp * Time.deltaTime);

        smoothPos.x = Mathf.Clamp(smoothPos.x, minBoundary.x, maxBoundary.x);
        smoothPos.y = Mathf.Clamp(smoothPos.y, minBoundary.y, maxBoundary.y);

        transform.position = smoothPos;
    }
}
