using UnityEngine;
using System.Collections;

public class CustomerDestroyer : MonoBehaviour {
	
	void OnTriggerEnter(Collider customer) {
		Destroy(customer.gameObject);
	}
}