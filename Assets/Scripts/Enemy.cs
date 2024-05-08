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
    public float fadeRate = 1f;
    public Material vanishMaterial;
    private SpriteRenderer[] spriteRenderers;

    // Start is called before the first frame update
    void Awake()
    {
        fade = 1;
        state = EnemyState.NORMAL;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.DEAD)
        {
            if (fade == 1)
            {
                // spriteRenderer.material = vanishMaterial;
                foreach (SpriteRenderer item in spriteRenderers)
                {
                    item.material = vanishMaterial;
                }
            }
            if (fade > 0)
            {
                // print("FADE: " + fade);
                // change fade value on shader material
                vanishMaterial.SetFloat("_Fade", fade);
                fade -= fadeRate * Time.deltaTime;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void ChangeState(EnemyState state)
    {
        this.state = state;
    }
}
