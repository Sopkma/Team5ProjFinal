using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour{
    private CharacterController characterController;
    public float Speed = 5f;
    void Start(){
        characterController = GetComponent<CharacterController>();
    }

    
    void Update(){
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        characterController.Move(move * Time.deltaTime * Speed);
    }
}
