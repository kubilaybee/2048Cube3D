using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(AutoDestroy());
    }

    public IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(1.1f);
        Destroy(this.gameObject);
    }
  
}
