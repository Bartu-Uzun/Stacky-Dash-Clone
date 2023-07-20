using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBridgeStack : MonoBehaviour
{
    private BridgeHandler _bridgeHandler;

    private void Awake() {
        
        _bridgeHandler = GetComponentInParent<BridgeHandler>();
    }
    private void OnTriggerExit(Collider other) {

        ChangeBridgeState();
        

    }

    private void ChangeBridgeState() {


        gameObject.tag = TagManager.DROPPED_STACK_TAG;
        _bridgeHandler.SwitchState(_bridgeHandler.GetBridgeNormalState());
        Destroy(this);
    }
}
