using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;


/* 
    Author:     fyw 
    CreateDate: 2018-01-23 15:59:23 
    Desc:       手机  指示线发射器
*/  

namespace ffDevelopmentSpace
{
        public struct PointerEventArgs
        {
            public uint controllerIndex;
            public uint flags;
            public float distance;
            public Transform target;
        }
        public delegate void PointerEventHandler(object sender, PointerEventArgs e);
        public class Pointer_Base : MonoBehaviour 
        {

            #region public property
                public bool active = true;
                public Color color;
                public float thickness = 0.002f;
                public GameObject holder;
                public GameObject pointer;
                // bool isActive = false;
                // public bool addRigidBody = false;
                public Transform reference;
                public event PointerEventHandler PointerIn;
                public event PointerEventHandler PointerOut;
            #endregion
            #region private property
              protected Transform previousContact = null;

            #endregion

            #region unity function
            void OnEnable()
            {
            }
            void Start () 
            {
                holder = new GameObject();
                holder.transform.parent = this.transform;
                holder.transform.localPosition = Vector3.zero;
                holder.transform.localRotation = Quaternion.identity;

                pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
                pointer.transform.parent = holder.transform;
                pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
                pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
                pointer.transform.localRotation = Quaternion.identity;
                // BoxCollider collider = pointer.GetComponent<BoxCollider>();
                // if (addRigidBody)
                // {
                //     if (collider)
                //     {
                //         collider.isTrigger = true;
                //     }
                //     Rigidbody rigidBody = pointer.AddComponent<Rigidbody>();
                //     rigidBody.isKinematic = true;
                // }
                // else
                // {
                //     if(collider)
                //     {
                //         Object.Destroy(collider);
                //     }
                // }
                Material newMaterial = new Material(Shader.Find("Unlit/Color"));
                newMaterial.SetColor("_Color", color);
                pointer.GetComponent<MeshRenderer>().material = newMaterial;
            }   
            void Update () 
            {
                // if (!isActive)
                // {
                //     isActive = true;
                //     this.transform.GetChild(0).gameObject.SetActive(true);
                // }

                float dist = 100f;

                // SteamVR_TrackedController controller = GetComponent<SteamVR_TrackedController>();

                Ray raycast = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                bool bHit = Physics.Raycast(raycast, out hit);

                if(previousContact && previousContact != hit.transform)
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
                }
                if(bHit && previousContact != hit.transform)
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
                }
                if(!bHit)
                {
                    previousContact = null;
                }
                if (bHit && hit.distance < 100f)
                {
                    dist = hit.distance;
                }

                // pointer.transform.localScale = new Vector3(thickness * 5f, thickness * 5f, dist);
                pointer.transform.localScale = new Vector3(thickness, thickness, dist);
                pointer.transform.localPosition = new Vector3(0f, 0f, dist/2f);
            }
            void OnDisable()
            {
            }
            void OnDestroy()
            {
            }
            #endregion

            #region public function
            #endregion
            #region private function
            #endregion

            #region event function
                public virtual void OnPointerIn(PointerEventArgs e)
                {
                    if (PointerIn != null)
                        PointerIn(this, e);
                }

                public virtual void OnPointerOut(PointerEventArgs e)
                {
                    if (PointerOut != null)
                        PointerOut(this, e);
                }

            #endregion
        }
}
