using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Copyright Sami S.

// use of any kind without a written permission 
// from the author is not allowed.

// DO NOT:
// Fork, clone, copy or use in any shape or form.


public enum Mode
{
    None,
    Edit,
    Play
}

public class CellPos
{
    public Vector3Int pos;
    public TileBase cell;
}


public class GameOfLife : MonoBehaviour
{

    public Camera cam;

    [Header("Cells")]
    public TileBase tileCell;
    public TileBase tileEmpty;

    [Header("Maps")]
    public Tilemap tilemapBg;
    public Tilemap tilemap;

    [Header("Settings")]
    public int fps = 10;
    public Vector2Int size = new Vector2Int(25, 25);

    [Header("UI")]
    public GameOfLifeUI UI;

    [Header("Debug")]
    [SerializeField] Mode mode;
    [SerializeField] BoundsInt bounds;
    [SerializeField] bool inBounds;

    // local
    List<CellPos> nextGenCells;
    float nextFrame;
    Vector2Int sizeWas;
    int frame = 0;


    void Start()
    {
        UI.UpdateModeText("MODE: Edit");
    }


    void Update()
    {
        bounds = GetBounds();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            mode = Mode.Edit;
            UI.UpdateModeText("MODE: Edit");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            frame = 0;
            mode = Mode.Play;
            UI.UpdateModeText("MODE: Play");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            SimulateStep();
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            ClearMap();
            mode = Mode.Edit;
            UI.UpdateModeText("MODE: Edit");
        }

        if (mode == Mode.Edit)
        {
            DrawCells();
        }

        if (mode == Mode.Play)
        {
            PlaySimulation();
        }

        if (size != sizeWas)
        {
            cam.orthographic = true;
            cam.orthographicSize = size.y / 2f + 2f;
            ClearMap();
        }

        sizeWas = size;
    }




    // Simulation mode

    void PlaySimulation()
    {
        if (Time.time < nextFrame)
            return;

        nextFrame = Time.time + 1f / fps;
        SimulateStep();
    }

    void SimulateStep()
    {
        nextGenCells = new List<CellPos>();

        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                var cell = UpdateCell(x, y);
                nextGenCells.Add(new CellPos() { cell = cell, pos = new Vector3Int(x, y, 0) });
            }
        }

        // updated cells
        foreach (var cell in nextGenCells)
        {
            tilemap.SetTile(cell.pos, cell.cell);
        }


        // Update frame
        frame++;
        UI.UpdateFrameText("FRAME: " + frame);
    }

    TileBase UpdateCell(int x, int y)
    {
        var pos = new Vector3Int(x, y, 0);
        var cell = tilemap.GetTile(pos);
        var around = GetNeighborsCount(pos);

        if (cell != null)
        {
            // Live cell, less than 2 live neighbours - die
            if (around < 2)
            {
                return null;
            }

            // Live cell, 2 or 3 live neighbours - lives
            else if (around == 2 || around == 3)
            {
                return tileCell;
            }

            // Live cell, more than 3 live neighbours - die
            else if (around > 3)
            {
                return null;
            }
        }
        else
        {
            // Dead cell, with 3 live neighbours - lives
            if (around == 3)
            {
                return tileCell;
            }
        }

        return null;
    }

    int GetNeighborsCount(Vector3Int pos)
    {
        int count = 0;

        var n = new Vector3Int(pos.x, pos.y + 1, 0);
        var ne = new Vector3Int(pos.x + 1, pos.y + 1, 0);
        var e = new Vector3Int(pos.x + 1, pos.y, 0);
        var se = new Vector3Int(pos.x + 1, pos.y - 1, 0);
        var s = new Vector3Int(pos.x, pos.y - 1, 0);
        var sw = new Vector3Int(pos.x - 1, pos.y - 1, 0);
        var w = new Vector3Int(pos.x - 1, pos.y, 0);
        var nw = new Vector3Int(pos.x - 1, pos.y + 1, 0);

        if (tilemap.GetTile(n) != null) count++;
        if (tilemap.GetTile(ne) != null) count++;
        if (tilemap.GetTile(e) != null) count++;
        if (tilemap.GetTile(se) != null) count++;
        if (tilemap.GetTile(s) != null) count++;
        if (tilemap.GetTile(sw) != null) count++;
        if (tilemap.GetTile(w) != null) count++;
        if (tilemap.GetTile(nw) != null) count++;

        return count;
    }

    void ClearMap()
    {
        Debug.Log("Clear map");
        tilemap.ClearAllTiles();

        Debug.Log("Clear bg");
        for (int y = 0; y < bounds.size.y; y++)
        {
            for (int x = 0; x < bounds.size.x; x++)
            {
                tilemapBg.SetTile(new Vector3Int(bounds.min.x + x, bounds.min.y + y, 0), tileEmpty);
            }
        }
    }




    // Draw mode

    void DrawCells()
    {
        var pos = Input.mousePosition;
        var wpos = Camera.main.ScreenToWorldPoint(pos);
        var tpos = tilemap.WorldToCell(wpos);

        if (!InBounds(tpos))
            return;

        if (Input.GetMouseButton(0))
        {
            tilemap.SetTile(tpos, tileCell);
        }

        if (Input.GetMouseButton(1))
        {
            tilemap.SetTile(tpos, null);
        }
    }





    // Helpers -----

    // Bounds

    bool InBounds(Vector3Int pos)
    {
        inBounds = bounds.Contains(pos);
        return inBounds;
    }

    BoundsInt GetBounds()
    {
        var px = Mathf.FloorToInt(size.x * 0.5f);
        var py = Mathf.FloorToInt(size.y * 0.5f);
        var b = new BoundsInt(new Vector3Int(-px, -py, 0), new Vector3Int(size.x, size.y, 1));
        return b;
    }



    // Debug

    void OnDrawGizmos()
    {
        var dx = Mathf.FloorToInt(size.x * 0.5f);
        var dy = Mathf.FloorToInt(size.y * 0.5f);
        GameOfLife.DrawRect(new Rect(-dx, -dy, size.x, size.y), Color.red);
    }

    public static void DrawRect(Rect rect, Color color, float duration = 0f)
    {
        var ll = new Vector2(rect.min.x, rect.min.y);
        var ul = new Vector2(rect.min.x, rect.max.y);
        var lr = new Vector2(rect.max.x, rect.min.y);
        var ur = new Vector2(rect.max.x, rect.max.y);

        // Draw
        Debug.DrawLine(ll, ul, color, duration);
        Debug.DrawLine(ul, ur, color, duration);
        Debug.DrawLine(ur, lr, color, duration);
        Debug.DrawLine(ll, lr, color, duration);
    }

}