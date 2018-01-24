using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using System.Collections.Generic;


/* 
    Author:     fyw 
    CreateDate: 2018-01-23 16:11:47 
    Desc:       注释 
*/  

namespace ffDevelopmentSpace
{
public class Pointer_Bt300 : Pointer_Base 
{
    #region enum
        public enum PointTypeEnum
        {
            Line,
            Cube
        }
        public enum PointerMaterialEnum
        {
            Material,
            Color
        }
        public enum PointerTipEnum
        {
            Sphere,
            Perfab
        }
    #endregion
    #region public property
        public PointTypeEnum pointerType = PointTypeEnum.Line;
        public float lineStartAlpha = 1;
        public float lineEndAlpha = 1;
        public float lineStartThickness = 0.02f;
        public float lineEndThickness = 0.02f;
        public PointerMaterialEnum pointerMatType= PointerMaterialEnum.Color;
        public Material PointerMaterial;
        // if material used the scale the texture should use
        public float matScale = 5;
        //	texture animated speed
        public Vector2 texMovementSpeed = Vector2.zero;
        public Color badColor;
        public bool showPointerTip = true;
        public PointerTipEnum pointerTipType=PointerTipEnum.Sphere;
        public GameObject pointerTipPerfab;
        private GameObject pointerTip;
        //public LayerMask layersToIgnore = Physics.IgnoreRaycastLayer;
        public float pointerLength = 100f;
        //=========================瞬移支持
        public bool enableTeleport = true;
        //public bool onlyLandOnFlat = true;
        ////Anywhere flat slot limit
        //public float slopeLimit = 20;
        public bool onlyLandOnTag = false;
        //Tags object has to have to be valid
        public List<string> tags = new List<string>();
        //Leave empty to collide with everything
        public List<string> raycastLayer = new List<string>();
        //Collide only with selected layers or only ignore selected layers
        public bool ignoreRaycastLayers = false;
        private LayerMask raycastLayerMask;
        //=============
        public TeleportType teleportType = TeleportType.TeleportTypeUseZeroY;
        public enum TeleportType
        {
            TeleportTypeUseTerrain,
            TeleportTypeUseCollider,
            TeleportTypeUseZeroY
        }
        Transform reference
        {
            get
            {
                // var top = SteamVR_Render.Top();
                // return (top != null) ? top.origin : null;
                return null;
            }
        }
    #endregion
	#region private property
     private LineRenderer _lineRenderer;
    private List<Vector3> linePositions = new List<Vector3>();
    private Material defalutMat;
    private Material defalutLineMat;
    #endregion

    #region unity function
    void OnEnable()
    {
    }
    void Start () 
	{
        OpenPointerBeam();
    }   
	void Update () 
	{
          if (pointer.gameObject.activeSelf)
        {
            Ray pointerRaycast = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            bool rayHit = false;// = Physics.Raycast(pointerRaycast, out hit, pointerLength, ~layersToIgnore);
            //================
            //	Check if we hit something
            //bool hitSomething = false;
            if (raycastLayer == null || raycastLayer.Count == 0)
                rayHit = Physics.Raycast(pointerRaycast, out hit, pointerLength);
            else
            {
                LayerMask raycastLayerMask = 1 << LayerMask.NameToLayer(raycastLayer[0]);
                for (int j = 1; j < raycastLayer.Count; j++)
                {
                    raycastLayerMask |= 1 << LayerMask.NameToLayer(raycastLayer[j]);
                }
                if (ignoreRaycastLayers) raycastLayerMask = ~raycastLayerMask;
                rayHit = Physics.Raycast(pointerRaycast, out hit, pointerLength, raycastLayerMask);
            }
            //=================
            var pointerBeamLength = GetPointerBeamLength(rayHit, hit);
            UpdatePointerTransform(pointerBeamLength, thickness);
        }
	}
    void OnDisable()
    {
    }
    void OnDestroy()
    {
    }
    #endregion

