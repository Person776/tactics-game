using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string tileName;
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;
    [SerializeField] private GameObject enemyHighlight;
    [SerializeField] public bool isWalkable;

    public int x, y;

    public Tile parent;
    public int gCost;
    public int hCost;
    public int FCost { get { return gCost + hCost; } }

    public BaseUnit occupiedUnit;
    public bool walkable => isWalkable && occupiedUnit == null;


    
    public virtual void Init(int x, int y)
    {
        
    }

    private void OnMouseEnter()
    {
        //highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    private void OnMouseExit()
    {
        //if ((occupiedUnit == null || occupiedUnit != UnitManager.Instance.selectedHero)) highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }


    BaseUnit hero;
    private void OnMouseDown()
    {
       // print("Mouse Down! " + "Occupied unit: " + occupiedUnit + " Faction: " + occupiedUnit.faction + " Game state: " + GameManager.Instance.GameState);
        
        if (GameManager.Instance.GameState != GameState.HeroesTurn) return;
        print("Selected!!!!!!");
        if (occupiedUnit != null)
        {
            foreach (var tile in GridManager.Instance.tiles)
            { tile.Value.highlight.SetActive(false); }

            if (occupiedUnit.faction == Faction.Hero)
            {

                //occupiedUnit.pathfinder.startTransform = transform;
                 hero = UnitManager.Instance.SetSelectedHero((BaseHero)occupiedUnit);
                //Keep the highlight on selected friendly unit and make it blue
                highlight.SetActive(true);
                highlight.GetComponent<Renderer>().material.color = Color.blue;

               

                //Highlight every enemy tile in red.
                foreach (var tile in GridManager.Instance.tiles)
                {
                    if (tile.Value.occupiedUnit != occupiedUnit) tile.Value.highlight.GetComponent<Renderer>().material.color = Color.white;
                    if (tile.Value.occupiedUnit != null && tile.Value.occupiedUnit.faction == Faction.Enemy) tile.Value.enemyHighlight.SetActive(true);
                    if (tile.Value.occupiedUnit != null && tile.Value.occupiedUnit != occupiedUnit) tile.Value.highlight.SetActive(false);
                    if (tile.Value.occupiedUnit == null && tile.Value.isWalkable == true && (Vector3.Distance(tile.Value.transform.position, this.transform.position) <= occupiedUnit.range))
                    {
                        tile.Value.highlight.SetActive(true);
                        //if (tile.Value.occupiedUnit != null) ; //tile.Value.highlight.GetComponent<Renderer>().material.color = Color.blue;
                    }
                }
            }
            else
            {
                if (hero != null) hero.pathfinder.goalTransform = transform;
                Attack((BaseEnemy)occupiedUnit);
                foreach (var tile in GridManager.Instance.tiles)
                {
                    if (tile.Value.occupiedUnit != null && tile.Value.occupiedUnit != occupiedUnit) tile.Value.highlight.SetActive(false);
                }
            }
        } 
        else
        {
           // highlight.SetActive(false);
            if (UnitManager.Instance.selectedHero != null && isWalkable == true && Vector3.Distance(UnitManager.Instance.selectedHero.transform.position, this.transform.position) <= UnitManager.Instance.selectedHero.range)
            {
                SetUnit(UnitManager.Instance.selectedHero);

                UnitManager.Instance.SetSelectedHero(null);
                highlight.SetActive(false);
                highlight.GetComponent<Renderer>().material.color = Color.white;
            }
        }
        //highlight.GetComponent<Renderer>().material.color = Color.white;

    }

    public void SetUnit(BaseUnit unit)
    {
        if (unit.occupiedTile != null)
        {
            unit.occupiedTile.highlight.SetActive(false);
            unit.occupiedTile.occupiedUnit = null;
        }
        foreach (var tile in GridManager.Instance.tiles)
        { tile.Value.highlight.SetActive(false);
            tile.Value.enemyHighlight.SetActive(false);
        }
        unit.transform.position = transform.position;
        occupiedUnit = unit;
        unit.occupiedTile = this;
    }


    private void Attack(BaseEnemy enemy)
    {
        if (UnitManager.Instance.selectedHero != null && occupiedUnit.faction == Faction.Enemy && Vector3.Distance(UnitManager.Instance.selectedHero.transform.position, enemy.transform.position) <= 1)
        { //Kill the enemy
            //var enemy = (BaseEnemy)occupiedUnit;
            enemy.health -= 5;
            enemyHighlight.SetActive(false); foreach (var tile in GridManager.Instance.tiles)
            {
                if (tile.Value.occupiedUnit != null && tile.Value.occupiedUnit.faction == Faction.Enemy) tile.Value.enemyHighlight.SetActive(false);
            }//occupiedUnit.occupiedTile.highlight.SetActive(false);
            print("Enemy health after hit: " + enemy.health);
            if (enemy.health <= 0)
            {
                Destroy(enemy.gameObject);
                
            }
            UnitManager.Instance.SetSelectedHero(null);

        }
        else
        {
            highlight.SetActive(false);
        }
    }
}

    