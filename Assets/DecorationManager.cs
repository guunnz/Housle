using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationManager : MonoBehaviour
{

    public List<GameObject> decorationList;
    public List<GameObject> decorationSpawnedList;

    public RoomGenerator roomGenerator;

    public int DecorationAmount;

    public void Generate()
    {
        decorationSpawnedList.ForEach(x => Destroy(x));
        decorationSpawnedList.Clear();
        StartCoroutine(CGenerate());
    }

    public IEnumerator CGenerate()
    {
        while (!roomGenerator.finishedGeneratingWardRobe || !roomGenerator.finishedGeneratingBed)
        {
            yield return null;
        }

        decorationList.Sort((a, b) => 1 - 2 * Random.Range(0, 1));


        for (int i = 0; i < decorationList.Count; i++)
        {
            StartCoroutine(SpawnDecoration(decorationList[i]));
            yield return null;
        }
    }

    public IEnumerator SpawnDecoration(GameObject decorationPrefab)
    {
        int min = 0;
        int maxZ = roomGenerator.GridSizeZ - 2;
        int maxX = roomGenerator.GridSizeX - 2;

        int RandomStartPointX = Random.Range(min, maxZ);
        int RandomStartPointZ = Random.Range(min, maxX);

        Decoration decorComponent = decorationPrefab.GetComponent<Decoration>();
        GameObject decoration = Instantiate(decorationPrefab, new Vector3(RandomStartPointX, decorComponent.DecorationYExtra, RandomStartPointZ), Quaternion.Euler(decorationPrefab.transform.eulerAngles.x, Random.Range(0, 5) * 45, decorationPrefab.transform.eulerAngles.z), null);

        MovableObject decorationMovable = decoration.GetComponentInChildren<MovableObject>();

        int count = 0;
        while (true)
        {
            count++;
            if (decorationMovable.InsideSomething)
            {
                decoration.transform.position = new Vector3(Random.Range(min, maxZ), decorComponent.DecorationYExtra, Random.Range(min, maxX));
            }

            if (count > 10)
            {
                if (decorationMovable.InsideSomething)
                {
                    Destroy(decoration);
                    StartCoroutine(SpawnDecoration(decorationPrefab));
                }
                break;
            }
            yield return new WaitForSeconds(0.02f);

        }
        decorationSpawnedList.Add(decoration);
        yield return null;
    }
}
