using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeParent : MonoBehaviour
{

    [SerializeField] private Bridge[] _bridges;
    [SerializeField] private Bridge _currentBridge;

    [SerializeField] private int _currentBridgeIndex = 0;
    [SerializeField] private int _incrementAmount = 1;
    [SerializeField] private bool _isSliding = false;
    [SerializeField] private bool _hasStoppedOnBridge = false;
    [SerializeField] private bool _isPlayerOnTheBridge = false;



    // this class can manage sliding operations so that player slides through bridges one by one

    private void Awake() {
        
        _bridges = GetComponentsInChildren<Bridge>();
    }
    
    public void OnEnterBridge() {

        if (!_isPlayerOnTheBridge) {

            Debug.Log("i got in! playeronbridge: " + _isPlayerOnTheBridge);

            _isPlayerOnTheBridge = true;

            //slide on _bridges[0] if player has stack
            if (!PathPlayerController.Instance.GetIsSliding()) {

                _currentBridge = _bridges[_currentBridgeIndex];

                _currentBridge.SlidePlayerOnBridge();

                _isSliding = true;

                _currentBridgeIndex += _incrementAmount;
            }
        }

        
    }

    //OnTriggerStay --> CheckPlayerOnBridge()
    public void CheckPlayerOnBridge() {

        _isPlayerOnTheBridge = true;

        //if !isSliding --> slide on _bridges[i+1] if player has stack
        if (!PathPlayerController.Instance.GetIsSliding() && _currentBridgeIndex < _bridges.Length) {

            Debug.Log("not sliding, so start a new slide");

            _currentBridge = _bridges[_currentBridgeIndex];

            _currentBridge.SlidePlayerOnBridge();

            _isSliding = true;

            _currentBridgeIndex += _incrementAmount;


        }

    }

    //if player has already slided on the last bridge, stop, recalculate bridge values
    private void OnExitBridge() {


    }

    public void SetHasStoppedOnBridge(bool hasStoppedOnBridge) {

        _hasStoppedOnBridge = hasStoppedOnBridge;

    }

    public void SetIsPlayerOnTheBridge(bool isPlayerOnTheBridge) {

        _isPlayerOnTheBridge = isPlayerOnTheBridge;
    }

    public void SetIsSliding(bool isSliding) {

        _isSliding = isSliding;
    }
}
