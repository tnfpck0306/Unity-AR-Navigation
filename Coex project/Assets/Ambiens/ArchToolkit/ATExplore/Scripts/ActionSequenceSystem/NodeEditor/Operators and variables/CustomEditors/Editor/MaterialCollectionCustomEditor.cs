using ambiens.archtoolkit.atexplore.XNode;
using ArchToolkit;
using ArchToolkit.AnimationSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using ambiens.archtoolkit.atexplore.XNodeEditor;
namespace ambiens.archtoolkit.atexplore.actionSystem
{
    /*
    [CustomNodeEditor(typeof(MaterialsCollection))]
    public class MaterialsCollectionCustomEditor : AAssetCollectionCustomEditor
    {

        protected override void DrawAssetPicker(AAssetCollection.AssetReference variant)
        {
            variant.Asset = EditorGUILayout.ObjectField(variant.Asset, typeof(Material), false);
        }

        protected override List<AAssetCollection.AssetReference> GetAssetList()
        {
            var t=this.target as MaterialsCollection;
            return t.assetList;
        }

        protected override string GetConnectionName()
        {
            return "MATERIAL";
        }

        protected override AAssetCollection GetTarget()
        {
            return this.target as MaterialsCollection;
        }

        protected override void SetAssetList(List<AAssetCollection.AssetReference> list)
        {
            var t = this.target as MaterialsCollection;
            t.assetList=list;
        }

    }
    */
}
