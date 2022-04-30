using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject GridPrefab;
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

    public void GenerateRoom()
    {
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
        StartCoroutine(GenerateV2(room.transform));
        lastRoom = room.transform;

        yield return null;
    }

    IEnumerator Generate(Transform parent)
    {
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                for (int z = 0; z < GridSizeZ; z++)
                {
                    GameObject obj = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, parent);
                    obj.transform.localPosition = new Vector3(x, yCanGenerateInX.Contains(x) ? y : yCanGenerateInZ.Contains(z) || yCanGenerateInX.Count == 0 && yCanGenerateInZ.Count == 0 ? y : 0, z);
                    yield return null;
                    if ((ColumnsInZ ? Random.Range(0, 1000) <= ChanceOfColumn1in1000Z : Random.Range(0, 1000) <= ChanceOfColumn1in1000X) && (ColumnsInZ ? z == 0 : x == 0) && ColumnsSpawned < ColumnMaxAmount && (x == 0 ? z + ColumnLengthMax < GridSizeZ : x + ColumnLengthMax < GridSizeX))
                    {
                        ColumnsInZ = Random.Range(0, 2) == 1;
                        ColumnsSpawned++;
                        int ColumnSize = Random.Range(ColumnSizeMin, ColumnSizeMax);
                        int ColumnLength = Random.Range(ColumnLengthMin, ColumnLengthMax);
                        for (int i = 0; i < GridSizeY; i++)
                        {
                            for (int l = 1; l < ColumnSize + 1; l++)
                            {
                                for (int s = 1; s < ColumnLength; s++)
                                {
                                    yield return null;
                                    GameObject objAux = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, parent);
                                    objAux.transform.localPosition = new Vector3(x == 0 ? l : x + s, i, z == 0 ? l : z + s);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator GenerateV2(Transform parent)
    {
        for (int z = 0; z < GridSizeZ; z++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                int x = 0;
                GameObject obj = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, parent);
                obj.transform.localPosition = new Vector3(x, yCanGenerateInX.Contains(x) ? y : yCanGenerateInZ.Contains(z) || yCanGenerateInX.Count == 0 && yCanGenerateInZ.Count == 0 ? y : 0, z);

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

                                GameObject objAux = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, parent);
                                objAux.transform.localPosition = new Vector3(!ColumnsInZ ? l : x + s, i, ColumnsInZ ? l : z + s);
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
                GameObject obj = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, parent);
                obj.transform.localPosition = new Vector3(x, yCanGenerateInX.Contains(x) ? y : yCanGenerateInZ.Contains(z) || yCanGenerateInX.Count == 0 && yCanGenerateInZ.Count == 0 ? y : 0, z);


            }
            yield return null;
        }

        for (int x = 0; x < GridSizeZ; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                int z = 0;
                GameObject obj = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, parent);
                obj.transform.localPosition = new Vector3(x, yCanGenerateInX.Contains(x) ? y : yCanGenerateInZ.Contains(z) || yCanGenerateInX.Count == 0 && yCanGenerateInZ.Count == 0 ? y : 0, z);

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

                                GameObject objAux = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, parent);
                                objAux.transform.localPosition = new Vector3(!ColumnsInZ ? l : x + s, i, ColumnsInZ ? l : z + s);
                            }
                        }
                    }
                    yield return null;
                    ColumnsInZ = Random.Range(0, 2) == 1;
                }
            }
            yield return null;
        }
    }
}
