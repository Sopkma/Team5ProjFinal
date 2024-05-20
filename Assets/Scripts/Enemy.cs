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
    private Material newVanish;
    private SpriteRenderer[] spriteRenderers;
    private Collider2D[] colliders;

    // Start is called before the first frame update
    void Awake()
    {
        newVanish = new Material(vanishMaterial);
        fade = 1;
        state = EnemyState.NORMAL;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        colliders = GetComponentsInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.DEAD)
        {
            foreach (Collider2D item in colliders)
            {
                item.enabled = false;
            }
            if (fade == 1)
            {
                // spriteRenderer.material = vanishMaterial;
                foreach (SpriteRenderer item in spriteRenderers)
                {
                    item.material = newVanish;
                }
            }
            if (fade > 0.1)
            {
                // print("FADE: " + fade);
                // change fade value on shader material
                newVanish.SetFloat("_Fade", fade);
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

    public void FacePlayer(Vector2 distance)
    {
        if (distance.x < 0)
        {
            // if sprite facing Right
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
        }
        else
        {
            // if sprite facing Left
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
        }
    }

    public void FacePlayer(Vector2 distance, GameObject sprite)
    {
        if (distance.x < 0)
        {
            // if sprite facing Right
            if (sprite.transform.localScale.x > 0)
            {
                sprite.transform.localScale = new Vector2(-sprite.transform.localScale.x, sprite.transform.localScale.y);
            }
        }
        else
        {
            // if sprite facing Left
            if (sprite.transform.localScale.x < 0)
            {
                sprite.transform.localScale = new Vector2(-sprite.transform.localScale.x, sprite.transform.localScale.y);
            }
        }
    }
}
