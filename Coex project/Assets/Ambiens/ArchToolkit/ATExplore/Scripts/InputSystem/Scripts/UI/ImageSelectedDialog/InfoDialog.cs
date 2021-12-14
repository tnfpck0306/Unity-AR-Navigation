using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ArchToolkit.Utils.ArchMath;
using ArchToolkit.AnimationSystem;
using ArchToolkit.Utils;
using TMPro;

namespace ArchToolkit.InputSystem
{

    public class InfoDialog : SingletonPrefab<InfoDialog>
    {
        [Serializable]
        public class KeyVal
        {
            public KeyVal(string k, string v)
            {
                this.Key = k;
                this.Val = v;
            }
            [SerializeField]
            public string Key;
            [SerializeField]
            public string Val;
        }

        public GameObject wrapper;
        public RawImage img;
        public GameObject dataPrefab;
        public Action onCloseCallback;
        public List<GameObject> visibilityChecks;

        public float timeSinceOpening = 0;
        public void InitMenu( string title, string description, Texture2D txt, List<KeyVal> data, Action onClose, Transform positionAndRotation)
        {
            if (onCloseCallback != null)
                onCloseCallback();
            timeSinceOpening = Time.time;

            this.gameObject.SetActive(false);

            // Reset ALL
            this.gameObject.SetActive(true);
            
            this.onCloseCallback = null;

            dataPrefab.SetActive(false);

            // SET ELEMENT LOGICS
            this.onCloseCallback = onClose;

            this.GetComponent<ImageSelectedDialogAnimation>().SetTitleText(title);
            this.GetComponent<ImageSelectedDialogAnimation>().SetDescriptionText(description);

            this.img.texture = txt;

            var r = (float)txt.height / (float)txt.width;
            
            var h = this.img.GetComponent<RectTransform>().rect.width * r;
            this.img.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
            
            visibilityChecks.Clear();

            var checks = this.gameObject.GetComponentsInChildren<BoxCollider>();
            foreach (var c in checks) visibilityChecks.Add(c.gameObject);

            //visibilityChecks.Add(this.transform.Find("CanvasWrapper/Canvas/Title/Image/VisibilityObj").gameObject);
            //visibilityChecks.Add(this.transform.Find("CanvasWrapper/Canvas/Description/Image/VisibilityObj").gameObject);

            StartCoroutine(this.CreateDescription(data));

            //1. Reset rotation
            this.transform.rotation = Quaternion.Euler(Vector3.zero);
            //2. Set the right position
            this.transform.position = positionAndRotation.position;
            var handeRot = positionAndRotation.rotation.eulerAngles;
            
            //If the handle is parallel to the floor, adjust the rotation of the dialog
            if (handeRot.z > 60 && handeRot.z < 120 || handeRot.x > 60 && handeRot.x < 120 || handeRot.z > 240 && handeRot.z < 300 || handeRot.x > 240 && handeRot.x < 300)
            {
                //1. Align the dialog with the head forward:
                var f = ArchToolkitManager.Instance.visitor.Head.transform.forward;
                f = Vector3.ProjectOnPlane(f, Vector3.up);
                this.transform.right = -f;
                //2. Force the dialog to align with the floor
                var e = this.transform.rotation.eulerAngles;
                this.transform.rotation = Quaternion.Euler(new Vector3(0, e.y, 65));

            }
            //Else manage the vertical rotation based on the handle
            else
            {
                /*if(handeRot.y>40 && handeRot.y<120 ){
                    this.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
                }
                else if(handeRot.y<40 && handeRot.y>-300){
                    this.transform.rotation = Quaternion.Euler(new Vector3(0,-90,0));
                }
                //Fix vertical rotation based on the angle between the head and the right vector of the dialog
                var angle=Vector3.Angle(ArchToolkitManager.Instance.visitor.Head.transform.forward, this.transform.right);
                if(angle<90)
                {
                    var e =this.transform.rotation.eulerAngles;
                    this.transform.rotation = Quaternion.Euler(new Vector3(e.x,e.y-180,e.z));
                }*/
                var f = ArchToolkitManager.Instance.visitor.Head.transform.forward;
                f = Vector3.ProjectOnPlane(f, Vector3.up);
                this.transform.right = -f;
                //2. Force the dialog to align with the floor
                // var e =this.transform.rotation.eulerAngles;
                //this.transform.rotation = Quaternion.Euler(new Vector3(e.x,0,e.z));
            }

            this.CheckAllButtonsAvailable();

        }
        
        public IEnumerator CreateDescription(List<KeyVal> data)
        {
            foreach(Transform c in this.wrapper.transform)
            {
                if(c.gameObject!=this.dataPrefab)
                    GameObject.Destroy(c.gameObject);
            }
            yield return new WaitForEndOfFrame();
            

            if (data!=null && data.Count > 0)
            {
                foreach(var d in data)
                {
                    var dataUI=GameObject.Instantiate(this.dataPrefab, this.wrapper.transform);
                    dataUI.transform.Find("Key").GetComponent<TextMeshProUGUI>().text = d.Key;
                    dataUI.transform.Find("Val").GetComponent<TextMeshProUGUI>().text = d.Val;
                    visibilityChecks.Add(dataUI.transform.Find("VisibilityObj").gameObject);
                    dataUI.SetActive(true);
                    yield return new WaitForEndOfFrame();

                }
            }

            this.wrapper.SetActive(false);
            yield return new WaitForEndOfFrame();

            this.wrapper.SetActive(true);

            visibilityChecks.Clear();

            var checks = this.gameObject.GetComponentsInChildren<BoxCollider>();
            foreach (var c in checks) visibilityChecks.Add(c.gameObject);
        }

        public void Close()
        {
            //Debug.Log("Close");
            this.gameObject.SetActive(false);

            if (this.onCloseCallback != null) this.onCloseCallback();
        }

        private void Update()
        {
            if(!allButtonsAvailable)
            {
                var e =this.transform.rotation.eulerAngles;
                this.transform.rotation = Quaternion.Euler(new Vector3(e.x,e.y+20f*Time.deltaTime,e.z));
                CheckAllButtonsAvailable();
            }

            if (Time.time - timeSinceOpening > 30)
            {
                if (!CheckAllButtonsAvailable())
                {
                    this.Close();
                }
            }
        }
        private bool allButtonsAvailable=true;
        bool CheckAllButtonsAvailable()
        {
            var headPosition=ArchToolkitManager.Instance.visitor.Head.transform.position;
            
            this.allButtonsAvailable=true;
            RaycastHit hit;
            foreach(var b in this.visibilityChecks){
                if (b != null)
                {
                    if (Physics.Raycast(headPosition, b.transform.position - headPosition, out hit, 10))
                    {
                        if (hit.transform != b.transform)
                        {
                            this.allButtonsAvailable = false;
                        }
                    }
                }
                
            }
            return this.allButtonsAvailable;
            
        }
        

    }
}