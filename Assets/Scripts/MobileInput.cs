using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static MobileInput Instance;

    [HideInInspector]
    public bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;

    [HideInInspector]
    public Vector2 swipeDelta, startTouch;

    private const float _THRESHOLD = 100;

    private void Awake() {
        
        if (Instance != null) {

            Destroy(gameObject);
        }
        else {
            Instance = this;
        }

        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        //reset
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        //read input
        #region Computer Inputs

        if (Input.GetMouseButtonDown(0)) { //pushed

            tap = true;
            startTouch = Input.mousePosition;
        }

        else if (Input.GetMouseButtonUp(0)) { //no more button pushed

            startTouch = swipeDelta = Vector2.zero; //reset both vectors
        }

        #endregion

        #region Mobile Inputs

        if (Input.touches.Length != 0) {

            if (Input.touches[0].phase == TouchPhase.Began) {

                tap = true;
                startTouch = Input.touches[0].position;
            }

            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) {

                startTouch = swipeDelta = Vector2.zero;
            }
        }

        #endregion

        // calculate the swipe input

        swipeDelta = Vector2.zero;

        if (startTouch != Vector2.zero) { //if there is an input

            // for mobile
            if (Input.touches.Length != 0) {

                swipeDelta = Input.touches[0].position - startTouch;
            }

            //for computer
            else if (Input.GetMouseButton(0)) {

                swipeDelta = (Vector2) Input.mousePosition - startTouch; //we cast mousePosition to Vector2

            }
        }

        
        // check where the swipe is towards, and if the input should be applied

        if (swipeDelta.magnitude > _THRESHOLD) { // check if input swipe is greater than the threshold

            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y)) { // we should move in x direction

                if (x < 0) { // in -x direction, ie Left

                    swipeLeft = true;
                }
                else { // in +x direction, ie Right

                    swipeRight = true;
                }
            }

            else { // we should move in y direction

                if (y < 0) { // -y, ie Down

                    swipeDown = true;

                }
                else { // +y, ie Up
                    swipeUp = true;
                }

            }

            startTouch = swipeDelta = Vector2.zero;


        }

        
    }
}
