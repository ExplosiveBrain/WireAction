﻿using UnityEngine;
using System.Collections;

public class WirePod : MonoBehaviour {
    FixedJoint fixedjoint;

    public float lifetime = 5f;
    private float time;

    public float PushForce = 200f;

    private bool istarget = false;
    private bool isactive = false;

    Vector3 vector;
    public GameObject Player;
    private Rigidbody PlayerRig;

    //壁に着いてるか判定
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

    //Playerの引き寄せの可否
    public bool IsActive
    {
        get
        {
            return isactive;
        }

        set
        {
            isactive = value;
        }
    }

	// Use this for initialization
	void Start () {
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
            //スピード・回転・寿命をリセットした後にDeactive化
            time = lifetime;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            ObjectPool.instance.ReleaseGameObject(gameObject);
            istarget = false;
            isactive = false;
        }
	}
    
    void OnCollisionEnter(Collision collision)
    {
        //TagがWallの物体にのみ粘着
        if (collision.gameObject.tag == "Wall")
        {
            //同時に複数のPodが壁に着くのを防ぐ
            if (fixedjoint == null)
            {
                fixedjoint = gameObject.AddComponent<FixedJoint>();
                fixedjoint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
            }
            istarget = true;
        }
    }
}
