using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

public class JoyStickUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private PlayerCtrl playerCtrl;

    [SerializeField] private GameObject backgroundUI;
    [SerializeField] private GameObject handlerUI;

    [SerializeField] private float maxDist = 95f;

    private Vector3 startPos, currPos, playerDir;

    void Start()
    {
        playerCtrl = FindFirstObjectByType<PlayerCtrl>();
        backgroundUI.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currPos = eventData.position;
        Vector3 dragDir = (currPos - startPos);
    
        float distance = Mathf.Min(dragDir.magnitude, maxDist);

        handlerUI.transform.position = startPos + dragDir.normalized * distance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        backgroundUI.SetActive(true);
        backgroundUI.transform.position = eventData.position;
        startPos = eventData.position;
        playerDir = Vector3.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector3 finalPos = eventData.position;
        Vector3 finalDir = (finalPos - startPos);

        float distance = Mathf.Min(finalDir.magnitude, maxDist);
        float movePower = distance / maxDist;

        int invDir = -1;
        Vector3 playerDir = invDir * movePower * finalDir.normalized;

        playerCtrl.InputJoyStick(playerDir.x, playerDir.y);

        handlerUI.transform.localPosition = Vector3.zero;
        backgroundUI.SetActive(false);

        startPos = Vector3.zero;
        currPos = Vector3.zero;
    }
}