	#region public function
     public void OpenPointerBeam()
    {
        if (this.enabled && !active)
        {
            //setPlayAreaCursorCollision(false);
            if (holder == null) InitHolder();
            TogglePointer(true);
            active = true;
            //updateListenEvent();
            //addEvent();
        }
    }
    public void ClosePointerBeam(uint index)
    {
        if (active)
        {
            TogglePointer(false);
            active = false;
            //removeEvent();
        }
    }
    public void RebuildHolder()
    {
        if (holder == null) return;
        //changeProperty = false;
        Util.ClearChild(transform);
        InitHolder();
        // canUpdate = true;
    }
	#endregion
	#region private function
     private void InitHolder()
    {
        if(pointerMatType== PointerMaterialEnum.Color)
        {
           
        }
        else
        {
            if (PointerMaterial == null)
            {
                // canUpdate = false;
                return;
            }
            //PointerMaterial = 
        }

        holder = new GameObject(string.Format("[{0}]F_Pointer_Holder", this.gameObject.name));
        //Utilities.SetPlayerObject(pointerHolder, VRTK_PlayerObject.ObjectTypes.Pointer);
        holder.transform.parent = this.transform;
        holder.transform.localPosition = Vector3.zero;
        InitPointer();
        InitPointTip();
        SetPointerMaterial(color);
        UpdatePointerTransform(pointerLength, thickness);
        //TogglePointer(false);
    }
    private float GetPointerBeamLength(bool rayHit, RaycastHit hit)
    {
        var actualLength = pointerLength;

        //reset if beam not hitting or hitting new target
        if (!rayHit || (previousContact && previousContact != hit.transform))
        {
            PointerEventArgs args = new PointerEventArgs();
            // if (controller != null)
            // {
            //     args.controllerIndex = controller.controllerIndex;
            // }
            args.distance = 0f;
            args.flags = 0;
            args.target = previousContact;
            OnPointerOut(args);
            previousContact = null;
            //============================
            //pointerContactDistance = 0f;
            //destinationPosition = Vector3.zero;
            SetPointerMaterial(badColor);
        }
         //check if beam has hit a new target
        if (rayHit && previousContact != hit.transform)
        {
            PointerEventArgs argsIn = new PointerEventArgs();
            // if (controller != null)
            // {
            //     argsIn.controllerIndex = controller.controllerIndex;
            // }
            argsIn.distance = hit.distance;
            argsIn.flags = 0;
            argsIn.target = hit.transform;
            OnPointerIn(argsIn);
            previousContact = hit.transform;
            SetPointerMaterial(color);
        }

        //adjust beam length if something is blocking it
        if (rayHit && hit.distance < pointerLength)
        {
            actualLength = hit.distance;
        }
        return actualLength;
    }
     private void UpdatePointerTransform(float setLength, float setThicknes)
    {
        //Vector3 endPostion = new Vector3(0f, 0f, setLength );
        Vector3 endPostion = new Vector3(0f, 0f, setLength - (pointerTip.transform.localScale.z / 2));
        pointerTip.transform.localPosition = endPostion;
        holder.transform.localRotation = Quaternion.identity;
        if (pointerType == PointTypeEnum.Line)
        {
            List<Vector3> linePositions = new List<Vector3>();
            linePositions.Add(holder.transform.position);
            linePositions.Add(pointerTip.transform.position+ Vector3.forward * (pointerTip.transform.localScale.z / 2));
            _lineRenderer.SetVertexCount(linePositions.Count);
            _lineRenderer.SetPositions(linePositions.ToArray());
            if (pointerMatType == PointerMaterialEnum.Material)
            {
                if(_lineRenderer.material)
                {
                    _lineRenderer.material.mainTextureScale = new Vector2(matScale, 1);
                    _lineRenderer.material.mainTextureOffset = new Vector2(_lineRenderer.material.mainTextureOffset.x + texMovementSpeed.x, _lineRenderer.material.mainTextureOffset.y + texMovementSpeed.y);
                }
            }
        }
        else
        {
            //if the additional decimal isn't added then the beam position glitches
            var beamPosition = setLength / (2 + 0.00001f);
            pointer.transform.localScale = new Vector3(setThicknes, setThicknes, setLength);
            pointer.transform.localPosition = new Vector3(0f, 0f, beamPosition);
            if (pointerMatType == PointerMaterialEnum.Material)
            {
                if (pointer.GetComponent<Renderer>().material)
                {
                    pointer.GetComponent<Renderer>().material.mainTextureScale = new Vector2(matScale, 1);
                    pointer.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(pointer.GetComponent<Renderer>().material.mainTextureOffset.x + texMovementSpeed.x, pointer.GetComponent<Renderer>().material.mainTextureOffset.y + texMovementSpeed.y);
                }
            }
        }
    }
	private void InitPointer()
    {
        if(pointerType == PointTypeEnum.Line)
        {
            pointer = new GameObject("F_Pointer_LinePointer");
            pointer.transform.SetParent(holder.transform);
            _lineRenderer = pointer.AddComponent<LineRenderer>();
            //_lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _lineRenderer.SetWidth(lineStartThickness, lineEndThickness);
        }
        else
        {
            pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pointer.transform.name = string.Format("[{0}]F_Pointer_CubePointer", this.gameObject.name);
            pointer.transform.parent = holder.transform;
            pointer.GetComponent<BoxCollider>().isTrigger = true;
            pointer.AddComponent<Rigidbody>().isKinematic = true;
        }
        LayerManger.setLayer(pointer, LayerManger.IGNORE_RAYCAST);
    }
    private void InitPointTip()
    {
        if (showPointerTip == false)
        {
            pointerTip = new GameObject("F_Pointer_PointerTip");
            pointerTip.transform.SetParent(holder.transform);
        }
        else
        {
            if (pointerTipType == PointerTipEnum.Sphere)
            {
                pointerTip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                pointerTip.transform.name = string.Format("[{0}]F_Pointer_PointerTip", this.gameObject.name);
                pointerTip.GetComponent<SphereCollider>().isTrigger = true;
            }
            else
            {
                if (pointerTipPerfab == null)
                {
                    // canUpdate = false;
                    return;
                }
                else pointerTip = (GameObject)Instantiate(pointerTipPerfab, Vector3.zero, Quaternion.identity);
            }
           
            pointerTip.transform.parent = holder.transform;
            pointerTip.transform.localScale = Vector3.one * 0.3f;
            pointerTip.AddComponent<Rigidbody>().isKinematic = true;
        }
        LayerManger.setLayer(pointerTip, LayerManger.IGNORE_RAYCAST);
    }
   
