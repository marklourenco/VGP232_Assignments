using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WPF_Sprite_Sheet_Creator
{
    [Serializable]
    public class SpriteSheetProject
    {
        public string OutputDirectory { get; set; }
        public string OutputFile { get; set; }
        public List<string> ImagePaths { get; set; }
        public bool IncludeMetaData { get; set; }

        public SpriteSheetProject()
        {
            ImagePaths = new List<string>();
        }
    }
}