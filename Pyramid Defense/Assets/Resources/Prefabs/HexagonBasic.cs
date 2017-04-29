using System.Collections.Generic;
using Pyramid_Defense.Common;
using UnityEngine;

namespace Assets.Prefabs
{
    public class HexagonBasic : MonoBehaviour
    {
        public MapElement HexagonType;
        public bool IsTraversable;

        public List<HexagonBasic> HexagonsConnected = new List<HexagonBasic>();
        public HexagonBasic PathSuccessor = null;

        private static readonly float _sizeXBasic = 1.732f;
        public static float SizeX
        {
            get { return (1 + _gap) * _sizeXBasic; }
        }

        private static readonly float _sizeYBasic = 0.547f;
        public static float SizeY
        {
            get { return _sizeYBasic; }
        }

        private static readonly float _sizeZBasic = 2.0f;
        public static float SizeZ
        {
            get { return (1 + _gap) * _sizeZBasic; }
        }
         
        private static readonly float _gap = 0.05f;
        
        public delegate void MouseDownHandler(object o);

        public event MouseDownHandler OnMouseDownEvent;

        // Use this for initialization
        void Start () {
            gameObject.AddComponent<BoxCollider>();
        }
	
        // Update is called once per frame
        void Update (){
            
        }

        public void JoinHexagon(HexagonBasic other)
        {
            HexagonsConnected.Add(other);
            other.HexagonsConnected.Add(this);
        }

        void OnMouseDown()
        {
            if(OnMouseDownEvent != null)
                OnMouseDownEvent.Invoke(this);
        }
    }
}
