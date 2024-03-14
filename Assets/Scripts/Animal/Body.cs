using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Body : MonoBehaviour //everything to do with body here
{
    protected Animal stats;
    GameObject gfx;
    public Collider gfxCollider;
    Color original;
    Coroutine redCoroutine;


    protected virtual void Awake()
    {
        stats = GetComponent<Stats>().stats;
        gfx = GetComponentInChildren<MeshRenderer>().gameObject;
        gfxCollider = gfx.GetComponent<Collider>();

        UpdateModel();
    }

    protected void UpdateModel()
    {
        if (gfx!= null)
        {
            Destroy(gfx);
        }
        gfx = Instantiate(stats.model, transform);
        gfxCollider = gfx.GetComponent<Collider>();
        gfx.name = "GFX";
    }

    public void FlashRed()
    {
        if (redCoroutine == null)
        {
            redCoroutine = StartCoroutine(RedCoroutine());
        }
    }

    IEnumerator RedCoroutine()
    {
       MeshRenderer renderer = gfx.GetComponent<MeshRenderer>();
       original = renderer.material.color;
       renderer.material.color = Color.red;
       yield return new WaitForSeconds(0.1f);
       renderer.material.color = original;
       redCoroutine = null;
    }



}