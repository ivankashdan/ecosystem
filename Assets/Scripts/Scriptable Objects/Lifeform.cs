using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Lifeform")]
public class Lifeform : ScriptableObject
{
    public string objectName;
    public bool edible;
    public int nutrition;
}