    private void TogglePointer(bool state)
    {
        pointer.gameObject.SetActive(state);
        var tipState = (showPointerTip ? state : false);
        pointerTip.gameObject.SetActive(tipState);
    }
     private void SetPointerMaterial(Color color)
    {
       
      
        //=========================
        if (pointerType == PointTypeEnum.Line)
        {
            if (pointerMatType == PointerMaterialEnum.Material)
            {
                if (PointerMaterial == null) return;
                PointerMaterial.color = color;
                _lineRenderer.material = PointerMaterial;
                //_lineRenderer.material.mainTextureScale = new Vector2(matScale, 1);
                //_lineRenderer.material.mainTextureOffset = new Vector2(_lineRenderer.material.mainTextureOffset.x + texMovementSpeed.x, _lineRenderer.material.mainTextureOffset.y + texMovementSpeed.y);
            }
            if (pointerMatType == PointerMaterialEnum.Color)
            {
                if (defalutLineMat == null) defalutLineMat = new Material(Shader.Find("Sprites/Default"));
                pointer.GetComponent<Renderer>().material = defalutLineMat;
                Color startColor = color;
                startColor.a = lineStartAlpha;
                Color endColor = color;
                endColor.a = lineEndAlpha;
                _lineRenderer.SetColors(startColor, endColor);
                _lineRenderer.SetWidth(lineStartThickness, lineEndThickness);
            }
        }
        else
        {
            if (pointerMatType == PointerMaterialEnum.Material)
            {
                if (PointerMaterial == null) return;
                PointerMaterial.color = color;
                pointer.GetComponent<Renderer>().material = PointerMaterial;
                //pointer.GetComponent<Renderer>().material.mainTextureScale = new Vector2(matScale, 1);
                //pointer.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(pointer.GetComponent<Renderer>().material.mainTextureOffset.x + texMovementSpeed.x, pointer.GetComponent<Renderer>().material.mainTextureOffset.y + texMovementSpeed.y);
            }
            if(pointerMatType == PointerMaterialEnum.Color)
            {
                if (defalutMat == null) defalutMat = new Material(Shader.Find("Unlit/Color"));
                defalutMat.color = color;
                pointer.GetComponent<Renderer>().material = defalutMat;
            }
        }
        //=========================

        if (pointerMatType == PointerMaterialEnum.Color)
        {
            
            //_lineRenderer.SetColors(color, color);
        }
        else
        {
           
        }
        //if (showPointerTip) pointerTip.GetComponent<Renderer>().material = defalutMat;

    }
    #endregion

    #region event function
    public override void OnPointerIn(ffDevelopmentSpace.PointerEventArgs e)
    {
         base.OnPointerIn(e);
        //  SetObjectPointIn(e.target.gameObject);
    }
    public override void OnPointerOut(ffDevelopmentSpace.PointerEventArgs e)
    {
        base.OnPointerOut(e);
    }
  
    #endregion
}
}