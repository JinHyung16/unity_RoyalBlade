using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresenterSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// 각 Scene에서 Presenter들이 상속받아 사용한다.
    /// Singleton 타입으로, 만약 이를 사용히
    /// Presenter를 참조하는 Class들은 Awake(), OnDisalbe()에서 Presenter들을 호출하는 일은 없어야한다.
    /// </summary>
    
    private static T instance;
    public static T GetInstance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }

        OnAwake();
    }

    protected virtual void OnAwake()
    {
    }

}
