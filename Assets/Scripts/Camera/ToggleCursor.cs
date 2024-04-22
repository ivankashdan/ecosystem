using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCursor : MonoBehaviour
{
    public bool cursor;

    void Start()
    {
        Cursor.visible = cursor ? true : false;
    }

}
