using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class WindowsGenerator : MonoBehaviour
{
    public RoomGenerator roomGenerator;
    public GameObject WindowPrefab;

    public int WindowLengthMax;
    public int WindowLengthMin;
    public int WindowHeightMax;
    public int WindowHeightMin;

    public int MinY;
    public int MinLength;
    private bool WindowOnZ;

    public int WindowsAmountMin = 1;
    public int WindowsAmountMax = 3;

    private List<GameObject> windowsGenerated = new List<GameObject>();

    public void GenerateWindows()
    {
        windowsGenerated.ForEach(x => Destroy(x));
        windowsGenerated.Clear();
        int windowsAmount = Random.Range(WindowsAmountMin, WindowsAmountMax);
        for (int i = 0; i < windowsAmount; i++)
        {
            GenerateWindow();
        }
    }

    void GenerateWindow()
    {
        bool Z = WindowOnZ = Random.Range(0, 2) == 0;
        int WindowHeight = Random.Range(WindowHeightMin, WindowHeightMax);
        int maxY = roomGenerator.GridSizeY - WindowHeight - 1;
        int HeightChosen = Random.Range(MinY, maxY);

        if (HeightChosen + WindowHeight > maxY)
        {
            GenerateWindow();
            return;
        }
        int MaxPos = 0;
        int MinPos = 1;

        int LengthChosen = Random.Range(WindowLengthMin, WindowLengthMax);

        MaxPos = (Z ? roomGenerator.GridSizeZ : roomGenerator.GridSizeX) - LengthChosen;

        int PosChosen = Random.Range(MinPos, MaxPos);

        GameObject window = Instantiate(WindowPrefab, new Vector3(Z ? 0 : PosChosen, HeightChosen, Z ? PosChosen : 0), Quaternion.identity, null);

        window.transform.localScale = new Vector3(Z ? 1 : LengthChosen, WindowHeight, Z ? LengthChosen : 1);
        windowsGenerated.Add(window);
    }
}