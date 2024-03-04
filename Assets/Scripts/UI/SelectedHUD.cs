using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedHUD : MonoBehaviour
{
    public Text objectName;
    public Slider healthBar; 
    public Slider hungerBar;
    private AnimalStatus _selected;
    public AnimalStatus selected //logic for when selected is changed
    {
        get { return _selected; }
        set 
        {
            if (value != _selected) //first pass, check if new value different
            {

                if (_selected != null)
                {
                    //CancelHighlight();
                }
            }
            _selected = value; //second pass, actions on new value

            if (_selected == null)
            {
                Hide();
            }
            else
            {
                objectName.text = _selected.stats.objectName; //set selected name
           
                //HighlightObject();
                Show();
            }
        }
    }

    CanvasGroup _canvasGroup;
    Color _originalColor;



    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        selected = null;
        
    }

    private void FixedUpdate()
    {
        if (selected != null)
        {
            healthBar.value = (float)selected.health / selected.stats.maxHealth;
            hungerBar.value = (float)selected.hunger / selected.stats.maxHunger;
        }
        else
        {
            Hide();
        }
    }

    public void Hide()
    {
        //gameObject.SetActive(false);
        _canvasGroup.alpha = 0f; 
        _canvasGroup.blocksRaycasts = false; 
    }

    public void Show()
    {
        //gameObject.SetActive(true);
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }

    void HighlightObject()
    {
        Renderer renderer = selected.transform.GetComponent<Renderer>();
        _originalColor = renderer.material.color;
        renderer.material.color = Color.white;
    }

    void CancelHighlight()
    {
        Renderer renderer = selected.transform.GetComponent<Renderer>();
        renderer.material.color = _originalColor;
    }

}
