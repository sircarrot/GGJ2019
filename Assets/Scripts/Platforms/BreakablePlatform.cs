using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour {

    [SerializeField] private float secondsToDestroy = 3f;
    [SerializeField] private float destroyAnimationDelay = 2f;
    [SerializeField] private float repairAnimationDelay = 2f;

    private BoxCollider2D boxCollider2D;
    private IEnumerator coroutine = null;
    private bool coroutineRunning = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (coroutineRunning) { return; }
        if (collision.gameObject.tag != "Player") { return; }

        if (boxCollider2D == null) { boxCollider2D = GetComponent<BoxCollider2D>(); } 

        Debug.Log("Breaking Object: " + gameObject.name);
        coroutine = BreakAndRestorePlatform();
        StartCoroutine(coroutine);
    }

    private IEnumerator BreakAndRestorePlatform()
    {
        if (coroutineRunning) { yield break; }
        coroutineRunning = true;

        yield return new WaitForSeconds(secondsToDestroy);

        boxCollider2D.enabled = false;

        yield return new WaitForSeconds(destroyAnimationDelay);


        yield return new WaitForSeconds(repairAnimationDelay);

        boxCollider2D.enabled = true;
        coroutineRunning = false;
    }

}
