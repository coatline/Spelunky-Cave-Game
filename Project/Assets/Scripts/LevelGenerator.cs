using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField] GameObject[] roomsLR;
    [SerializeField] GameObject[] roomsLRD;
    [SerializeField] GameObject[] roomsLRUD;

    [SerializeField] GameObject[] startingRooms;
    [SerializeField] GameObject[] endingRooms;
    [SerializeField] GameObject[] otherRooms;
    [SerializeField] Grid grid;

    public int levelWidth;
    public int levelHeight;

    int roomX;
    int roomY;

    int direction; // 1 / 2 = left  3 / 4 = right  5 = down

    int[,] gridMap;
    Vector2Int gridPos;

    int previousDirection;
    GameObject previousRoom;

    bool spawning = true;

    int rooms;

    int roomWidth = 10;
    int roomHeight = 8;

    void Awake()
    {
        gridMap = new int[levelWidth, levelHeight];
        roomX = Random.Range(0, levelWidth) * roomWidth;
        UpdateGridPos(1);

        while (spawning)
        {
            SpawnRoom();
        }

        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                if (gridMap[x, y] == 0)
                {
                    //Spawn room

                    var coords = new Vector3(x * roomWidth, -y * roomHeight);

                    var index = Random.Range(0, otherRooms.Length);

                    var newRoom = Instantiate(otherRooms[index], coords, Quaternion.identity, grid.transform);

                    Flip(newRoom);
                }
            }
        }
    }

    void UpdateGridPos(int roomType)
    {
        gridPos = new Vector2Int(roomX / roomWidth, (int)Mathf.Abs(roomY) / roomHeight);
        gridMap[gridPos.x, gridPos.y] = roomType;
    }

    void SpawnRoom()
    {
        direction = Random.Range(1, 8);

        // starting Room
        if (rooms == 0)
        {
            var index = Random.Range(0, startingRooms.Length);

            print(grid);
            var newRoom = Instantiate(startingRooms[index], new Vector3(roomX, roomY), Quaternion.identity, grid.transform);

            previousRoom = newRoom;

            Flip(newRoom);

            rooms++;
            UpdateGridPos(1);
            return;
        }

        // move right
        if (direction == 1 || direction == 2 || direction == 3)
        {
            if (previousDirection == 4 || previousDirection == 5 || previousDirection == 6)
            {
                return;
            }

            if (roomX == levelWidth * roomWidth - roomWidth)
            {
                //move down

                if (roomY == -levelHeight * roomHeight + roomHeight)
                {
                    Destroy(previousRoom);

                    var index = Random.Range(0, endingRooms.Length);

                    var newRoom = Instantiate(endingRooms[index], new Vector3(roomX, roomY), Quaternion.identity, grid.transform);

                    Flip(newRoom);

                    spawning = false;
                }
                else
                {
                    roomY -= roomHeight;

                    var index = Random.Range(0, roomsLRUD.Length);

                    if (rooms > 1)
                    {
                        Destroy(previousRoom.gameObject);

                        var newRoomThatYeah = Instantiate(roomsLRUD[index], new Vector3(roomX, roomY + roomHeight), Quaternion.identity, grid.transform);
                        Flip(newRoomThatYeah);
                    }


                    index = Random.Range(0, roomsLRUD.Length);

                    var newRoom = Instantiate(roomsLRUD[index], new Vector3(roomX, roomY), Quaternion.identity, grid.transform);

                    Flip(newRoom);

                    previousRoom = newRoom;
                }
            }
            else
            {

                roomX += roomWidth;

                var index = Random.Range(0, roomsLR.Length);

                var newRoom = Instantiate(roomsLR[index], new Vector3(roomX, roomY), Quaternion.identity, grid.transform);

                Flip(newRoom);

                previousRoom = newRoom;
            }
        }

        // move left
        else if (direction == 4 || direction == 5 || direction == 6)
        {
            if (previousDirection == 1 || previousDirection == 2 || previousDirection == 3)
            {
                return;
            }

            if (roomX == 0)
            {
                //move down

                if (roomY == -levelHeight * roomHeight + roomHeight)
                {
                    Destroy(previousRoom);

                    var index = Random.Range(0, endingRooms.Length);

                    Instantiate(endingRooms[index], new Vector3(roomX, roomY), Quaternion.identity, grid.transform);

                    spawning = false;
                }
                else
                {

                    roomY -= roomHeight;

                    var index = Random.Range(0, roomsLRUD.Length);

                    if (rooms > 1)
                    {
                        Destroy(previousRoom.gameObject);

                        var otherRoom = Instantiate(roomsLRUD[index], new Vector3(roomX, roomY + roomHeight), Quaternion.identity, grid.transform);
                        Flip(otherRoom);
                    }

                    index = Random.Range(0, roomsLRUD.Length);

                    var newRoom = Instantiate(roomsLRUD[index], new Vector3(roomX, roomY), Quaternion.identity, grid.transform);

                    Flip(newRoom);

                    previousRoom = newRoom;
                }
            }
            else
            {
                roomX -= roomWidth;

                var index = Random.Range(0, roomsLR.Length);

                var newRoom = Instantiate(roomsLR[index], new Vector3(roomX, roomY), Quaternion.identity, grid.transform);

                Flip(newRoom);

                previousRoom = newRoom;
            }
        }

        // move down
        else if (direction == 7)
        {
            if (roomY == -levelHeight * roomHeight + roomHeight)
            {
                Destroy(previousRoom);

                var index = Random.Range(0, endingRooms.Length);

                var otherRoom = Instantiate(endingRooms[index], new Vector3(roomX, roomY), Quaternion.identity, grid.transform);

                Flip(otherRoom);

                spawning = false;
            }
            else
            {
                roomY -= roomHeight;

                var index = 0;

                if (rooms > 1)
                {
                    Destroy(previousRoom.gameObject);

                    if (roomY == -roomHeight)
                    {
                        index = Random.Range(0, roomsLRD.Length);

                        var otherRoom = Instantiate(roomsLRD[index], new Vector3(roomX, roomY + roomHeight), Quaternion.identity, grid.transform);

                        Flip(otherRoom);
                    }

                    else
                    {
                        index = Random.Range(0, roomsLRUD.Length);

                        var otherRoom = Instantiate(roomsLRUD[index], new Vector3(roomX, roomY + roomHeight), Quaternion.identity, grid.transform);

                        Flip(otherRoom);
                    }
                }

                index = Random.Range(0, roomsLRUD.Length);

                var newRoom = Instantiate(roomsLRUD[index], new Vector3(roomX, roomY), Quaternion.identity, grid.transform);

                Flip(newRoom);

                previousRoom = newRoom;
            }
        }


        rooms++;

        previousDirection = direction;

        UpdateGridPos(1);
    }


    void Flip(GameObject room)
    {
        if (IsFlipped())
        {
            room.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    bool IsFlipped()
    {
        int rnd = Random.Range(0, 2);

        if (rnd == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
