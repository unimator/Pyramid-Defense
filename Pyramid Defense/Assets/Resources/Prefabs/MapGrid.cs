using UnityEngine;
using Pyramid_Defense.Common;

public class MapGrid : MonoBehaviour
{
    public string LevelXmlFileName;

    private MapGridStructure _mapGridStructure;

	// Use this for initialization
	void Start () { 
        _mapGridStructure = new MapGridStructure(LevelXmlFileName);

	    var areaGridPlayerA = GameObject.Find("AreaGridPlayerA").GetComponent<AreaGrid>();
	    if (areaGridPlayerA != null)
	    {
	        areaGridPlayerA.CreateGrid(_mapGridStructure, false);
	    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
