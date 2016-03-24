using UnityEngine;
using System.Collections;

public class BeamCasting : MonoBehaviour {
    private Transform ParticleSystem;
    private Transform Target;
	// Use this for initialization
	void Start () {
        Target = GameObject.Find("Muzzle").transform;
        ParticleSystem = transform.FindChild("Particle System");
	}
	
	// Update is called once per frame
	void Update () {
        if(ParticleSystem != null) ParticleSystem.LookAt(Target);
        else ParticleSystem = transform.FindChild("Particle System");
    }
}
