using System.Collections.Generic;
using System.Linq;
using Assets.Prefabs;
using Pyramid_Defense.Common;
using UnityEngine;

public class AreaGrid : MonoBehaviour {

    public HexagonBasic StartHexagon { get; private set; }
    public HexagonBasic FinishHexagon { get; private set; }

    private List<HexagonBasic> hexagonsGraph;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateGrid(MapGridStructure mapGridStructure, bool symmetrical)
    {
        HexagonBasic[,] hexagonsGrid = new HexagonBasic[mapGridStructure.SizeX, mapGridStructure.SizeZ];
        hexagonsGraph = new List<HexagonBasic>();
        for (int x = 0; x < mapGridStructure.SizeX; ++x)
        {
            for (int z = 0; z < mapGridStructure.SizeZ; ++z)
            {
                switch (mapGridStructure.MapStructure[x, z])
                {
                    case MapElement.StandardHex:
                        hexagonsGrid[x, z] = CreateHexagon(mapGridStructure, x, z, mapGridStructure.MapStructure[x, z]);
                        break;
                    case MapElement.Start:
                        StartHexagon = hexagonsGrid[x, z] = CreateHexagon(mapGridStructure, x, z, mapGridStructure.MapStructure[x, z]);
                        break;
                    case MapElement.Finish:
                        FinishHexagon = hexagonsGrid[x, z] = CreateHexagon(mapGridStructure, x, z, mapGridStructure.MapStructure[x, z]);
                        break;
                    default:
                        hexagonsGrid[x, z] = null;
                        break;
                }
                if (hexagonsGrid[x, z] != null)
                    hexagonsGraph.Add(hexagonsGrid[x, z]);
            }
        }

        for (int x = 0; x < mapGridStructure.SizeX - 1; ++x)
        {
            for (int z = 0; z < mapGridStructure.SizeZ - 1; ++z)
            {
                if(hexagonsGrid[x, z] == null) 
                    continue;
                if (hexagonsGrid[x + 1, z] != null)
                {
                    hexagonsGrid[x, z].JoinHexagon(hexagonsGrid[x + 1, z]);
                }
                if (z + 1 < mapGridStructure.SizeZ)
                {
                    if (hexagonsGrid[x, z + 1] != null)
                    {
                        hexagonsGrid[x, z].JoinHexagon(hexagonsGrid[x, z + 1]);
                    }
                    if (z % 2 == 0)
                    {
                        if (x - 1 > 0 && hexagonsGrid[x - 1, z + 1] != null)
                        {
                            hexagonsGrid[x, z].JoinHexagon(hexagonsGrid[x - 1, z + 1]);
                        }
                    }
                    else
                    {
                        if (x + 1 < mapGridStructure.SizeX && hexagonsGrid[x + 1, z + 1] != null)
                        {
                            hexagonsGrid[x, z].JoinHexagon(hexagonsGrid[x + 1, z + 1]);
                        }
                    }
                }
            }
        }

        FindPath(true);
    }

