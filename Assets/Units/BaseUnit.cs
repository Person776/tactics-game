using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public Tile occupiedTile;
    [SerializeField] int moveRange;
    public Faction faction;
    public string unitName;
    public static int MAX_HEALTH = 10;
    public int health = MAX_HEALTH;
    public int range;

    public Pathfinding pathfinder;

    public object Pathfinding { get; internal set; }

    private void Awake()
    {
        pathfinder.startTransform = transform;
    }
}
