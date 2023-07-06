using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtentions
{
    public static bool IsLayer(this GameObject go, LayerMask layer)
    {
        return layer == (layer | 1 << go.layer);
    }
}
