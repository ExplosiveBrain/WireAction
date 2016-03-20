using UnityEngine;
using System.Collections;

public class WireLauncher : MonoBehaviour {
    public LayerMask mask;
    public GameObject PrefabPod;
    private RaycastHit hit;

    private GameObject pod;
    public bool trigger1 = false;
    public bool trigger2 = false;

    public float limit;
    private GameObject[] podsleft;

    //射出位置調整
    public float Offset = 1f;
    private Vector3 offset;

    //Playerのプッシュ力
    public float Speed = 20f;
    private Vector3 direction;

    //PlayerのJump力
    public float JumpSpeed = 10f;

    //PlayerがPodに接近できる距離
    public float Range = 2.0f;

    //射撃可能な対象物の最小距離
    public float minShotRange = 1f;

    //Podの射出力
    public float ShotPower = 2000f;

    private GameObject Player;
    private Rigidbody PlayerRig;

    public GameObject Pod
    {
        get
        {
            return pod;
        }
    }

    


	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerRig = Player.GetComponent<Rigidbody>();        
	}

    // Update is called once per frame
    void Update()
    {
        if (!trigger1 && Input.GetButtonDown("Fire1"))
        {
            //対象が近すぎる場合は射出不能
            if (!Physics.Raycast(transform.position, -transform.forward, out hit, minShotRange))
            {
                LaunchPod();
            }
        }

        if (pod != null)
        {
            //壁に付着できた場合は他のPodをはずす
            if (pod.GetComponent<WirePod>().IsTarget == true)
            {
                podsleft = GameObject.FindGameObjectsWithTag("Pod");
                foreach (GameObject podleft in podsleft)
                {
                    if (podleft != pod) DetachPod(podleft);
                }

                pod.GetComponent<WirePod>().IsActive = true;
                if (trigger1 == false)
                {
                    trigger1 = true;
                }
            }
        }

        if (trigger1 && Input.GetButtonDown("Fire1"))
        {
            trigger2 = true;
        }

        if (pod.GetComponent<WirePod>().IsActive && Input.GetButtonDown("Jump"))
        {
            direction = pod.transform.position - Player.transform.position;
            DetachPod(pod);
            PlayerRig.isKinematic = false;
            pod.GetComponent<WirePod>().IsTarget = false;
            pod.GetComponent<WirePod>().IsActive = false;
            trigger1 = false;
            if (trigger2)
            {
                Player.GetComponent<Rigidbody>().velocity = JumpSpeed * (direction.normalized + Vector3.up * 0.3f);
            }
            else
            {
                Player.GetComponent<Rigidbody>().velocity = JumpSpeed * Vector3.up * 0.3f;
            }
            
        }

        if (trigger2)
        {            
            MoveToPod();
        }
    }

    void LaunchPod()
    {
        offset = transform.forward * -Offset;
        pod = ObjectPool.instance.GetGameObject(PrefabPod, transform.position + offset, transform.rotation);
        pod.GetComponent<Rigidbody>().AddForce(-transform.forward * ShotPower);  
    }

    //壁についたPodの取り外し
    public void DetachPod(GameObject pod)
    {
        if (pod.GetComponent<FixedJoint>() != null)
        {
            Destroy(pod.GetComponent<FixedJoint>());
        }
        pod.GetComponent<WirePod>().IsTarget = false;
        pod.GetComponent<WirePod>().IsActive = false;
    }

    void MoveToPod()
    {
        if (pod.GetComponent<WirePod>().IsActive == true)
        {            
            direction = pod.transform.position - Player.transform.position;
            PlayerRig.isKinematic = true;
            if (direction.sqrMagnitude > Range * Range)
            {
                Player.transform.position += direction.normalized * Speed;
            }
            else
            {
                pod.GetComponent<WirePod>().IsTarget = false;
                trigger1 = false;
                trigger2 = false;
            }
        }
        else
        {
            PlayerRig.isKinematic = false;
        }
    }
}
