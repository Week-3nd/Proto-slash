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
    public float RecoilMultiplier = 1.0f;
    private float RecoilTimer = 0.0f;

    void Start()
    {
        //SwordOffset = new Vector3(SwordForwardOffset, SwordHorizontalOffset, SwordVerticalOffset);
    }
    

    void Update()
    {

        //activer l'épée
        if (PlayerController.WantsToSlice && !PreviousWill)
        {
            PlayerInput.MouseSensitivityMultiplier = HoldSensitivityRatio;
            SwordObject.transform.position = transform.position + SwordForwardOffset * PlayerController.WantedDirectionLook.normalized;
            SwordObject.transform.rotation = transform.rotation;
            SwordObject.SetActive(true);
            AngleList[0] = PlayerController.WantedDirectionLook;
        }

        //l'épée est active
        if (PlayerController.WantsToSlice && PreviousWill)
        {
            SwordObject.transform.position = transform.position + SwordForwardOffset * PlayerController.WantedDirectionLook.normalized;
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
                SwordCollisionDetector.Colliding = false;
                Vector3 Amplitude = AngleList[0] - AngleList[1];
                float Speed = Amplitude.magnitude / Time.deltaTime;
                RecoilVector = -Amplitude.normalized * Speed * RecoilMultiplier;
                //Debug.Log("Recoil : " + Recoil);
                Debug.Log("Collision!  |speed = " + Speed + "  |RecoilVector : " + RecoilVector + "  |RecoilSize : " + RecoilSize);
                RecoilTimer = RecoilDuration;
            }
        }

        //ranger l'épée
        if (!PlayerController.WantsToSlice && PreviousWill)
        {
            PlayerInput.MouseSensitivityMultiplier = 1.0f;
            SwordObject.SetActive(false);
        }

        RecoilHandler();
        PreviousWill = PlayerController.WantsToSlice;
    }

    void RecoilHandler()
    {
        //recoil : la size est consommée jusqu'à atteindre zéro
        RecoilTimer -= Time.deltaTime;
        if (RecoilTimer <= 0)
        {
            RecoilVector = Vector3.zero;
        }
    }
}