    public bool FindPath(bool update)
    {
        float weight = 1f;

        List<HexagonBasic> resultSet = new List<HexagonBasic>(); 
        Dictionary<HexagonBasic, float> distances = new Dictionary<HexagonBasic, float>();
        List<HexagonBasic> hexagonsSet = new List<HexagonBasic>();
        Dictionary<HexagonBasic, HexagonBasic> hexagonPredecessors = new Dictionary<HexagonBasic, HexagonBasic>();

        foreach (HexagonBasic hexagonBasic in hexagonsGraph)
        {
            if(hexagonBasic.IsTraversable == false)
                continue;
            distances.Add(hexagonBasic, float.PositiveInfinity);
            hexagonPredecessors.Add(hexagonBasic, null);
        }
        hexagonsSet.Add(FinishHexagon);
        distances[FinishHexagon] = 0f;

        while (hexagonsSet.Count > 0)
        {
            hexagonsSet.Sort((a, b) => distances[a] > distances[b] ? 1 : -1);
            var hexagon = hexagonsSet.First();
            hexagonsSet.Remove(hexagon);
            resultSet.Add(hexagon);
            
            foreach (var neighbour in hexagon.HexagonsConnected)
            {
                if(resultSet.Contains(neighbour) || neighbour.IsTraversable == false)
                    continue;

                if (distances[hexagon] + weight < distances[neighbour])
                {
                    distances[neighbour] = distances[hexagon] + weight;
                    hexagonPredecessors[neighbour] = hexagon;
                    hexagonsSet.Add(neighbour);
                }
            }
        }

        HexagonBasic current = StartHexagon;
        if (!CheckIfPathExists(StartHexagon, FinishHexagon, hexagonPredecessors))
            return false;

        if (update)
        {
            ClearPath();
            while (current != null)
            {
                var currentModel = current.transform.GetChild(0);
                currentModel.parent = null;
                Destroy(currentModel.gameObject);

                var prefabNewModel = Resources.Load("Models\\hexagon_path");
                var newModelGameObject = Instantiate(prefabNewModel) as GameObject;
                var newModel = newModelGameObject.transform.GetChild(0);

                newModel.SetParent(current.transform);
                newModel.localPosition = Vector3.zero;

                newModelGameObject.transform.parent = null;
                Destroy(newModelGameObject);

                current = current.PathSuccessor = hexagonPredecessors[current];
            }
        }
        return true;
    }

    private bool CheckIfPathExists(HexagonBasic sourceHex, HexagonBasic destHex, Dictionary<HexagonBasic, HexagonBasic> hexagonPredecessors)
    {
        while (hexagonPredecessors[sourceHex] != null && sourceHex != destHex)
        {
            sourceHex = hexagonPredecessors[sourceHex];
        }

        if (sourceHex == destHex)
            return true;
        else
            return false;
    }

    private void ClearPath()
    {
        HexagonBasic current = StartHexagon;
        while (current != null)
        {
            var currentModel = current.transform.GetChild(0);
            currentModel.parent = null;
            Destroy(currentModel.gameObject);

            var prefabNewModel = Resources.Load("Models\\hexagon_basic");
            var newModelGameObject = Instantiate(prefabNewModel) as GameObject;
            var newModel = newModelGameObject.transform.GetChild(0);

            newModel.SetParent(current.transform);
            newModel.localPosition = Vector3.zero;

            newModelGameObject.transform.parent = null;
            Destroy(newModelGameObject);

            var tempHex = current;
            current = current.PathSuccessor;
            tempHex.PathSuccessor = null;
        }
    }

    private HexagonBasic CreateHexagon(MapGridStructure mapGridStructure, int x, int z, MapElement hexagonType)
    {
        var prefabHexagon = Resources.Load("Prefabs\\hexagon_basic");
        var hexagonGameObject = Instantiate(prefabHexagon) as GameObject;
        HexagonBasic hexagon = hexagonGameObject.GetComponent<HexagonBasic>();
        hexagon.transform.parent = gameObject.transform;
        hexagon.HexagonType = hexagonType;
        hexagon.IsTraversable = true;
        hexagon.OnMouseDownEvent += (object o) =>
        {
            var hex = o as HexagonBasic;
            if (hex != null && hex.IsTraversable)
            {
                hex.IsTraversable = false;
                if (!FindPath(true))
                    hex.IsTraversable = true;
            }
        };

        Vector3 origin = new Vector3(mapGridStructure.OffsetX, 0, mapGridStructure.OffsetZ);
        Vector3 transfrom = CalcTransformPosition(x, z);

        hexagon.gameObject.transform.position = new Vector3(origin.x + transfrom.x, origin.y + transfrom.y, origin.z + transfrom.z);

        return hexagon;
    }

    private static Vector3 CalcTransformPosition(int x, int z)
    {
        Vector3 transformVector = new Vector3();

        transformVector.x = x * HexagonBasic.SizeX;
        if (z % 2 == 1)
        {
            transformVector.x += HexagonBasic.SizeX * 0.5f;
        }
        transformVector.z = z * 0.75f * HexagonBasic.SizeZ;
        transformVector.y = 0;

        return transformVector;
    }
}
