using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Vector2Int size;
    public Vector2 offset;
    public GameObject redBrickPrefab;
    public GameObject greenBrickPrefab;
    public GameObject blueBrickPrefab;
    public GameObject purpolBrickPrefab;

    private void Awake()
    {
        for ( int i = 0; i < size.x; i++ ) 
        { 
            for ( int j = 0; j < size.y; j++ )
            {
                GameObject newBrick = Instantiate( redBrickPrefab, transform );
                newBrick.transform.position = transform.position + new Vector3((float)((size.x - 1) * .5f - i) * offset.x, j * offset.y, 0);
            }
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
