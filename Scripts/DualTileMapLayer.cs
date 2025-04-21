using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Tool]
public partial class DualTileMapLayer: TileMapLayer
{
    internal event Action? DualTileMapLayerChanged;

    private Vector2I _tilePlaceholderAtlasCoord;
    private int _sourceId = 0;

    [Export]
    public Vector2I TilePlaceholderAtlasCoord
    {
        get => _tilePlaceholderAtlasCoord; 
        private set
        {
            if (_tilePlaceholderAtlasCoord == value)
            {
                return;
            }

            _tilePlaceholderAtlasCoord = value;

            DualTileMapLayerChanged?.Invoke();
        }
    }

    [Export]
    public int SourceId
    {
        get => _sourceId;
        private set
        {
            if (_sourceId == value)
            {
                return;
            }

            _sourceId = value;

            DualTileMapLayerChanged?.Invoke();
        }
    }

}
