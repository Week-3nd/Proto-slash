using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Controller PlayerController;
    public InputController PlayerInput;
    public float HoldSensitivityRatio = 0.9f;

    private Vector3[] AngleList = new Vector3[2];

    private bool PreviousWill = false;

    public GameObject SwordObject;
    public CollisionDetector SwordCollisionDetector;
    public float SwordForwardOffset = 1.0f;
    public float SwordHorizontalOffset = 0.0f;
    public float SwordVerticalOffset = 0.0f;
    //private Vector3 SwordOffset;

    public Vector3 RecoilVector;
    public float RecoilSize = 0.0f;
    public float RecoilDuration = 0.5f; //en secondes

    void Start()
    {
        //SwordOffset = new Vector3(SwordForwardOffset, SwordHorizontalOffset, SwordVerticalOffset);
    }
    

    void Update()
    {
        //gérer le recoil avant qu'il soit de nouveau calculé au besoin
        RecoilHandler();

        //activer l'épée
        if (PlayerController.WantsToSlice && !PreviousWill)
        {
            PlayerInput.MouseSensitivityMultiplier = HoldSensitivityRatio;
            SwordObject.transform.position = transform.position + SwordForwardOffset * PlayerController.WantedDirectionLook;
            SwordObject.transform.rotation = transform.rotation;
            SwordObject.SetActive(true);
            AngleList[0] = PlayerController.WantedDirectionLook;
        }

        //l'épée est active
        if (PlayerController.WantsToSlice && PreviousWill)
        {
            SwordObject.transform.position = transform.position + SwordForwardOffset * PlayerController.WantedDirectionLook;
            SwordObject.transform.rotation = transform.rotation;

            if ( PlayerController.WantedDirectionLook != AngleList[0] )
            {
                AngleList[1] = AngleList[0];
                AngleList[0] = PlayerController.WantedDirectionLook;
            }
            
            //if choc
            // position 1 - position 2. abs = vitesse, norm = direction
            // caméra = mouvement forcé de -direction, vitesse

            if (SwordCollisionDetector.Colliding)
            {
                Vector3 Amplitude = AngleList[0] - AngleList[1];
                float Speed = Amplitude.magnitude / Time.deltaTime;
                //Debug.Log("Collision! speed :  " + Speed);
                //Debug.Log(SwordCollisionDetector.Colliding);
                SwordCollisionDetector.Colliding = false;
                RecoilVector = -Amplitude.normalized * Speed;
                RecoilSize = Speed * RecoilDuration;
                //Debug.Log("Recoil : " + Recoil);
            }
        }

        //ranger l'épée
        if (!PlayerController.WantsToSlice && PreviousWill)
        {
            PlayerInput.MouseSensitivityMultiplier = 1.0f;
            SwordObject.SetActive(false);
        }

        PreviousWill = PlayerController.WantsToSlice;
    }

    void RecoilHandler()
    {
        //recoil : la size est consommée jusqu'à atteindre zéro
        RecoilSize -= RecoilVector.magnitude * Time.deltaTime;

        if (RecoilSize >= 0)
        {
            RecoilVector = Vector3.zero;
        }
    }
}