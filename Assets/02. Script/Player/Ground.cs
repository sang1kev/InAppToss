using System.Collections;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private PlayerCtrl playerCtrl;
    
    private Animator groundAnim;

    void Start()
    {
        groundAnim = GetComponent<Animator>();
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (!playerCtrl.DidPlayerExit)
        {
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(GroundCollapse());
        }
    }

    IEnumerator GroundCollapse()
    {
        yield return new WaitForSeconds(2.25f);

        groundAnim.SetTrigger("Broke");

        yield return new WaitForSeconds(2.25f);
        groundAnim.gameObject.SetActive(false);
    }
}
