using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotationController : MonoBehaviour
{

    public static CharacterRotationController Instance;

    private void Awake() {
        
        if (Instance == null) {

            Instance = this;
        }
        else {

            Destroy(this);
        }
    }
    public void Rotate(Vector3 direction) {

        transform.forward = direction;
    }
}
