using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    SPAWNING,
    NORMAL,
    FREEZE,
    DEAD
}

public class Enemy : MonoBehaviour
{
    protected EnemyState state; // Change to protected to allow access in subclasses
    private float fade;
    public float fadeRate = 1f;
    public float spawnTime = 1.7f;
    public Material vanishMaterial;
    private Material newVanish;
    private SpriteRenderer[] spriteRenderers;
    private Collider2D[] colliders;
    [Header("Gameobject with particle system for spawning")]
    public GameObject spawnEffect;
    

    // Start is called before the first frame update
    void Awake()
    {
        newVanish = new Material(vanishMaterial);
        fade = 1;
        state = EnemyState.SPAWNING;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        colliders = GetComponentsInChildren<Collider2D>();
        StartCoroutine(SpawnIn());
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

    private IEnumerator SpawnIn()
    {
        Instantiate(spawnEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(spawnTime);
        state = EnemyState.NORMAL;
    }
}
