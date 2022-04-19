using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HittableType
{
    Chomper,
    Spitter,
    Gunner,
    Boss,
    Other
}

public interface IHittable
{
    
    public abstract HittableType OnHit(Collider sender);
}
