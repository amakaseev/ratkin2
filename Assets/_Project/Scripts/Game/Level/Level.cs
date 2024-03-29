using System.Collections.Generic;
using UnityEngine;

public class Level: MonoBehaviour {

  private List<LevelCell> _cells = new List<LevelCell>();
  private Vector2Int      _size;
  private Bounds          _bounds;
  private bool            _needUpdateSize;

  public static LevelCell CreateCell() {
    GameObject cellObject = new GameObject("LevelCell");
    LevelCell cell = cellObject.AddComponent<LevelCell>();
    return cell;
  }

  public static Vector2Int GetGridPos(Vector3 pos) {
    return new Vector2Int(
      (int)(pos.x - ((pos.x < 0)? 1 : 0)),
      (int)(pos.z - ((pos.z < 0)? 1 : 0))
    );
  }

  public static Vector3 CenterOfGridPos(Vector2Int pos) {
    return new Vector3(
      pos.x + 0.5f,
      0,
      pos.y + 0.5f
    );
  }

  public List<LevelCell> GetCells() {
    return _cells;
  }

  public LevelCell GetCell(Vector2Int pos) {
    foreach (var cell in _cells) {
      if (cell.gridPos == pos) {
        return cell;
      }
    }

    return null;
  }

  public int width {
    get {
      if (_needUpdateSize) {
        UpdateSize();
      }
      return _size.x;
    }
  }

  public int height {
    get {
      if (_needUpdateSize) {
        UpdateSize();
      }
      return _size.y;
    }
  }

  public void Clear() {
    _cells.Clear();

    _needUpdateSize = true;
  }

  private void UpdateSize() {
    _bounds = new Bounds();
    if (_cells.Count > 0) {
      foreach(var cell in _cells) {
        _bounds.Encapsulate(cell.position);
      }
      _bounds.Expand(new Vector3(1, 1, 1));
    }
  }

  public void SetCell(Vector2Int pos, LevelCell cell) {
    var prevCell = GetCell(pos);
    if (prevCell) {
      RemoveCell(prevCell);
    }
    _cells.Add(cell);
    cell.gridPos = pos;
    cell.transform.parent = transform;

    _needUpdateSize =true;
  }

  public void RemoveCell(LevelCell cellToRemove) {
    foreach (var cell in _cells) {
      if (cell.gridPos == cellToRemove.gridPos) {
        _cells.Remove(cell);
        Destroy(cell.gameObject);
        return;
      }
    }

    _needUpdateSize = true;
  }

  void OnDrawGizmos() {
    UpdateSize();
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(_bounds.center, _bounds.size);
  }

}
