using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [SerializeField] Tile dirtTileOne = null;
    [SerializeField] Tile dirtTileTwo = null;
    [SerializeField] Vector3Int[] tilePosChance = null;
    [SerializeField] int height = 8;
    [SerializeField] int width = 10;
    [SerializeField] int tilenum = 2;

    Tilemap tilemap = null;

    int tilesPlaced = 0;

    void Start()
    {
        tilenum = Random.Range(3, tilePosChance.Length + 1);

        tilemap = GetComponent<Tilemap>();

        for (int i = tilePosChance.Length - 1; i >= 0; i--)
        {
            if (tilesPlaced == tilenum)
            {
                break;
            }

            i = Random.Range(0, tilePosChance.Length);

            var rand = Random.Range(0, 2);

            if (rand == 0)
            {

                tilemap.SetTile(tilePosChance[i], dirtTileOne);
            }
            else
            {
                tilemap.SetTile(tilePosChance[i], dirtTileTwo);
            }

            tilesPlaced++;
        }

        //for (int i = 0; i < width; i++)
        //{
        //    var tile = tilemap.GetTile(new Vector3Int(i, 0, 0));

        //    //tilemap.GetComponent<TilemapCollider2D>().ClosestPoint();


        //}
    }

    private void Update()
    {

    }

}
