using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BaseEnemy : MonoBehaviour
{
    public int fallDownPower;

    public int knockBackSwordPower;
    public int knockBackShieldPower;

    public virtual void OnDamge(int damage) { }

}
