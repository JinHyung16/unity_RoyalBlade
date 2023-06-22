using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    /// <summary>
    /// PoolDictionary에서 Pooling할 오브젝트들은 모두 PoolObject로 취급한다.
    /// 만약 Pooling할 오브젝트에 PoolObject가 컴포넌트로 없다면 강제로 컴포넌트에 부착한다.
    /// </summary>

    private string objName;
    public string Name
    {
        get
        {
            return this.objName;
        }
        set
        {
            this.objName = value;
        }
    }
    public void RemovePrefab()
    {
        this.gameObject.SetActive(false);
    }
}
