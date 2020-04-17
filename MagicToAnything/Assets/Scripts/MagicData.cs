using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MagicData
{
    public string Name;
    public int Type;
    public float TModifier;

    public int Target;

    public int Effect;
    public float EModifier;

    public MagicData(string name, int type, float tmodifier, int target, int effect, float emodifier)
    {
        Name = name;
        Type = type;
        TModifier = tmodifier;
        Target = target;
        Effect = effect;
        EModifier = emodifier;
    }
}