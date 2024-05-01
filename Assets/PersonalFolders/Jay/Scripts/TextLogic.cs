using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    public float destroyTime = 2f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(new Vector2(0, 0.5f), ForceMode2D.Impulse);
        StartCoroutine(DestroyIn());
    }

    private IEnumerator DestroyIn()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
