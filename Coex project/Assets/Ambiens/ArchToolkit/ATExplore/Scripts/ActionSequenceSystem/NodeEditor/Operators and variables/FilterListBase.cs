using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;
using UnityEngine.UI;

namespace ambiens.archtoolkit.atexplore.actionSystem
{

    public abstract class FilterListBase<T> : Node
    {
        [Input]
        public List<T> list;
        [Input]
        public int index;
        [Output]
        public string output;
        
        public override object GetValue(NodePort port)
        {

            if (port.fieldName == "output")
            {
                var n = this.GetInputPort("list");
                if (n.IsConnected)
                {
                    this.list = this.GetInputValue<List<T>>("list");
                    
                }
                var i = this.GetInputPort("index");
                if (i.IsConnected)
                {
                    this.index = this.GetInputValue<int>("index");
                }

                if(this.list!=null && this.index<this.list.Count)
                    return this.list[this.index];
                else return "";
            }
            else return "";
        }

    }
}
