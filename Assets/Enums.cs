using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tags
{
    bullet,
    enemy,
    Player,
    wall
}

public enum EnemyStates
{
    NotMove,
    Patrol,
    Rotate,
    Chase
}
