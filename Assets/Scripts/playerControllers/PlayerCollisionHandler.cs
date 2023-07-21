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

            else if (other.CompareTag(TagManager.BRIDGE_TAG)) {

                //CamFollowPlayer.Instance.SetIsOnBridge(true); // alert camera

                StartCoroutine(EnterBridge(other, false)); // player is not on end_platform
                
            }
            else if (other.CompareTag(TagManager.PATH_TAG)) {

                Debug.Log("added path");

                //PathManager.Instance.AddPath(other.gameObject); //PATHNEW add path if player has access to it

                
            }
            else if (other.CompareTag(TagManager.END_BRIDGE_TAG)) {

                StartCoroutine(EnterBridge(other, true)); //player is on end_platform

            }
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

    

    private void OnTriggerStay(Collider other) {

        if (!GameManager.Instance.GetLevelFinished()) { //if level has not finished yet

            if (other.gameObject.CompareTag(TagManager.BRIDGE_TAG)) {
                flag = other.GetComponent<BridgeHandler>().Slide();
            }
            else if (other.gameObject.CompareTag(TagManager.END_BRIDGE_TAG)) {
                flag = other.GetComponent<BridgeHandler>().Slide();
            }

            if (flag) {
            
                GameManager.Instance.LevelFinished(false);
            }

        }        
        
    }

    private void OnTriggerExit(Collider other) {
        
        if (other.CompareTag(TagManager.BRIDGE_TAG)) {

            //CamFollowPlayer.Instance.SetIsOnBridge(false); // alert camera

            other.GetComponent<BridgeHandler>().OnTriggerExitBridge();
        }
        else if (other.CompareTag(TagManager.PATH_TAG)) {

            //PathManager.Instance.RemovePath(other.gameObject); //PATHNEW remove path if player has no access to it
        }
    }

    private void OnCollisionEnter(Collision other) {
        
        if (other.gameObject.CompareTag(TagManager.CHEST_TAG)) {

            RigidbodyPlayerController.Instance.ChestStop();

        }
        
    }
    


    
    private IEnumerator EnterBridge(Collider other, bool enteredEndPlatform) {

        yield return new WaitForSeconds(0.05f);

        other.GetComponent<BridgeHandler>().OnEnterBridge(enteredEndPlatform);

    }
    
    
}
