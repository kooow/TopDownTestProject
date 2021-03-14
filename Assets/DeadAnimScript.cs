using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadAnimScript : MonoBehaviour
{

    [SerializeField]
    private Animator animator;


    public void DeadAnimEnd()
    {
        Destroy(this.gameObject);
    }

    public void Play()
    {
        this.animator.enabled = true;

    }

}
