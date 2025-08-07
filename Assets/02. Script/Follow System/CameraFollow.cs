using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D targetRb;
    private Camera cam;

    #region Camera Follow Setting
    private Vector3 baseOffset = new Vector3(0, 0, -10);
    private Vector3 minBoundary = new Vector3(-2f, 0f, 0f);
    private Vector3 maxBoundary = new Vector3(2f, 0f, 0f);

    private float damp = 10;

    #endregion

    #region Camera zoom Setting
    private float baseSize = 5f;       
    private float maxSize = 6.5f;        
    private float targetSize;
    private float zoomSpeed = 3f;      
    private float velZoomThreshold = 5f;

    #endregion

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetRb = target.GetComponent<Rigidbody2D>();
        cam = GetComponent<Camera>();
        targetSize = baseSize;
    }

    /// <summary>
    /// �ӵ��� ���� ���� ���� ����
    /// </summary>
    void LateUpdate()
    {
        ZoomUpdate();

        Vector3 targetPos = target.position + baseOffset;

        if (targetPos.y > minBoundary.y)
        {
            minBoundary.y = targetPos.y;
        }

        maxBoundary.y = target.position.y + 10f;

        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, damp * Time.deltaTime);

        smoothPos.x = Mathf.Clamp(smoothPos.x, minBoundary.x, maxBoundary.x);
        smoothPos.y = Mathf.Clamp(smoothPos.y, minBoundary.y + baseOffset.y, maxBoundary.y);

        transform.position = smoothPos;
    }

    private void ZoomUpdate()
    {
        float velY = targetRb.linearVelocity.y;

        if (velY > velZoomThreshold)
        {
            targetSize = maxSize;
        }
        else if (velY < -velZoomThreshold)
        {
            targetSize = baseSize;
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);
    }
}
