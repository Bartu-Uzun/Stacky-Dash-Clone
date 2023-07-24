using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeParent : MonoBehaviour
{

    [SerializeField] private Bridge[] _bridges;


    // this class can manage sliding operations so that player slides through bridges one by one

    private void Awake() {
        
        _bridges = GetComponentsInChildren<Bridge>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
