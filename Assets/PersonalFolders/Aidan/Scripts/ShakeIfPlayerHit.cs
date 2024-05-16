using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShakeIfPlayerHit : MonoBehaviour
{
    public float hitAmplitudeGain = 2, hitFrequencyGain = 2, shakeTime = 1;
    public bool isShaking = false;
    float shakeTimeElapsed = 0f;

    CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin noisePerlin;
    HealthManager healthManager;
    private float health;
    private float newHealth;

    [Header("Place player in here :)")]
    public GameObject player;

    private void Start() {
        healthManager = player.GetComponent<HealthManager>();
        //print(healthManager);
        health = healthManager.health;
        //print(health);


        vcam = GetComponent<CinemachineVirtualCamera>();
        noisePerlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void StartShake() {
        //print("player hit do shake");
        noisePerlin.m_AmplitudeGain = hitAmplitudeGain;
        noisePerlin.m_FrequencyGain = hitFrequencyGain;
        isShaking = true;
        shakeTimeElapsed = 0f;
    }

    public void StopShake () {
        isShaking = false;
        noisePerlin.m_AmplitudeGain = 0;
        noisePerlin.m_FrequencyGain = 0;
        shakeTimeElapsed = 0f;
    }

    private void Update() {
        newHealth = healthManager.health;
        if (health > newHealth) {
            StartShake();
            health = newHealth;
        }

        if (isShaking) {
            shakeTimeElapsed += Time.deltaTime;

            if (shakeTimeElapsed > shakeTime) {
                StopShake();
            }
        }

    }
}
