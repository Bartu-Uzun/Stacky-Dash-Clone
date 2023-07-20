using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _lerpTime;

    private Vector3 _destination;

    // private void Start() {
        
    //     _destination = _target.transform.position + _offset;
    //     transform.position = Vector3.Lerp(transform.position, _destination, _lerpTime);
    // }
    

    // Update is called once per frame
    void LateUpdate()
    {

        _destination = _target.transform.position + _offset;
        transform.position = Vector3.Lerp(transform.position, _destination, _lerpTime);

        // _destination.x = _target.transform.position.x + _offset.x;

        // _destination.y = this.transform.position.y;
        // _destination.z = this.transform.position.z;

        // transform.position = Vector3.Lerp(transform.position, _destination, _lerpTime);
        
    }
}
