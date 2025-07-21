using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor.Experimental.GraphView;

public class JoyStickUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private PlayerFSM pCtrl;

    [SerializeField] private GameObject backgroundUI;
    [SerializeField] private GameObject handlerUI;

    private Vector3 startPos, currPos;

    void Start()
    {
        pCtrl = FindFirstObjectByType<PlayerFSM>();
        backgroundUI.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currPos = eventData.position;
        Vector3 dragDir = currPos - startPos;

        float maxDist = Mathf.Min(dragDir.magnitude, 95f);

        handlerUI.transform.position = startPos + dragDir.normalized * maxDist;

        pCtrl.InputJoyStick(dragDir.normalized.x, dragDir.normalized.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        backgroundUI.SetActive(true);
        backgroundUI.transform.position = eventData.position;
        startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handlerUI.transform.localPosition = Vector3.zero;
        pCtrl.InputJoyStick(0, 0);
        backgroundUI.SetActive(false);
    }
}
