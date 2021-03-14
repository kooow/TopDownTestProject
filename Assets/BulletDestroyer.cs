using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    public float Seconds = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    // Update is called once per frame
    void Update()
    {

        float distanceFromCenter = Vector3.Distance(this.transform.position, Vector3.zero);
        if (distanceFromCenter > 10)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(Seconds);
        Destroy(gameObject);
    }
}
