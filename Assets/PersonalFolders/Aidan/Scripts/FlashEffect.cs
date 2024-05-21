using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour {
    public Material flashMaterial;
    private Material originalMaterial;
    private SpriteRenderer spriteRenderer;
    private float flashDuration = 1.0f;
    private float timer = 0.0f;
    private bool isFlashing = false;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    void Update() {
        if (isFlashing) {
            timer += Time.deltaTime;
            float flashAmount = Mathf.PingPong(timer, flashDuration) / flashDuration;
            flashMaterial.SetFloat("_FlashAmount", flashAmount);

            if (timer >= flashDuration * 2) {
                isFlashing = false;
                timer = 0;
                spriteRenderer.material = originalMaterial;
            }
        }
    }

    public void StartFlashing() {
        isFlashing = true;
        timer = 0;
        spriteRenderer.material = flashMaterial;
    }
}