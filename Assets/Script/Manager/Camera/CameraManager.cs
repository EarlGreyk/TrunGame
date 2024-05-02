using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public  static CameraManager instance;

    private Vector3 CameraVec;

    private bool Lock;

    private Creture Target;

    private float MaxSize;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Lock = true;
        Target = GameManager.instance.Player;
        transform.position = Target.transform.position;
        CameraVec = new Vector3(0, 12, -7);
        MaxSize = GameSetManager.Instance.Size * 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T");
            LockCamera();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            LeftLotation();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            RightLotation();
        }



        if(!Lock)
        {
            if(Input.GetKey(KeyCode.W)) 
            {
                UnLockCameraMove(KeyCode.W);
            }
            if(Input.GetKey(KeyCode.S)) 
            {
                UnLockCameraMove(KeyCode.S);
            }
            if(Input.GetKey(KeyCode.D)) 
            {
                UnLockCameraMove(KeyCode.D);
            }
            if(Input.GetKey(KeyCode.A)) 
            {
                UnLockCameraMove(KeyCode.A); 
            }

        }else
        {
            LockCameraMove();
        }
    }

    private void LeftLotation()
    {
        Camera.main.transform.rotation = Quaternion.Euler(60.0f, Camera.main.transform.rotation.eulerAngles.y - 90.0f, 0);
        LotationPosSet();


    }

    private void RightLotation()
    {
        Camera.main.transform.rotation = Quaternion.Euler(60.0f, Camera.main.transform.rotation.eulerAngles.y + 90.0f, 0);
        LotationPosSet();
    }
    private void LotationPosSet()
    {
        switch (Camera.main.transform.rotation.eulerAngles.y)
        {
            case 90.0f:
                CameraVec = new Vector3(-7, 12, 0);
                break;
            case 180.0f:
                CameraVec = new Vector3(0, 12, 7);
                break;
            case 270.0f:
                CameraVec = new Vector3(7, 12, 0);
                break;
            default:
                CameraVec = new Vector3(0, 12, -7);
                break;
        }
        LockCameraMove();
    }

    public void LockCameraMove()
    {
        if(Lock && Target != null)
        { 
            Camera.main.transform.position = CameraVec + Target.transform.position;
        }
    }
    public void UnLockCameraMove(KeyCode key)
    {
        switch(key)
        {
            case KeyCode.A:
                if(CameraVec.x -0.1f > 0)
                {
                    Camera.main.transform.position = CameraVec + new Vector3(-0.1f, 0, 0);
                }
                break;
            case KeyCode.D:
                if (CameraVec.x + 0.1f < MaxSize)
                {
                    Camera.main.transform.position = CameraVec + new Vector3(0.1f, 0, 0);
                }
                break;
            case KeyCode.W:
                if (CameraVec.z + 0.1f < MaxSize-10)
                {
                    Camera.main.transform.position = CameraVec + new Vector3(0, 0, 0.1f);
                }
                break;
            case KeyCode.S:
                if (CameraVec.z - 0.1f > -10)
                {
                    Camera.main.transform.position = CameraVec + new Vector3(0, 0, -0.1f);
                }
                break;
        }
        
        CameraVec = Camera.main.transform.position;
    }

    public void LockCamera()
    {
        if (Lock)
        {
            Lock = false;
            CameraVec += Target.transform.position;
        }
        else
        {
            Lock = true;
            LotationPosSet();
        }
    }

    public void TargetSet(Creture creture)
    {
        Target = creture;
    }
}
