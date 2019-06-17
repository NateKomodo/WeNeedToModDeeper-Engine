using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ModEngine
{
    public class ModEngineCustomSubmarine
    {
    }

    public class BuildManifest
    {

    }

    public class BuildItem
    {
        public CreationMode mode;

        public string itemName;

        public Vector2 pos;
    }

    public enum CreationMode
    {
        CREATE,
        MOVE
    }
}
