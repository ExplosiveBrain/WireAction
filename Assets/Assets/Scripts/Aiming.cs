using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

/*アイアンサイトの実装*/
/*Press Fire2 to aim through iron sight*/

public class Aiming : MonoBehaviour {
    private Vector3 DefaultLocalPosition;
    private Vector3 AimingLocalPosition;

    /*Player Script*/
    private RigidbodyFirstPersonController FPScon_R;

    /*Camera Component*/
    private Camera Camera;

    /*AIM時の武器の座標*/
    public float AimingPosition;

    /*視点変更のスピード*/
    public float SmoothTime;

    /*zoom管理用の変数*/
    private bool zoomin;
    public float zoomspeed;

    /*通常時のFOV*/
    private float DefaultFOV;
    /*AIM時のFOV*/
    public float AimingFOV = 30f;

    /*通常時の感度*/
    private float DefaultSensitivity_X;
    private float DefaultSensitivity_Y;

    /*AIM時の感度*/
    public float AimSensitivity_X;
    public float AimSensitivity_Y;

    //デフォルト感度の設定
    private void AimSensitivitySettings(float x, float y)
    {
        DefaultSensitivity_X = x;
        DefaultSensitivity_Y = y;
    }

    private Vector3 velocity = Vector3.zero;
	// Use this for initialization
	void Start () {
        DefaultLocalPosition = transform.localPosition;
        AimingLocalPosition = new Vector3(0, 0, AimingPosition);
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        FPScon_R = GameObject.Find("Player").GetComponent<RigidbodyFirstPersonController>();
        DefaultFOV = Camera.fieldOfView;

        //デフォルト感度の設定
        AimSensitivitySettings(FPScon_R.mouseLook.XSensitivity, FPScon_R.mouseLook.YSensitivity);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire2"))
        {
            SwitchAim(AimingLocalPosition);
            zoomin = true;
            FPScon_R.mouseLook.XSensitivity = AimSensitivity_X;
            FPScon_R.mouseLook.YSensitivity = AimSensitivity_Y;
            //transform.localPosition = AimingLocalPosition;
        }
        else
        {
            SwitchAim(DefaultLocalPosition);
            zoomin = false;
            FPScon_R.mouseLook.XSensitivity = DefaultSensitivity_X;
            FPScon_R.mouseLook.YSensitivity = DefaultSensitivity_Y;
            //transform.localPosition = DefaultLocalPosition;
        }
        Zoom(zoomin);
	}

    /*視点切り替え*/
    void SwitchAim(Vector3 TargetPosition)
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, TargetPosition, ref velocity, SmoothTime);
    }

    /*ズーム*/
    void Zoom(bool zoomin)
    {
        if (zoomin && Camera.fieldOfView > AimingFOV)
        {
            Camera.fieldOfView -= zoomspeed * Time.deltaTime;
        }
        else if(!zoomin && Camera.fieldOfView < DefaultFOV)
        {
            Camera.fieldOfView += zoomspeed * Time.deltaTime;
        }
    }
}
