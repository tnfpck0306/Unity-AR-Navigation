using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArchToolkit.MenuSystem{
    public class HeightSettingsUI : MonoBehaviour
    {
        public Text heightIndicator;

        // Start is called before the first frame update
        void Start()
        {
            this.RefreshIndicator();
        }

        public void AddHeight(float amount){
            ArchToolkitManager.Instance.visitor.HeightDifference=ArchToolkitManager.Instance.visitor.HeightDifference+amount;
            this.RefreshIndicator();
        }
        public void RefreshIndicator(){
            this.heightIndicator.text=ArchToolkitManager.Instance.visitor.HeightDifference.ToString("F2")+"m";
        }
    }

}
