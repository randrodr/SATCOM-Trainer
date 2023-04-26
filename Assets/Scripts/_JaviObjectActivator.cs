using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _JaviObjectActivator : MonoBehaviour
{
	public GameObject ObjectToObserve;
	public GameObject ObjectToActivate;
	public float ActivationTime = 0.035f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(ObjectToObserve.activeSelf && !ObjectToActivate.activeSelf){
			StartCoroutine(ObjectActivate());
		}else if (!ObjectToObserve.activeSelf && ObjectToActivate.activeSelf){
			ObjectToActivate.SetActive(false);
		}else if (ObjectToActivate.activeSelf){
			StopCoroutine(ObjectActivate());
		}else{
			
		}
    }
	
	private IEnumerator ObjectActivate(){
		yield return new WaitForSeconds(ActivationTime);
		ObjectToActivate.SetActive(true);
		Debug.Log("Running coroutine");
	}
}
