using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HexGrid : MonoBehaviour
{
    public TextMeshProUGUI cellLabelPrefab;
    int cellCountX, cellCountZ;

    public HexCell cellPrefab;

    HexCell[] cells;
    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    public Texture2D noiseSource;

    public int chunkCountX = 4, chunkCountZ = 3;
    public HexGridChunk chunkPrefab;//As HexGrid will be instantiating these chunks
    HexGridChunk[] chunks;


    private void Awake()
    {
        HexMetrics.noiseSource = noiseSource;
        cellCountX = chunkCountX * HexMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;
        CreateChunks();
        CreateCells();

    }

    private void OnEnable()
    {
        HexMetrics.noiseSource = noiseSource;
    }

    private void CreateChunks()
    {
        chunks = new HexGridChunk[chunkCountX * chunkCountZ];

        for (int z = 0, i = 0; z < chunkCountZ; z++)
        {
            for (int x = 0; x < chunkCountX; x++)
            {
                HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
            }
        }
    }

    private void CreateCells()
    {
        cells = new HexCell[cellCountZ * cellCountX];

        for (int z = 0, i = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + 0.5f * z - z / 2) * (HexMetrics.innerRadius * 2f);  // z/2整除 没有分数部分
        position.y = 0f;
        position.z = z * (HexMetrics.outRadius * 1.5f);


        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        //cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x - z/2, z);
        cell.color = defaultColor;


        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0) //& is the bitwise AND operator.
                              //It performs the same logic, but on each individual pair of bits of its operands.
                              //So both bits of a pair need to be 1 for the result to be 1.
                              //For example, 10101010 & 00001111 yields 00001010.
                              //偶数行
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
                if (x < cellCountX - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
                }

            }
        }

        TextMeshProUGUI label = Instantiate<TextMeshProUGUI>(cellLabelPrefab);
        //label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();

        cell.uiRect = label.rectTransform;
        cell.Elevation = 0;

        AddCellToChunk(x, z, cell);
    }

    private void AddCellToChunk(int x, int z, HexCell cell)
    {
        int chunkX = x / HexMetrics.chunkSizeX;
        int chunkZ = z / HexMetrics.chunkSizeZ;// integer division

        HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];
        //the cell's index local to its chunk
        int localX = x - chunkX * HexMetrics.chunkSizeX;
        int localZ = z - chunkZ * HexMetrics.chunkSizeZ;

        chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
        return cells[index];
    }

}
