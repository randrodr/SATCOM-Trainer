using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _JaviQuckTransformSmap : MonoBehaviour
{
	public GameObject ObjectToFollow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ObjectToFollow.activeSelf == true){
			this.transform.position = ObjectToFollow.transform.position;
		}else{
			
		}
    }
}
