using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using UnityEditor;


/* 
    Author:     fyw 
    CreateDate: 2018-01-23 17:08:39 
    Desc:       注释 
*/

[CustomEditor(typeof(Pointer_Bt300))]
    public class Pointer_Bt300Editor : Editor 
{

	 // target component
    public Pointer_Bt300 m_Component = null;
    static bool raycastLayerFoldout = false;
    int raycastLayersSize = 0;
    static bool tagsFoldout = false;
    int tagsSize = 0;

    public void OnEnable()
    {
        m_Component = (Pointer_Bt300)target;
        if (m_Component.raycastLayer != null)
            raycastLayersSize = m_Component.raycastLayer.Count;
        else raycastLayersSize = 0;
        if (m_Component.tags != null)
            tagsSize = m_Component.tags.Count;
        else tagsSize = 0;
    }
    public override void OnInspectorGUI()
    {
        bool changed = false;
        //======================
        //激活状态
        var oldActive = m_Component.active;
        m_Component.active = EditorGUILayout.Toggle("isActive", m_Component.active);
        if (oldActive != m_Component.active) changed = true;
        if (m_Component.active)
        {
            //空行
            EditorGUILayout.Space();
            //===可交互颜色
            var oldGoodSpotCol = m_Component.color;
            m_Component.color = EditorGUILayout.ColorField("Good Colour", m_Component.color);
            if (oldGoodSpotCol != m_Component.color) changed = true;
            //===不可交互颜色
            var oldBadSpotCol = m_Component.badColor;
            m_Component.badColor = EditorGUILayout.ColorField("Bad Colour", m_Component.badColor);
            if (oldBadSpotCol != m_Component.badColor) changed = true;

            //空行
            EditorGUILayout.Space();
            //标题栏
            EditorGUILayout.LabelField("-------------------------------------------------------");
            //标题栏
            EditorGUILayout.LabelField("Pointer Property");
         
            //==射线类型  连线，长方体
            var _pointerType = m_Component.pointerType;
            m_Component.pointerType = (Pointer_Bt300.PointTypeEnum)EditorGUILayout.EnumPopup("Poointer Type", m_Component.pointerType);
            if (_pointerType != m_Component.pointerType) changed = true;
            if(m_Component.pointerType == Pointer_Bt300.PointTypeEnum.Cube)
            {
                //=========线粗细
                var oldThickness = m_Component.thickness;
                m_Component.thickness = EditorGUILayout.FloatField("Pointer Thickness", m_Component.thickness);
                if (oldThickness != m_Component.thickness) changed = true;
                //===线长度
                var _PointerLength = m_Component.pointerLength;
                m_Component.pointerLength = EditorGUILayout.FloatField("Pointer Length", m_Component.pointerLength);
                if (_PointerLength != m_Component.pointerLength) changed = true;
            }
            else
            {
                //=========线粗细
                var _lineStartThickness = m_Component.lineStartThickness;
                m_Component.lineStartThickness = EditorGUILayout.FloatField("Start Line Thickness", m_Component.lineStartThickness);
                if (_lineStartThickness != m_Component.lineStartThickness) changed = true;
                //=========线粗细
                var _lineEndThickness = m_Component.lineEndThickness;
                m_Component.lineEndThickness = EditorGUILayout.FloatField("End Line Thickness", m_Component.lineEndThickness);
                if (_lineEndThickness != m_Component.lineEndThickness) changed = true;
                //===线起点透明度
                var _lineStartAlpha = m_Component.lineStartAlpha;
                m_Component.lineStartAlpha = EditorGUILayout.Slider("Start Line Alpha", m_Component.lineStartAlpha,0,1);
                if (_lineStartAlpha != m_Component.lineStartAlpha) changed = true;
                //===线终点透明度
                var _lineEndAlpha = m_Component.lineEndAlpha;
                m_Component.lineEndAlpha = EditorGUILayout.Slider("End Line Alpha", m_Component.lineEndAlpha,0,1);
                if (_lineEndAlpha != m_Component.lineEndAlpha) changed = true;

            }
          
           
            ////==发射点类型 默认方块，模型
            //==射线材质类型  默认圆柱体，材质球
            var oldArcMat = m_Component.pointerMatType;
            m_Component.pointerMatType = (Pointer_Bt300.PointerMaterialEnum)EditorGUILayout.EnumPopup("Material Type", m_Component.pointerMatType);
            if (oldArcMat != m_Component.pointerMatType) changed = true;
            if (m_Component.pointerMatType == Pointer_Bt300.PointerMaterialEnum.Material)
            {
                //==射线材质
                var oldGoodTeleMat = m_Component.PointerMaterial;
                m_Component.PointerMaterial = (Material)EditorGUILayout.ObjectField("Pointer Material", m_Component.PointerMaterial, typeof(Material), false);
                if (oldGoodTeleMat != m_Component.PointerMaterial) changed = true;
                //==材质缩放
                var oldMatScale = m_Component.matScale;
                m_Component.matScale = EditorGUILayout.FloatField("Material scale", m_Component.matScale);
                if (oldMatScale != m_Component.matScale) changed = true;
                //==材质贴图速度
                var oldTexMovementSpeed = m_Component.texMovementSpeed;
                m_Component.texMovementSpeed = EditorGUILayout.Vector2Field("Material Movement Speed", m_Component.texMovementSpeed);
                if (oldTexMovementSpeed != m_Component.texMovementSpeed) changed = true;
            }
            else
            {

            }

            //空行
            EditorGUILayout.Space();
            //标题栏
            EditorGUILayout.LabelField("-------------------------------------------------------");
            //标题栏
            EditorGUILayout.LabelField("PointerTip Property");
            //==是否使用 射线顶点
            var _showPointerTip = m_Component.showPointerTip;
            m_Component.showPointerTip = EditorGUILayout.Toggle("ShowPointerTip", m_Component.showPointerTip);
            if (_showPointerTip != m_Component.showPointerTip) changed = true;
            if(m_Component.showPointerTip)
            {
                //==射线顶点模型
                var _pointerTipType = m_Component.pointerTipType;
                m_Component.pointerTipType = (Pointer_Bt300.PointerTipEnum)EditorGUILayout.EnumPopup("Pointer Tip Type", m_Component.pointerTipType);
                if (_pointerTipType != m_Component.pointerTipType) changed = true;
                if (m_Component.pointerTipType == Pointer_Bt300.PointerTipEnum.Perfab)
                {
                    var _pointerTip = m_Component.pointerTipPerfab;
                    m_Component.pointerTipPerfab = (GameObject)EditorGUILayout.ObjectField("Pointer Tip Perfab", m_Component.pointerTipPerfab, typeof(GameObject), false);
                    if (_pointerTip != m_Component.pointerTipPerfab) changed = true;
                } 
            }
            ////==是否添加刚体
            //===========

            //======是否支持瞬移
            //空行
            EditorGUILayout.Space();
            //标题栏
            EditorGUILayout.LabelField("-------------------------------------------------------");
            //标题栏
            EditorGUILayout.LabelField("Teleport Property");
            //====开启 瞬移
            var _enableTeleport = m_Component.enableTeleport;
            m_Component.enableTeleport = EditorGUILayout.Toggle("EnableTeleport", m_Component.enableTeleport);
            if (_enableTeleport != m_Component.enableTeleport) changed = true;
            if (m_Component.enableTeleport==true)
            {
                //var oldLandOnFlat = m_Component.onlyLandOnFlat;
                //m_Component.onlyLandOnFlat = EditorGUILayout.Toggle("Only land on flat", m_Component.onlyLandOnFlat);
                //if (oldLandOnFlat != m_Component.onlyLandOnFlat) changed = true;
                //if (m_Component.onlyLandOnFlat)
                //{
                //    var oldSlopeLimit = m_Component.slopeLimit;
                //    m_Component.slopeLimit = EditorGUILayout.FloatField("Slope limit", m_Component.slopeLimit);
                //    if (oldSlopeLimit != m_Component.slopeLimit) changed = true;
                //}
                //===TAG判断 是否可瞬移
                var oldOnlyLandOnTag = m_Component.onlyLandOnTag;
                m_Component.onlyLandOnTag = EditorGUILayout.Toggle("Only land on tagged", m_Component.onlyLandOnTag);
                if (oldOnlyLandOnTag != m_Component.onlyLandOnTag) changed = true;
                if (m_Component.onlyLandOnTag)
                {
                    tagsFoldout = EditorGUILayout.Foldout(tagsFoldout, "Tags");
                    if (tagsFoldout)
                    {
                        EditorGUI.indentLevel++;
                        var oldTagSize = tagsSize;
                        tagsSize = EditorGUILayout.IntField("Size", tagsSize);
                        if (oldTagSize != tagsSize)
                        {
                            if (m_Component.tags == null) m_Component.tags = new System.Collections.Generic.List<string>();
                            changed = true;
                        }
                        if (tagsSize > m_Component.tags.Count)
                        {
                            int newFields = tagsSize - m_Component.tags.Count;
                            for (int i = 0; i < newFields; i++)
                                m_Component.tags.Add("");
                        }
                        else if (tagsSize < m_Component.tags.Count)
                        {
                            int fieldsToRemove = m_Component.tags.Count - tagsSize;
                            m_Component.tags.RemoveRange(m_Component.tags.Count - fieldsToRemove, fieldsToRemove);
                        }
                        for (int i = 0; i < tagsSize; i++)
                        {
                            var oldTag = m_Component.tags[i];
                            m_Component.tags[i] = EditorGUILayout.TextField("Element " + i, m_Component.tags[i]);
                            if (oldTag != m_Component.tags[i]) changed = true;
                        }
                        EditorGUI.indentLevel--;
                    }
                }
                raycastLayerFoldout = EditorGUILayout.Foldout(raycastLayerFoldout, "Raycast Layers");
                if (raycastLayerFoldout)
                {
                    EditorGUI.indentLevel++;
                    var oldRaycastLayersSize = raycastLayersSize;
                    raycastLayersSize = EditorGUILayout.IntField("Size", raycastLayersSize);
                    if (oldRaycastLayersSize != raycastLayersSize)
                    {
                        if (m_Component.raycastLayer == null) m_Component.raycastLayer = new System.Collections.Generic.List<string>();
                        changed = true;
                    }
                    if (raycastLayersSize > m_Component.raycastLayer.Count)
                    {
                        int newFields = raycastLayersSize - m_Component.raycastLayer.Count;
                        for (int i = 0; i < newFields; i++)
                            m_Component.raycastLayer.Add("");

                    }
                    else if (raycastLayersSize < m_Component.raycastLayer.Count)
                    {
                        int fieldsToRemove = m_Component.raycastLayer.Count - raycastLayersSize;
                        m_Component.raycastLayer.RemoveRange(m_Component.raycastLayer.Count - fieldsToRemove, fieldsToRemove);
                    }
                    for (int i = 0; i < raycastLayersSize; i++)
                    {
                        var oldRaycastLayer = m_Component.raycastLayer[i];
                        m_Component.raycastLayer[i] = EditorGUILayout.TextField("Element " + i, m_Component.raycastLayer[i]);
                        if (oldRaycastLayer != m_Component.raycastLayer[i]) changed = true;
                    }
                    EditorGUILayout.HelpBox("Leave raycast layers empty to collide with everything", MessageType.Info);
                    if (m_Component.raycastLayer != null && m_Component.raycastLayer.Count > 0)
                    {
                        var oldIgnoreRaycastLayer = m_Component.ignoreRaycastLayers;
                        m_Component.ignoreRaycastLayers = EditorGUILayout.Toggle("Ignore raycast layers", m_Component.ignoreRaycastLayers);
                        if (oldIgnoreRaycastLayer != m_Component.ignoreRaycastLayers) changed = true;
                        EditorGUILayout.HelpBox("Ignore raycast layers True: Ignore anything on the layers specified. False: Ignore anything on layers not specified", MessageType.Info);
                    }
                    EditorGUI.indentLevel--;
                }
            }


          

        }




        //public Color color;
        //public GameObject holder;
        //public GameObject pointer;
        //bool isActive = false;
        //public bool addRigidBody = false;
        //public Transform reference;
        //======================
        if (changed)
        {
            EditorUtility.SetDirty(m_Component);
            //m_Component.changeProperty = changed;
            m_Component.RebuildHolder();
        }
    }
}
