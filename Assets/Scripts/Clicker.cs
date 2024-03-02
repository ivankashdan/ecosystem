using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public SelectedHUD selectedHUD;
    Camera m_Camera;

    void Awake()
    {
        m_Camera = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.GetComponent<AnimalBehaviour>())
                {
                    AnimalBehaviour animal = hit.collider.GetComponent<AnimalBehaviour>();

                    selectedHUD.selected = animal;
                }
                else
                {
                    selectedHUD.selected = null;
                }
            }
        }
    }



}
