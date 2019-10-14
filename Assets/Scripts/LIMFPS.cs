using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LIMFPS : MonoBehaviour
{
    public int target = 50;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
    }

    void Update()
    {
        if (Application.targetFrameRate != target)
            Application.targetFrameRate = target;
    }
}
