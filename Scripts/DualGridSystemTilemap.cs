using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


[Tool]
public partial class DualGridSystemTilemap : TileMapLayer
{
    static readonly Vector2I[] NEIGHBOURS = [new(0, 0), new(1, 0), new(0, 1), new(1, 1)];

    [Export]
    public DualTileMapLayer[] DualTileMapLayers
    {
        get; private set;
    } = [];

    [Export]
    public bool IsBaked
    {
        get; private set;
    } = false;

    private static readonly Dictionary<Vector4I, Vector2I> s_neighboursToAtlasCoord = new() {
        {new (1, 1, 1, 1), new(2, 1)}, // All corners
        {new (0, 0, 0, 1), new(1, 3)}, // Outer bottom-right corner
        {new (0, 0, 1, 0), new(0, 0)}, // Outer bottom-left corner
        {new (0, 1, 0, 0), new(0, 2)}, // Outer top-right corner
        {new (1, 0, 0, 0), new(3, 3)}, // Outer top-left corner
        {new (0, 1, 0, 1), new(1, 0)}, // Right edge
        {new (1, 0, 1, 0), new(3, 2)}, // Left edge
        {new (0, 0, 1, 1), new(3, 0)}, // Bottom edge
        {new (1, 1, 0, 0), new(1, 2)}, // Top edge
        {new (0, 1, 1, 1), new(1, 1)}, // Inner bottom-right corner
        {new (1, 0, 1, 1), new(2, 0)}, // Inner bottom-left corner
        {new (1, 1, 0, 1), new(2, 2)}, // Inner top-right corner
        {new (1, 1, 1, 0), new(3, 1)}, // Inner top-left corner
        {new (0, 1, 1, 0), new(2, 3)}, // Bottom-left top-right corners
        {new (1, 0, 0, 1), new(0, 1)}, // Top-left down-right corners
		{new (0, 0, 0, 0), new(0, 3)}, // No corners
    };

    public override void _Ready()
    {
        this.Modulate = new Color(1, 1, 1, .0f);

        if (IsBaked)
        {
            return;
        }

        Bake();
    }

    public void Bake()
    {
        IsBaked = true;
        DualTileMapLayers ??= [];

        foreach (var layer in DualTileMapLayers)
        {
            foreach (var coord in GetUsedCells())
            {
                SetDisplayTile(layer, coord);
            }
        }

        Modulate = new Color(1, 1, 1, .0f);
        
    }

    public void SetTile(TileMapLayer tileMapLayer, Vector2I coords)
    {
        SetDisplayTile(tileMapLayer, coords);
    }

    void SetDisplayTile(TileMapLayer tileMapLayer, Vector2I pos)
    {
        // loop through 4 display neighbours
        for (var i = 0; i < NEIGHBOURS.Length; i++)
        {
            var newPos = pos + NEIGHBOURS[i];
            tileMapLayer.SetCell(newPos, 0, atlasCoords: CalculateDisplayTile(tileMapLayer, newPos));
        }
    }

    private Vector2I CalculateDisplayTile(TileMapLayer tileMapLayer, Vector2I coords)
    {
        // get 4 world tile neighbours
        var botRight = GetWorldTileMapLayerRelative(tileMapLayer, coords - NEIGHBOURS[0]);
        var botLeft = GetWorldTileMapLayerRelative(tileMapLayer, coords - NEIGHBOURS[1]);
        var topRight = GetWorldTileMapLayerRelative(tileMapLayer, coords - NEIGHBOURS[2]);
        var topLeft = GetWorldTileMapLayerRelative(tileMapLayer, coords - NEIGHBOURS[3]);

        // return tile (atlas coord) that fits the neighbour rules
        return s_neighboursToAtlasCoord[new(topLeft, topRight, botLeft, botRight)];
    }

    private int GetWorldTileMapLayerRelative(TileMapLayer relative, Vector2I cords)
    {
        var tileMapLayer = GetWorldTileMapLayer(cords);

        if (tileMapLayer == relative)
        {
            return 1;
        }

        return 0;
    }

    private DualTileMapLayer? GetWorldTileMapLayer(Vector2I coords)
    {
        var atlasCoord = GetCellAtlasCoords(coords);
        var minus = new Vector2I(-1, -1);

        if(atlasCoord == minus)
        {
            return null;
        }

        foreach (var layer in DualTileMapLayers)
        {
            if (layer.TilePlaceholderAtlasCoord == atlasCoord)
            {
                return layer;
            }
        }

        return null;
    }
}