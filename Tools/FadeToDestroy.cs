using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToDestroy : MonoBehaviour
{
    public void DestroyAfterXSec(float sec)
    {
        StartCoroutine(DestroyAfterXSecCorout(sec));
    }

    private IEnumerator DestroyAfterXSecCorout(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }
}
