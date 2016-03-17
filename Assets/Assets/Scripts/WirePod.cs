using UnityEngine;
using System.Collections;

public class WirePod : MonoBehaviour {
    Rigidbody rig;
    FixedJoint fixedjoint;

    public float force;
    public float lifetime = 5f;
    private float time;

    private bool istarget = false;

    GameObject Player;

    Vector3 vector;
    public bool IsTarget
    {
        get
        {
            return istarget;
        }
        set
        {
            istarget = value;
        }
    }

	// Use this for initialization
	void Start () {
        rig = transform.GetComponent<Rigidbody>();
        Player = GameObject.Find("Player");
        time = lifetime;
	}
	
	// Update is called once per frame
	void Update () {
        if (istarget)
        {
            time = lifetime;
        }
        else
        {
           //壁に当たらなかったら一定時間後に死ぬ
            time -= Time.deltaTime;
        }

        if (time < 0)
        {
            time = lifetime;
            ObjectPool.instance.ReleaseGameObject(gameObject);
        }
	}
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            fixedjoint = gameObject.AddComponent<FixedJoint>();
            fixedjoint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
            istarget = true;
        }
    }

    /*プレーヤー側に引き寄せ可否を委ねるかは検討
    今回はPod側で制御*/
    void AttractPlayer(Vector3 vector)
    {
        force = 100;
        Player.GetComponent<Rigidbody>().AddForce(vector * force);
    }
}
