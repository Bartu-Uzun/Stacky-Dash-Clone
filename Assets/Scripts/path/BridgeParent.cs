using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeParent : MonoBehaviour
{

    [SerializeField] private Bridge[] _bridges;
    [SerializeField] private Bridge _currentBridge;
    [SerializeField] private GameObject _edgePath1;
    [SerializeField] private GameObject _edgePath2;


    [SerializeField] private int _currentBridgeIndex = 0;
    [SerializeField] private int _incrementAmount = 1;
    [SerializeField] private bool _hasStoppedOnBridge = false;
    [SerializeField] private bool _isPlayerOnTheBridge = false;



    // this class can manage sliding operations so that player slides through bridges one by one

    private void Awake() {
        
        _bridges = GetComponentsInChildren<Bridge>();
    }

    //OnTriggerStay --> CheckPlayerOnBridge()
    public void CheckPlayerOnBridge() {


        //if !isSliding --> slide on _bridges[i+1] if player has stack
        if (_isPlayerOnTheBridge && !PathPlayerController.Instance.GetHasStoppedOnBridge() && !PathPlayerController.Instance.GetIsSliding() && _currentBridgeIndex < _bridges.Length && _currentBridgeIndex >= 0) {

            _hasStoppedOnBridge = false;

            _currentBridge = _bridges[_currentBridgeIndex];

            _currentBridge.SlidePlayerOnBridge();

            _currentBridgeIndex += _incrementAmount;

            if (_currentBridgeIndex < 0) { //player left the bridge from the first edge

                EndOfBridge(_edgePath1);
                _currentBridgeIndex = 0;
            }
            else if (_currentBridgeIndex >= _bridges.Length) { // player left the bridge from the second edge

                EndOfBridge(_edgePath2);


                _currentBridgeIndex = _bridges.Length - 1;

                //Debug.Log(_currentBridgeIndex);
            }


        }

    }

    public void PlayerEnteredTheBridge() {
        _isPlayerOnTheBridge = true;
    }

    private void EndOfBridge(GameObject edgePath) {

        _isPlayerOnTheBridge = false;

        PathPlayerController.Instance.EndOfBridge(edgePath);

        ReverseIncrementAmount();
    }

    /*
    private IEnumerator EndOfBridge(GameObject edgePath) {

        yield return new WaitForSeconds(0.1f);

        _isPlayerOnTheBridge = false;

        PathPlayerController.Instance.EndOfBridgeMovement(edgePath);

        ReverseIncrementAmount();
    }
    */

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

        ReverseIncrementAmount();

        _currentBridgeIndex += _incrementAmount;
        _currentBridge = _bridges[_currentBridgeIndex];
    }

    public void SetIsPlayerOnTheBridge(bool isPlayerOnTheBridge) {

        _isPlayerOnTheBridge = isPlayerOnTheBridge;
    }

    public void ReverseIncrementAmount() {

        _incrementAmount *= -1;
    }
    
}
