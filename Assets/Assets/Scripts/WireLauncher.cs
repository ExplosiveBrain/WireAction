using UnityEngine;
using System.Collections;

public class WireLauncher : MonoBehaviour {
    public LayerMask mask;
    public GameObject PrefabPod;
    private RaycastHit hit;

    public float Offset;

    private Vector3 offset;

	// Use this for initialization
	void Start () {
        
	}

    // Update is called once per frame
    void Update()
    {

        
        if (Physics.Raycast(transform.position, -transform.forward, out hit, 100))
        {
            print("name = " + hit.transform.name);
        }
        else
        {
            //print("No Target");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            LaucnhPod();
        }
    }

    //実用の際は存在するpodの個数で管理するべき
    void LaucnhPod()
    {
        if (true)
        {
            offset = transform.forward * -Offset;
            ObjectPool.instance.GetGameObject(PrefabPod, transform.position + offset, transform.rotation).GetComponent<Rigidbody>().AddForce(-transform.forward * 1000);
        }
    }
}
