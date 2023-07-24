using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{

    [SerializeField] private bool _hasStack = false;
    [SerializeField] private bool _isPlayerOn = false;
    [SerializeField] private GameObject _stack;
    [SerializeField] private GameObject _path;
    public void OnEnterBridge() {

        //Debug.Log("is sliding: " + PathPlayerController.Instance.GetIsSliding());

        // if !PathPlayerController.Instance.GetIsSliding()
        if (true) { // if player is not already sliding on another bridge

            //Debug.Log("okay");

            if (!_hasStack) { //bridge needs a stack

                if (StackHandler.Instance.GetStackCount() > 1) { //player has stack

                    AddStackToBridge();

                    LetPlayerSlide(); // move player in the bridge's path


                }
                else {

                
                    StopPlayer(); // player stops

                    // player cannot move in the bridge's direction
                }

            }
            else { // move player in the bridge's path

                LetPlayerSlide();
            }
        }
    }

    private void StopPlayer() {

        PathPlayerController.Instance.Stop();
    }

    private void LetPlayerSlide() {
        // move player in the bridge's path
        PathPlayerController.Instance.SlideOnBridge(_path);
    }

    private void AddStackToBridge() {

        // remove stack from stackhandler's stackList
        StackHandler.Instance.RemoveStackFromStackList();

        _hasStack = true;
        _stack.SetActive(true);
    }

    public bool GetIsPlayerOn() {

        return _isPlayerOn;
    }

    public void SetIsPlayerOn(bool isPlayerOn) {

        _isPlayerOn = isPlayerOn;
    }

    public void OnExitBridge() {

        SetIsPlayerOn(false);
    }
}
