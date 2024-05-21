using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour {
    public Material flashMaterial;
    public float flashDuration = 1.0f; // Duration of the flashing effect in seconds

    private float timeElapsed;
    private bool isFlashing;
    private Material originalMaterial;
    private SpriteRenderer sprenderer;

    void Start() {
        sprenderer = GetComponent<SpriteRenderer>();
        originalMaterial = sprenderer.material;
    }

    void Update() {
        if (isFlashing) {
            timeElapsed += Time.deltaTime;

            // Update the shader's _TimeElapsed property
            flashMaterial.SetFloat("_TimeElapsed", timeElapsed);

            // Stop flashing after the duration is over
            if (timeElapsed >= flashDuration) {
                StopFlashing();
            }
        }
    }

    public void StartFlashing() {
        timeElapsed = 0.0f;
        isFlashing = true;
        // Apply the flash material to the object
        sprenderer.material = flashMaterial;
    }

    private void StopFlashing() {
        isFlashing = false;
        // Reset the object's material to the original material
        sprenderer.material = originalMaterial;
    }
}