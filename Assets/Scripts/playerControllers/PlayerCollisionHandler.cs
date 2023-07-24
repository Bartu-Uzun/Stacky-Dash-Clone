using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlayerCollisionHandler : MonoBehaviour
{

    private bool flag = false;
    private void OnTriggerEnter(Collider other) {

        if (!GameManager.Instance.GetLevelFinished()) {

            if (other.CompareTag(TagManager.STACK_TAG)) {

                StackHandler.Instance.AddStack(other.gameObject);

                ScoreHandler.Instance.IncrementScore();

            }

            else if (other.CompareTag(TagManager.BRIDGE_TAG) && !other.GetComponent<Bridge>().GetIsPlayerOn()) {

                //CamFollowPlayer.Instance.SetIsOnBridge(true); // alert camera

                //StartCoroutine(EnterBridge(other, false)); // player is not on end_platform OLD VERSION


                other.GetComponent<Bridge>().SetIsPlayerOn(true);

                Debug.Log("i enter: " + other.gameObject.transform.parent.name);

                StartCoroutine(EnterBridge(other));
                
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

    private void OnTriggerExit(Collider other) {
        
        
        if (other.CompareTag(TagManager.BRIDGE_TAG) && other.GetComponent<Bridge>().GetIsPlayerOn()) {

            //CamFollowPlayer.Instance.SetIsOnBridge(false); // alert camera

            //other.GetComponent<BridgeHandler>().OnTriggerExitBridge(); OLD VERSION

            Debug.Log("i exit: " + other.gameObject.transform.parent.name);

            //other.GetComponent<Bridge>().OnExitBridge();
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
    
    private IEnumerator EnterBridge(Collider other) {
        
        yield return new WaitForSeconds(0.05f);

        other.GetComponent<Bridge>().OnEnterBridge();

    }


    /* OLD VERSION
    private IEnumerator EnterBridge(Collider other, bool enteredEndPlatform) {

        yield return new WaitForSeconds(0.05f);

        other.GetComponent<BridgeHandler>().OnEnterBridge(enteredEndPlatform);

    }
    */
    
}
