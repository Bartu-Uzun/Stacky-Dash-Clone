using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> _bridgeStacks = new List<GameObject>();

    [SerializeField] private int _capacity;

    [SerializeField] private BridgeBaseState _currentBridgeState;

    [SerializeField] private Vector3 _direction; //current direction of the bridge
    private Vector3 _initialDirection;
    private BridgeNormalState _bridgeNormalState = new BridgeNormalState();
    private BridgeFinishedState _bridgeFinishedState = new BridgeFinishedState();
    private BridgeWithPlayerStoppedState _bridgeWithPlayerStoppedState = new BridgeWithPlayerStoppedState();
    
    


    [SerializeField] private bool _playerJustEntered = false; // will become true at the instance, then become false again
    private bool _playerIsOnTheBridge = false; // will become true and stay that way as long as player is on the bridge
    private bool _enteredEndPlatform = false; // gets true whenever player enters the end_platform 

    private void Awake() {
        
        _currentBridgeState = _bridgeNormalState;
        _initialDirection = _direction;
    }

    public List<GameObject> GetBridgeStacks() {
        return _bridgeStacks;
    }

    public int GetCapacity() {
        return _capacity;
    }

    public BridgeFinishedState GetBridgeFinishedState() {

        return _bridgeFinishedState;
    }

    public BridgeWithPlayerStoppedState GetBridgeWithPlayerStoppedState() {

        return _bridgeWithPlayerStoppedState;
    }

    public BridgeNormalState GetBridgeNormalState() {

        return _bridgeNormalState;
    }

    public Vector3 GetDirection() {

        return _initialDirection;
    }

    public bool GetEnteredEndPlatform() {

        return _enteredEndPlatform;
    }

    
    /*
    ** called when player enters the bridge
    ** if level is finished (i.e. player is in one of the end bridges), _enteredEndPlatform tag will be updated
    ** player's movement will be updated according to the direction of the bridge, 
    ** and the allowed direction of the bridge will be reversed (i.e. multiplied by -1)
    */
    public void OnEnterBridge(bool enteredEndPlatform) {

        

        _enteredEndPlatform = enteredEndPlatform;


        if (!_playerJustEntered) {

            _playerJustEntered = true;

            RigidbodyPlayerController.Instance.EnterToANewBridge(_direction);

            _playerIsOnTheBridge = true;

            StartCoroutine(ChangeDirection());

        }

        
    }

    public void OnExitBridge() {

        _playerJustEntered = false;

        _playerIsOnTheBridge = false;

        StartCoroutine(ChangeDirection());
    }
    

    
    private IEnumerator ChangeDirection() {

        _direction *= -1;

        yield return new WaitForSeconds(0.1f);

        _playerJustEntered = false;

        
    }
    

    public void OnTriggerExitBridge() {

        _currentBridgeState.OnTriggerExitBridge(this);
    }
    public bool Slide() {

        bool flag = false;

        if (_playerIsOnTheBridge) {

            flag = _currentBridgeState.Slide(this);
        }

        return flag;
        
        
    }

    public void SwitchState(BridgeBaseState state) {

        _currentBridgeState = state;

    }

}
