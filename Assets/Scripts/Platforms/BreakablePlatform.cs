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
        if (coroutineRunning) { return; } // Check if coroutine is running
        if (collision.gameObject.tag != "Player") { return; } // Check if player is colliding

        // Check contact point
        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 center = collision.collider.bounds.center;
        bool bottom = contactPoint.y > center.y;
        if (bottom) { return; }

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

        // Play destroy animation
        yield return new WaitForSeconds(destroyAnimationDelay);

        // Play repair animation
        yield return new WaitForSeconds(repairAnimationDelay);

        boxCollider2D.enabled = true;
        coroutineRunning = false;
    }

}
