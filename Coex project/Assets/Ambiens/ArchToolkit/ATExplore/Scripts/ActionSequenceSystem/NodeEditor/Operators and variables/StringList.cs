using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;
using UnityEngine.UI;

namespace ambiens.archtoolkit.atexplore.actionSystem
{

    public delegate void Callback(object ele);

    [CreateNodeMenuAttribute("Variables/String List")]
    public class StringList : Node
    {
        [Input]
        public List<string> list;

        [Output]
        public List<string> output;

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "output")
            {
                var n = this.GetInputPort("list");
                if (n.IsConnected)
                {
                    var lists = this.GetInputValues<List<string>>("list");
                    this.output.Clear();

                    foreach(var l in lists)
                    {
                        this.output.AddRange(l);
                    }
                }
                return this.output;
            }
            else return new List<string>();
        }
    }
}
