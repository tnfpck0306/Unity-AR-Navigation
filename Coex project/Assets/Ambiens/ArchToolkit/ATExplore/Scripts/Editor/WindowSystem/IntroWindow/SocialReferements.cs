using UnityEngine;
using System;

namespace ArchToolkit.Editor.Window
{
    [Serializable]
    public class SocialReferements 
    {
        public string title;
        public string textureUrl;
        public string siteUrl;
        public string description;
        public Texture textureImage;

        public void SetTextureImage()
        {
            this.textureImage = Resources.Load<Texture>(this.textureUrl);
        }

        public SocialReferements(string textureUrl, string siteUrl, string description, string title)
        {
            this.textureUrl = textureUrl;
            this.description = description;
            this.siteUrl = siteUrl;
            this.title = title;
        }
    }
}