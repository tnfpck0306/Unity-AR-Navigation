using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ArchToolkit.Utils.ArchMath;
using ArchToolkit.AnimationSystem;
using ArchToolkit.Utils;

namespace ArchToolkit.InputSystem
{

    public class ImageSelectedDialog : SingletonPrefab<ImageSelectedDialog>
    {
        public GameObject versionCaretLeft;
        public GameObject versionCaretRight;
        public GameObject wrapper;
        public Action onCloseCallback;
        public List<ArchUIButton> buttons = new List<ArchUIButton>();
        private Action<Variant> clickCallback;
        private List<Variant> itemsSelected = new List<Variant>();
        private Variant CurrentSelectedItem = null;
        private int maxPages = 0;
        private int currentVersionsPage = 0;
        private const int _versionsPerPage = 3;
        public List<GameObject> visibilityChecks;

        private int currSelIndex = -1;
        public float timeSinceOpening = 0;
        public void InitMenu(int currSelectIndex, List<Variant> elementsLogics, Action<Variant> onClick, Action onClose, Transform positionAndRotation)
        {
            if (onCloseCallback != null)
                onCloseCallback();
            timeSinceOpening = Time.time;
            this.gameObject.SetActive(false);

            // Reset ALL
            this.gameObject.SetActive(true);
            this.maxPages = 0;
            this.clickCallback = null;
            this.onCloseCallback = null;


            // SET ELEMENT LOGICS
            //this.prevButton = RayHeadPointer.Instance.prevButton;
            this.itemsSelected = elementsLogics;
            this.maxPages = (int)(this.itemsSelected.Count / _versionsPerPage);

            this.clickCallback = onClick;
            this.onCloseCallback = onClose;

            // SET ELEMENT GRAPHICS
            if (itemsSelected.Count >= 1)
            {

                this.GetComponent<ImageSelectedDialogAnimation>().SetTitleText(itemsSelected[0].name);
                this.GetComponent<ImageSelectedDialogAnimation>().SetDescriptionText(itemsSelected[0].description);

            }

            this.currSelIndex = currSelectIndex;

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

            if (currSelIndex == -1)
                this.currentVersionsPage = 0;
            else
                this.currentVersionsPage = (int)(currSelIndex / _versionsPerPage);

            CreateVariantButtons();

            visibilityChecks.Clear();

            var checks = this.gameObject.GetComponentsInChildren<BoxCollider>();
            foreach (var c in checks) visibilityChecks.Add(c.gameObject);

            this.CheckAllButtonsAvailable();

        }

        void SetLastSelected()
        {
            //this.title.text = itemsSelected[currSelIndex].name; ENNIO
            //this.description.text = itemsSelected[currSelIndex].description; ENNIO
            this.GetComponent<ImageSelectedDialogAnimation>().SetTitleText(itemsSelected[currSelIndex].name);
            this.GetComponent<ImageSelectedDialogAnimation>().SetDescriptionText(itemsSelected[currSelIndex].description);

            foreach (var b in this.buttons)
            {
                if (b.selItem == itemsSelected[currSelIndex])
                {
                    b.isSelected = true;
                    break;
                }
            }
        }

        public void OnButtonClick(Variant item)
        {

            foreach (var button in this.buttons)
            {
                if (button.selItem == item)
                    button.isSelected = true;
                else
                    button.isSelected = false;
            }


            this.CurrentSelectedItem = item;

            //this.description.text = CurrentSelectedItem.description;
            //this.title.text = CurrentSelectedItem.name;
            this.GetComponent<ImageSelectedDialogAnimation>().SetTitleText(CurrentSelectedItem.name);
            this.GetComponent<ImageSelectedDialogAnimation>().SetDescriptionText(CurrentSelectedItem.description);

            if (this.clickCallback != null)
                this.clickCallback(CurrentSelectedItem);
        }

        public void Close()
        {
            this.gameObject.SetActive(false);

            if (this.onCloseCallback != null) this.onCloseCallback();
        }

        private void Update()
        {
            /*if (!allButtonsAvailable)
            {
                var e = this.transform.rotation.eulerAngles;
                this.transform.rotation = Quaternion.Euler(new Vector3(e.x, e.y + 30f * Time.deltaTime, e.z));
                CheckAllButtonsAvailable();
            }*/

            if (Time.time - timeSinceOpening > 30)
            {
                if (!CheckAllButtonsAvailable())
                {
                    this.Close();
                }
            }

        }
        private bool allButtonsAvailable = true;
        bool CheckAllButtonsAvailable()
        {
            var headPosition = ArchToolkitManager.Instance.visitor.Head.transform.position;

            this.allButtonsAvailable = true;
            RaycastHit hit;
            foreach (var b in this.visibilityChecks)
            {
                if (b != null)
                {
                    if (Physics.Raycast(headPosition, b.transform.position - headPosition, out hit, 10))
                    {
                        if (hit.transform != b.transform)
                        {

                            this.allButtonsAvailable = false;
                        }
                        // else Debug.Log("REACHED " + hit.transform.name, hit.transform);
                    }
                }
                
            }
            /*
            foreach (var b in this.buttons)
            {
                if (Physics.Raycast(headPosition, b.transform.position - headPosition, out hit, 10))
                {
                    if (hit.transform != b.transform)
                    {
                        //Debug.Log("NOT REACHED "+b.transform.name,b.transform);
                        //Debug.Log("REACHED "+hit.transform.name,hit.transform);

                        this.allButtonsAvailable = false;
                    }
                }
            }
            if (versionCaretLeft.activeSelf)
            {
                if (Physics.Raycast(headPosition, versionCaretLeft.transform.position - headPosition, out hit, 10))
                {
                    if (hit.transform != versionCaretLeft.transform)
                    {
                        //Debug.Log("NOT REACHED "+versionCaretLeft.transform.name,versionCaretLeft.transform);
                        //Debug.Log("REACHED "+hit.transform.name,hit.transform);

                        this.allButtonsAvailable = false;
                    }
                }
            }
            if (versionCaretRight.activeSelf)
            {
                if (Physics.Raycast(headPosition, versionCaretRight.transform.position - headPosition, out hit, 10))
                {
                    if (hit.transform != versionCaretRight.transform)
                    {
                        //Debug.Log("NOT REACHED "+versionCaretRight.transform.name,versionCaretRight.transform);
                        //Debug.Log("REACHED "+hit.transform.name,hit.transform);

                        this.allButtonsAvailable = false;
                    }
                }
            }
            */
            return this.allButtonsAvailable;
            //Debug.Log("All Buttons available "+allButtonsAvailable);
        }
        /*private void SetPositionVR()
        {
            if (ArchToolkitManager.Instance.managerContainer.archToolkitVRManager == null)
            {
                this.followVRPad = false;
                return;
            }

            var leftController = ArchToolkitManager.Instance.managerContainer.archToolkitVRManager.leftController;
            if (leftController != null)
            {
                this.followVRPad = true;
                this.transform.SetParent(leftController.uiAnchor.transform);
                this.transform.localPosition = new Vector3(0, 0.25f, 0);
                this.transform.rotation = leftController.uiAnchor.transform.rotation * Quaternion.Euler(0, 180, 0);
                this.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            }
            else
                this.followVRPad = false;
        }*/

