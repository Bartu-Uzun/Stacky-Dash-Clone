using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{

    [SerializeField] private bool _hasStack = false;
    [SerializeField] private GameObject _stack;
    [SerializeField] private GameObject _path;
    [SerializeField] private BridgeParent _parent;


    private void Awake() {
        _parent = GetComponentInParent<BridgeParent>();
    }

    public void SlidePlayerOnBridge() {

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

    private void StopPlayer() {
        
        //PathPlayerController.Instance.BridgeStop(_path);
        _parent.StopPlayer();
        

    }

    private void LetPlayerSlide() {
        // move player in the bridge's path
        _parent.LetPlayerSlide();
    }

    //adds one of the player's stacks to the bridge
    private void AddStackToBridge() {

        // remove stack from stackhandler's stackList
        StackHandler.Instance.RemoveStackFromStackList();

        _hasStack = true;
        _stack.SetActive(true);
    }

    public GameObject GetPath() {

        return _path;
    }
    
}
