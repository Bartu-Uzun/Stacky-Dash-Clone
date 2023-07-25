using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlayerCollisionHandler : MonoBehaviour
{

    [SerializeField] private BridgeParent _currentParent; //use to get rid of all of the getcomponent spam
    private bool _isOnABridge = false;
    private bool _touchedTheEdge = false;

    private void OnTriggerEnter(Collider other) {

        if (!GameManager.Instance.GetLevelFinished()) {

            if (other.CompareTag(TagManager.STACK_TAG)) {

                StackHandler.Instance.AddStack(other.gameObject);

                ScoreHandler.Instance.IncrementScore();

            }
            else if (other.CompareTag(TagManager.BRIDGE_EDGE_TAG) && !_touchedTheEdge) {

                

                if (_currentParent == null) {
                    _currentParent = other.GetComponent<BridgeParent>();

                    _isOnABridge = true;

                }
                else {
                    _currentParent = null;
                    _isOnABridge = false;
                }
                
                _touchedTheEdge = true;
                StartCoroutine(EdgeCoroutine());
            }

            else if (other.CompareTag(TagManager.BRIDGE_TAG)) {

                //Debug.Log("i enter: " + other.gameObject.transform.parent.name);

                if (_currentParent != null) {

                    //Debug.Log("i have a parent");
                    StartCoroutine(EnterBridge(other));

                }
                
                
            }
            else if (other.CompareTag(TagManager.PATH_TAG)) {

                //Debug.Log("added path");

                PathManager.Instance.AddPath(other.gameObject); //PATHNEW add path if player has access to it

            }
            /*
            else if (other.CompareTag(TagManager.END_BRIDGE_TAG)) {

                StartCoroutine(EnterBridge(other, true)); //player is on end_platform

            }
            */
            else if (other.CompareTag(TagManager.X1_TAG)) {

                GameManager.Instance.SetTimes(1);
            }
            else if (other.CompareTag(TagManager.X2_TAG)) {

                GameManager.Instance.SetTimes(2);
            }
            else if (other.CompareTag(TagManager.X3_TAG)) {

                GameManager.Instance.SetTimes(3);
            }
            else if (other.CompareTag(TagManager.X4_TAG)) {

                GameManager.Instance.SetTimes(4);
            }
            else if (other.CompareTag(TagManager.FINISHLINE_TAG)) {

                GameManager.Instance.LevelFinished(true);
            }

        }

        
        
        
    }

    
    /* OLD VERSION
    private void OnTriggerStay(Collider other) {

        if (!GameManager.Instance.GetLevelFinished()) { //if level has not finished yet

            
            if (other.gameObject.CompareTag(TagManager.BRIDGE_TAG)) {
                //flag = other.GetComponent<BridgeHandler>().Slide(); 
            }

            else if (other.gameObject.CompareTag(TagManager.END_BRIDGE_TAG)) {
                flag = other.GetComponent<BridgeHandler>().Slide();
            }

            if (flag) {
            
                GameManager.Instance.LevelFinished(false);
            }

        }        
        
    }
    */

    private void OnTriggerStay(Collider other) {
        
        if (!GameManager.Instance.GetLevelFinished()) { //if level has not finished yet

            if (other.gameObject.CompareTag(TagManager.BRIDGE_TAG)) {

                if (_currentParent != null) {

                    //_currentParent.CheckPlayerOnBridge();
                    StartCoroutine(EnterBridge(other));

                }

                
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        
        
        if (other.CompareTag(TagManager.BRIDGE_TAG)) {

            //CamFollowPlayer.Instance.SetIsOnBridge(false); // alert camera

            //Debug.Log("i exit: " + other.gameObject.transform.parent.name);

            
        } 
        if (other.CompareTag(TagManager.PATH_TAG)) {

            //Debug.Log("exit path");

            PathManager.Instance.RemovePath(other.gameObject); //PATHNEW remove path if player has no access to it
        }
        
    }

    private void OnCollisionEnter(Collision other) {

        Debug.Log("collision with: " + other.gameObject.name);
        
        if (other.gameObject.CompareTag(TagManager.CHEST_TAG)) {

            RigidbodyPlayerController.Instance.ChestStop();

        }
        
    }

    private IEnumerator EdgeCoroutine() {

        yield return new WaitForSeconds(0.1f);

        _touchedTheEdge = false;
    }
    
    private IEnumerator EnterBridge(Collider other) {
        
        yield return new WaitForSeconds(0.05f);

        //other.GetComponent<Bridge>().OnEnterBridge();
        _currentParent.CheckPlayerOnBridge();

    }


    /* OLD VERSION
    private IEnumerator EnterBridge(Collider other, bool enteredEndPlatform) {

        yield return new WaitForSeconds(0.05f);

        other.GetComponent<BridgeHandler>().OnEnterBridge(enteredEndPlatform);

    }
    */
    
}
