using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _moveSpeed = 20f;
    //private float bumpBackAmount = 0.2f;

    [SerializeField] private bool _shouldGoLeft = false;
    [SerializeField] private bool _shouldGoRight = false;
    [SerializeField] private bool _shouldGoForward = false;
    [SerializeField] private bool _shouldGoBack = false;

    private bool _isMoving = false;

    public static PlayerController Instance;

    private void Awake() {
        
        if (Instance != null) {
            Destroy(this);
        }
        else {
            Instance = this;
        }

        //DontDestroyOnLoad(gameObject); //NEED TO CHECK
    }

    private void Update() {
        
        ReadInput();

        MovePlayer();
    }

    /*  player stops moving bc of collision
    ** checks the direction the player was moving
    ** creates a little bump effect so that player doğru hizalansın
    ** updates states
     */
    public void CollideAndStop() {

        Vector3 direction;
        if (_shouldGoBack) {
            direction = Vector3.forward;
            _shouldGoBack = false;
        }
        else if (_shouldGoForward) {
            direction = Vector3.back;
            _shouldGoForward = false;
        }
        else if (_shouldGoLeft) {
            direction = Vector3.right;
            _shouldGoLeft = false;
        }
        else { //_shouldGoRight
            direction = Vector3.left;
            _shouldGoRight = false;
        }

        Debug.Log("before: " + transform.position);
        //transform.position += direction * bumpBackAmount;

        //Debug.Log("after: " + transform.position);
        _isMoving = false;
    }

    private void ReadInput(){

        if (!_isMoving) {

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || MobileInput.Instance.swipeLeft) {
            _shouldGoLeft = true;
            _isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || MobileInput.Instance.swipeRight) {
            _shouldGoRight = true;
            _isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || MobileInput.Instance.swipeDown) {
            _shouldGoBack = true;
            _isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || MobileInput.Instance.swipeUp) {
            _shouldGoForward = true;
            _isMoving = true;
        }
    }

    private void MovePlayer(){

        if (_shouldGoLeft) {

            transform.position += Vector3.left * _moveSpeed * Time.deltaTime;
            //_shouldGoLeft = false;
        }
        
        else if (_shouldGoRight) {

            transform.position += Vector3.right * _moveSpeed * Time.deltaTime;
            //_shouldGoRight = false;
        }

        else if (_shouldGoBack) {

            transform.position += Vector3.back * _moveSpeed * Time.deltaTime;
            //_shouldGoBack = false;
        }
        else if (_shouldGoForward) {

            transform.position += Vector3.forward * _moveSpeed * Time.deltaTime;
            //_shouldGoForward = false;
        }
    }
}
