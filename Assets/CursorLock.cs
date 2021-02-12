using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    void Update()
    {
        Cursor.visible = true;

        if (Input.GetKey(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "Lock Cursor") && !MenuScript.Hide)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        if (GUI.Button(new Rect(125, 0, 100, 50), "Confine Cursor") && !MenuScript.Hide)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
