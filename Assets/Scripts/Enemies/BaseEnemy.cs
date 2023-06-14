using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BaseEnemy : MonoBehaviour
{
    public int fallDownPower;
    public int bouncePower;

    public virtual void OnDamge(int damage) { }

}
