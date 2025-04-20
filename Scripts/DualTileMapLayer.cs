using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Tool]
public partial class DualTileMapLayer: TileMapLayer
{

    [Export]
    public Vector2I TilePlaceholderAtlasCoord
    {
        get; private set;
    }

}
