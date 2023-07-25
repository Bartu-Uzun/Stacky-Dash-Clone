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
        if (!PathPlayerController.Instance.GetHasStoppedOnBridge() && !PathPlayerController.Instance.GetIsSliding() && _currentBridgeIndex < _bridges.Length && _currentBridgeIndex >= 0) {

            _hasStoppedOnBridge = false;

            _currentBridge = _bridges[_currentBridgeIndex];

            _currentBridge.SlidePlayerOnBridge();

            _isSliding = true;

            _currentBridgeIndex += _incrementAmount;


        }

    }

    public void LetPlayerSlide() {

        PathPlayerController.Instance.SlideOnBridge(_currentBridge.GetPath());
    }

    public void StopPlayer() {

        if (!_hasStoppedOnBridge) {

            _hasStoppedOnBridge = true;

            ReCalculateBridgeWhenPlayerStopped();

            PathPlayerController.Instance.BridgeStop(_currentBridge.GetPath());

        }
        

    }

    public void ReCalculateBridgeWhenPlayerStopped() {

        //Debug.Log("im called");

        ReverseIncrementAmount();

        _currentBridgeIndex += _incrementAmount;
        _currentBridge = _bridges[_currentBridgeIndex];

        _isSliding = false;
    }

    public void SetIsPlayerOnTheBridge(bool isPlayerOnTheBridge) {

        _isPlayerOnTheBridge = isPlayerOnTheBridge;
    }

    public void SetIsSliding(bool isSliding) {

        _isSliding = isSliding;
    }

    public void ReverseIncrementAmount() {

        _incrementAmount *= -1;
    }
    
}
