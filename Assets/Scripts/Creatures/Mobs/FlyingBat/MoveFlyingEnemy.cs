using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFlyingEnemy : Creature
{
   
    protected override float CalculateYVelocity()
    {
        return Direction.y * CalculateSpeed();

    }
}
