using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject WallPrefab;
    public GameObject FloorPrefab;
    public GameObject BedPrefab;
    public GameObject WardrobePrefab;
    public int GridSizeX;
    public int GridSizeY;
    public int GridSizeZ;
    public List<int> yCanGenerateInX = new List<int>();
    public List<int> yCanGenerateInZ = new List<int>();
    private Transform lastRoom;

    public int ChanceOfColumn1in1000X = 1;
    public int ChanceOfColumn1in1000Z = 1;
    private GameObject room;
    public int ColumnMaxAmount = 1;
    private int ColumnsSpawned;
    public int ColumnLengthMax = 3;
    public int ColumnLengthMin = 1;
    public int ColumnSizeMax = 3;
    public int ColumnSizeMin = 1;
    public bool ColumnsInZ;

    internal bool finishedGeneratingBed;
    internal bool finishedGeneratingWardRobe;

    public void GenerateRoom()
    {
        finishedGeneratingWardRobe = false;
        finishedGeneratingBed = false;
           ColumnsSpawned = 0;
        ColumnsInZ = Random.Range(0, 2) == 1;
        if (room != null)
        {
            Destroy(room);
        }
        StartCoroutine(GenerateRoomCoroutine());
    }

    private IEnumerator GenerateRoomCoroutine()
    {
        room = new GameObject();
        if (lastRoom == null)
        {
            room.transform.position = new Vector3(0, 0, 0);
        }

        room.name = "Room ";

        int YAmountOfRandom = Random.Range(0, GridSizeX);
        int ZAmountOfRandom = Random.Range(0, GridSizeZ);

        List<int> yCanGenerateInXAux = new List<int>();
        List<int> yCanGenerateInZAux = new List<int>(Random.Range(0, GridSizeZ));
        StartCoroutine(Generate(room.transform));
        lastRoom = room.transform;

        yield return null;
    }

    IEnumerator Generate(Transform parent)
    {
        for (int z = 0; z < GridSizeZ; z++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                int x = 0;
                GameObject obj = Instantiate(WallPrefab, Vector3.zero, Quaternion.identity, parent);
                obj.transform.localPosition = new Vector3(x, yCanGenerateInX.Contains(x) ? y : yCanGenerateInZ.Contains(z) || yCanGenerateInX.Count == 0 && yCanGenerateInZ.Count == 0 ? y : 0, z);
                obj.tag = "GridWallLeft";
                if ((ColumnsInZ ? Random.Range(0, 1000) <= ChanceOfColumn1in1000Z : Random.Range(0, 1000) <= ChanceOfColumn1in1000X) && (ColumnsInZ ? z == 0 : x == 0) && ColumnsSpawned < ColumnMaxAmount && (x == 0 ? z + ColumnLengthMax <= GridSizeZ : x + ColumnLengthMax <= GridSizeX))
                {

                    Debug.Log(x + " " + z);
                    ColumnsSpawned++;
                    int ColumnSize = Random.Range(ColumnSizeMin, ColumnSizeMax);
                    int ColumnLength = Random.Range(ColumnLengthMin, ColumnLengthMax);
                    for (int i = 0; i < GridSizeY; i++)
                    {
                        for (int l = 1; l < ColumnSize + 1; l++)
                        {
                            for (int s = 1; s < ColumnLength; s++)
                            {

                                GameObject objAux = Instantiate(WallPrefab, Vector3.zero, Quaternion.identity, parent);
                                objAux.transform.localPosition = new Vector3(!ColumnsInZ ? l : x + s, i, ColumnsInZ ? l : z + s);
                                objAux.tag = obj.tag;
                            }
                        }
                    }
                    yield return null;
                    ColumnsInZ = Random.Range(0, 2) == 1;
                }
            }
            for (int x = 0; x < GridSizeX; x++)
            {
                int y = 0;
                GameObject obj = Instantiate(FloorPrefab, Vector3.zero, Quaternion.identity, parent);
                obj.transform.localPosition = new Vector3(x, yCanGenerateInX.Contains(x) ? y : yCanGenerateInZ.Contains(z) || yCanGenerateInX.Count == 0 && yCanGenerateInZ.Count == 0 ? y : 0, z);
                obj.tag = "GridFloor";
            }
            yield return null;
        }

        for (int x = 0; x < GridSizeZ; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                int z = 0;
                GameObject obj = Instantiate(WallPrefab, Vector3.zero, Quaternion.identity, parent);
                obj.transform.localPosition = new Vector3(x, yCanGenerateInX.Contains(x) ? y : yCanGenerateInZ.Contains(z) || yCanGenerateInX.Count == 0 && yCanGenerateInZ.Count == 0 ? y : 0, z);
                obj.tag = "GridWallRight";
                if ((ColumnsInZ ? Random.Range(0, 1000) <= ChanceOfColumn1in1000Z : Random.Range(0, 1000) <= ChanceOfColumn1in1000X) && (ColumnsInZ ? z == 0 : x == 0) && ColumnsSpawned < ColumnMaxAmount && (x == 0 ? z + ColumnLengthMax <= GridSizeZ : x + ColumnLengthMax <= GridSizeX))
                {
                    Debug.Log(x + " " + z);
                    ColumnsSpawned++;
                    int ColumnSize = Random.Range(ColumnSizeMin, ColumnSizeMax);
                    int ColumnLength = Random.Range(ColumnLengthMin, ColumnLengthMax);
                    for (int i = 0; i < GridSizeY; i++)
                    {
                        for (int l = 1; l < ColumnSize + 1; l++)
                        {
                            for (int s = 1; s < ColumnLength; s++)
                            {

                                GameObject objAux = Instantiate(WallPrefab, Vector3.zero, Quaternion.identity, parent);
                                objAux.transform.localPosition = new Vector3(!ColumnsInZ ? l : x + s, i, ColumnsInZ ? l : z + s);
                                objAux.tag = obj.tag;
                            }
                        }
                    }
                    yield return null;
                    ColumnsInZ = Random.Range(0, 2) == 1;
                }
            }
            yield return null;
        }

        yield return StartCoroutine(SpawnBed(Random.Range(0, 2) == 0));
        yield return StartCoroutine(SpawnWardrobe(Random.Range(0, 2) == 0));
    }

    IEnumerator SpawnBed(bool BedInZ = false)
    {
        int min = 0;
        int maxZ = GridSizeZ - 8; //8 is bed width, can be changed after if we have diff types of beds
        int maxX = GridSizeX; //8 is bed width, can be changed after if we have diff types of beds

        int RandomStartPoint = BedInZ ? Random.Range(min, maxZ) : Random.Range(min, maxX);

        GameObject bed = Instantiate(BedPrefab, new Vector3(!BedInZ ? RandomStartPoint + 0.5f : 0, 2, BedInZ ? RandomStartPoint + 0.5f : 0), Quaternion.Euler(0, BedInZ ? 90 : 0, 0), room.transform);

        MovableObject bedComponent = bed.GetComponentInChildren<MovableObject>();

        int count = 0;
        while (true)
        {
            count++;
            if (bedComponent.InsideSomething)
            {

                bed.transform.position = new Vector3(!BedInZ ? Random.Range(min, maxX) + 0.5f : 0, 2, BedInZ ? Random.Range(min, maxZ) + 0.5f : 0);
            }
            if (count > 10)
            {
                if (bedComponent.InsideSomething)
                {
                    Destroy(bed);
                    StartCoroutine(SpawnBed(!BedInZ));
                }
                else
                {
                    finishedGeneratingBed = true;
                }
                break;
            }
            yield return new WaitForSeconds(0.02f);

        }
        yield return null;
    }

    IEnumerator SpawnWardrobe(bool WardrobeInZ = false)
    {
        int min = 0;
        int maxZ = GridSizeZ - 2;
        int maxX = GridSizeX - 2;

        int RandomStartPoint = WardrobeInZ ? Random.Range(min, maxZ) : Random.Range(min, maxX);

        GameObject wardRobe = Instantiate(WardrobePrefab, new Vector3(!WardrobeInZ ? RandomStartPoint + 0.5f : 0, 0.5f, WardrobeInZ ? RandomStartPoint + 0.5f : 0), Quaternion.Euler(0, WardrobeInZ ? 90 : 0, 0), room.transform);

        MovableObject wardrobeComponent = wardRobe.GetComponentInChildren<MovableObject>();

        int count = 0;
        while (true)
        {
            count++;
            if (wardrobeComponent.InsideSomething)
            {
                wardRobe.transform.position = new Vector3(!WardrobeInZ ? Random.Range(min, maxX) + 0.5f : 0, 0.5f, WardrobeInZ ? Random.Range(min, maxZ) + 0.5f : 0);
            }
            

            if (count > 10)
            {
                if (wardrobeComponent.InsideSomething)
                {
                    Destroy(wardRobe);
                    StartCoroutine(SpawnWardrobe(!WardrobeInZ));
                }
                else
                {
                    finishedGeneratingWardRobe = true;
                }
                break;
            }
            yield return new WaitForSeconds(0.02f);

        }

        yield return null;

    }
}
