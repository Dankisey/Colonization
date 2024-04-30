﻿using System;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    private int _resources = 0;

    public event Action<int> ResourcesAmountChanged;

    private void Start()
    {
        ResourcesAmountChanged?.Invoke(_resources);
    }

    public void Store(Resource resource)
    {
        _resources++;
        ResourcesAmountChanged?.Invoke(_resources);
        resource.ReturnToPool();
    }
}