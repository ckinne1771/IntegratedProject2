using UnityEngine;
using System.Collections;

public class CustomerSpawningScript : MonoBehaviour {
 
	public Transform target;
	public GameObject unit;

	public CraftingScript spawnCraftingScript;
	public int customersSpwaned;
	public float spawnTime = 3f;
	float spawnTimeLeft = 8f;
	public GameObject SpawnPlayer;

	void Start()
	{
		SpawnPlayer=GameObject.Find ("Player");
		spawnCraftingScript = SpawnPlayer.GetComponent<CraftingScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(spawnTimeLeft <= 0 && customersSpwaned <8) {
			GameObject go = (GameObject)Instantiate(unit, transform.position, transform.rotation);
			go.GetComponent<CustomerAIScript>().target = target;
			spawnTimeLeft = spawnTime;
			customersSpwaned++;
			spawnCraftingScript.OncustomerSpawn ();


		}
		else {
			spawnTimeLeft -= Time.deltaTime;
		}
	}
}
