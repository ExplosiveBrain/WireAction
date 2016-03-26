using UnityEngine;
using System.Collections;

public class WireLauncher : MonoBehaviour {
    public LayerMask mask;
    public GameObject PrefabPod;

    public GameObject Muzzle;

    private RaycastHit hit;

    private GameObject pod;
    private GameObject particleEmitter;

    //trigger1はポッド付着状態を、trigger2は移動状態かどうか
    public bool trigger1 = false;
    public bool trigger2 = false;
    
    //鬼畜スイッチ
    public bool 鬼畜スイッチ = false;
    public bool trigger3 = true;

    public float limit;
    private GameObject[] podsleft;

    //private LineRenderer laser2;

    //Pazayerのプッシュ力
    public float Speed = 20f;
    private Vector3 direction;

    //PlayerのJump力
    public float JumpSpeed = 15f;

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
        //laser2 = gameObject.GetComponent<LineRenderer>();
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
                /*
                if (!laser2.enabled)
                {
                    laser2.enabled = true;
                }
                */
            }
            //pod付着時にクリックしたらtrigger2をtrueにしてMoveToPodをループ状態に
            if (trigger1 && Input.GetButtonDown("Fire1"))
            {
                trigger2 = true;
            }

            //podが壁に張り付いてる状態でspaceを押すと･･･
            if (pod.GetComponent<WirePod>().IsActive && Input.GetButtonDown("Jump"))
            {
                direction = pod.transform.position - Player.transform.position;

                //podがはがれた後
                DetachPod(pod);
                PlayerRig.isKinematic = false;
                trigger1 = false;

                //飛んでるときはその方向にブーン
                if (trigger2)
                {
                    Player.GetComponent<Rigidbody>().velocity = JumpSpeed * (direction.normalized + Vector3.up * 0.3f);
                }

                //飛ぶ前とかplayerが壁に張り付いた状態のときはその場でジャンプ
                else
                {
                    Player.GetComponent<Rigidbody>().velocity = JumpSpeed * Vector3.up * 0.3f;
                }



            }
            //MoveToPodがフレームごとに呼ばれてるみたいだから、よくわからんのでこうなった。
            if (trigger2)
            {
                MoveToPod();
            }

            if (trigger3)
            {
                trigger3 = false;
                DetachPod(pod);
                PlayerRig.isKinematic = false;
            }
            /*
            if (laser2.enabled)
            {
                laser2.SetPosition(1, Player.transform.position + (Player.transform.position - pod.transform.position).normalized * 5f);
                laser2.SetPosition(0, pod.transform.position);                
            }
            */
        }

        

    }

    void LaunchPod()
    {
        pod = ObjectPool.instance.GetGameObject(PrefabPod, Muzzle.transform.position, transform.rotation);
        //GameObjectの青→と反対方向に射出
        pod.GetComponent<Rigidbody>().AddForce(-transform.forward * ShotPower);
        //laser2.enabled = false;

        //pod.GetComponent<LineRenderer>().enabled = true;

        if (鬼畜スイッチ)
        {
            trigger3 = true;
        }
    }

    //壁についたPodの取り外し
    public void DetachPod(GameObject pod)
    {
        if (pod.GetComponent<FixedJoint>() != null)
        {
            Destroy(pod.GetComponent<FixedJoint>());
            pod.GetComponent<WirePod>().IsTarget = false;
            pod.GetComponent<WirePod>().IsActive = false;
            //laser2.enabled = false;
            pod.GetComponentInChildren<ParticleSystem>().Clear();
            pod.GetComponentInChildren<ParticleSystem>().Stop();

        }
        
    }

    void MoveToPod()
    {
        if (pod.GetComponent<WirePod>().IsActive == true)
        {
            direction = pod.transform.position - Player.transform.position;
            PlayerRig.isKinematic = true;
            if (direction.sqrMagnitude > Range * Range)
            {
                Player.transform.position += direction.normalized * Speed * Time.deltaTime;
            }

            //壁に到着次第trigger関係はリセット
            else
            {
                pod.GetComponent<WirePod>().IsTarget = false;
                trigger1 = false;
                trigger2 = false;
                pod.GetComponentInChildren<ParticleSystem>().Clear();
                pod.GetComponentInChildren<ParticleSystem>().Stop();
            }
        }
        else
        {
            PlayerRig.isKinematic = false;
        }
    }
}
