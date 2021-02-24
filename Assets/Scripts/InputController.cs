using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Controller
{
    public float XSensivity = 360; //Degres par seconde par valeur d input.
    public float YSensivity = 360; //Degres par seconde par valeur d input.
    public string MouseXName = "Mouse X";
    public string MouseYName = "Mouse Y";

    public float MouseSensitivityMultiplier = 1.0f;

    // Update is called once per frame
    void Update()
    {
        //Lecture des inputs
        Vector2 axisLook = MouseSensitivityMultiplier * new Vector2(Input.GetAxis(MouseXName), Input.GetAxis(MouseYName));
        Vector2 axisMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        WantsToSlice = Input.GetButton("Fire1");
        WantsToJump = Input.GetButton("Jump");
        WantsToJump = Input.GetButton("Dash");

        //Rotation
        if (axisLook.x != 0 || axisLook.y != 0)
        {
            Vector3 WantedDirectionLookRight = Vector3.Cross(Vector3.up, WantedDirectionLook);
            Quaternion rotateHorizontal = Quaternion.AngleAxis(axisLook.x * XSensivity * Time.deltaTime, Vector3.up);
            Quaternion rotateVertical = Quaternion.AngleAxis(-axisLook.y * YSensivity * Time.deltaTime, WantedDirectionLookRight);
            WantedDirectionLook = rotateHorizontal * rotateVertical * WantedDirectionLook;
        }

        //Déplacement
        Vector3 WantedDirectionRight = Vector3.Cross(Vector3.up, WantedDirectionLook);
        WantedDirectionMove = WantedDirectionLook * axisMove.y + WantedDirectionRight * axisMove.x;
        WantedSpeed = Mathf.Max(Mathf.Abs(axisMove.x), Mathf.Abs(axisMove.y));

        //On affiche le debug
        DrawDebug();

        UpdateCursorLock();
    }

    public bool LockCursor = true;
    private bool hasFocus = false;
    private void UpdateCursorLock()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            hasFocus = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            hasFocus = true;
        }

        if (hasFocus && LockCursor)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
        else if (!hasFocus)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
    }
}