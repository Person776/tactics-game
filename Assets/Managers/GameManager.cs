using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;
    private bool doneGenerating = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.GenerateGrid);
        
    }
    public void ChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                print(GameState);
                break;
            case GameState.SpawnHeroes:
                UnitManager.Instance.SpawnHeroes();
                print(GameState);
                break;
            case GameState.SpawnEnemies:
                UnitManager.Instance.SpawnEnemies();
                print(GameState);
                break;
            case GameState.HeroesTurn:
                print(GameState);
                break;
            case GameState.EnemiesTurn:
                print(GameState);
                break;
            default:
                throw new System.Exception();
        }
    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnHeroes = 1,
    SpawnEnemies = 2,
    HeroesTurn = 3,
    EnemiesTurn = 4
}
