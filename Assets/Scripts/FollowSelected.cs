using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSelected : MonoBehaviour
{
    [SerializeField] SelectedHUD hud;

    private void Update()
    {
        Vector3 selectedPosition = hud.selected.transform.position;

        selectedPosition = new Vector3(selectedPosition.x, selectedPosition.y, selectedPosition.z);

        transform.position = selectedPosition + transform.position;
    }


}
