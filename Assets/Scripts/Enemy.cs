using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    NORMAL,
    FREEZE,
    DEAD
}

public class Enemy : MonoBehaviour
{
    protected EnemyState state; // Change to protected to allow access in subclasses
    private float fade;
    public float fadeRate = 0.01f;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Material>();
        fade = 1;
        state = EnemyState.NORMAL;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.DEAD)
        {
            if (fade > 0)
            {
                // change fade value on shader material
                fade -= fadeRate;
            }
            else
            {
                Destroy(this);
            }
        }
    }

    public void ChangeState(EnemyState state)
    {
        this.state = state;
    }
}