        public void ViewNextPage()
        {
            this.currentVersionsPage = Math.Min(this.currentVersionsPage + 1, maxPages);
            CreateVariantButtons();
        }

        public void ViewPrevPage()
        {
            this.currentVersionsPage = Math.Max(this.currentVersionsPage - 1, 0);
            CreateVariantButtons();
        }

        private void CheckCaret(int maxIndex)
        {
            this.versionCaretLeft.SetActive((this.currentVersionsPage > 0));
            this.versionCaretRight.SetActive((maxIndex < this.itemsSelected.Count));
            /*if (this.currentVersionsPage > 0)
            {
                
                this.versionCaretLeft.GetComponent<Image>().enabled = true;
                this.versionCaretLeft.GetComponent<Collider>().enabled = true;
            }
            else
            {
                
                this.versionCaretLeft.GetComponent<Image>().enabled = false;
                this.versionCaretLeft.GetComponent<Collider>().enabled = false;
            }

            if (maxIndex < this.itemsSelected.Count)
            {
                
                this.versionCaretRight.GetComponent<Image>().enabled = true;
                this.versionCaretRight.GetComponent<Collider>().enabled = true;
            }
            else
            {

                this.versionCaretRight.GetComponent<Image>().enabled = false;
                this.versionCaretRight.GetComponent<Collider>().enabled = false;
            }*/
        }

        private void CreateVariantButtons()
        {

            if (this.itemsSelected.Count > 0)
            {
                wrapper.gameObject.SetActive(true);

                int minIndex = currentVersionsPage * _versionsPerPage;
                int maxIndex = Math.Min(minIndex + _versionsPerPage, this.itemsSelected.Count);

                //this.versionCaretLeft.SetActive((this.currentVersionsPage > 0));
                //this.versionCaretRight.SetActive((maxIndex < this.itemsSelected.Count));
                CheckCaret(maxIndex);

                foreach (Transform child in wrapper.transform)
                {
                    if (child.transform.name != versionCaretLeft.name && child.transform.name != versionCaretRight.name)
                    {
                        Destroy(child.gameObject);
                        buttons.Clear();
                    }

                }

                for (int count = minIndex; count < maxIndex; count++)
                {
                    var gO = GameObject.Instantiate(Resources.Load<GameObject>("UI/ImageSelectButton"));

                    gO.transform.SetParent(wrapper.transform);
                    gO.transform.localPosition = Vector3.zero;
                    gO.transform.localScale = Vector3.one;
                    gO.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    var isb = gO.GetComponent<ArchUIButton>();

                    buttons.Add(isb);

                    isb.InitButton(this.itemsSelected[count], OnButtonClick);
                }

                //this.currSelIndex = minIndex ;

                // if (currSelIndex != -1)
                //     SetLastSelected();

                versionCaretLeft.transform.SetAsFirstSibling();
                versionCaretRight.transform.SetAsLastSibling();
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

        /*private void SetPosition(float lerp)
        {
            var vector = this.lookAt.forward;

            var distance = (
                (this.lookAt.position + this.lookAt.forward * this.radius)
                - this.transform.position).sqrMagnitude;

            Debug.DrawLine(this.lookAt.position, this.lookAt.position + this.lookAt.forward * this.radius * 2, Color.white);

            var projection = Math3d.ProjectVectorOnPlane(this.planeNormal, vector);
            var angle = Math3d.AngleVectorPlane(vector, this.planeNormal);
            if ((angle > minAngleToStopFollow || distance > this.maxSqrDist))
            {
                var finalPos = this.lookAt.position + projection * this.radius + heightOfInterface * Vector3.up;
                this.transform.LookAt(this.lookAt);
                this.transform.position = Vector3.Lerp(this.transform.position, finalPos, Time.deltaTime * 3f);
                //if (Vector3.Distance(this.transform.position, finalPos) < 0.1f)
                //{
                //    
                //}

            }
            //Debug.Log(angle);
            //Debug.DrawLine(this.lookAt.position, this.lookAt.position + projection * this.radius, Color.red);

        }*/
    }
}