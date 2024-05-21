using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySwordSwing : MonoBehaviour
{

    private Animator anim;
    SwordAttack sword;
    // Start is called before the first frame update
    void Start()
    {
        sword = GetComponent<SwordAttack>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    


    public void SwingSword(float speed)
    {
        anim.SetBool("Attacking",true);
        anim.speed = speed;
        anim.Play("SwordSwing");
    }
}
