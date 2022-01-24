using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelCell : MonoBehaviour {

  public Platform  platform;

  Vector2Int _gridPos;
  bool       _selected;

  public Vector2Int gridPos {
    get {
      return _gridPos;
    }
    set {
      _gridPos = value;
      transform.position = Level.CenterOfGridPos(_gridPos);
    }
  }

  public Vector3 position {
    get {
      return transform.position;
    }
  }

  public bool isEmpty {
    get {
      if (platform != null) return false;
      return true;
    }
  }

  public void SetSelected(bool selected) {
    _selected = selected;
    if (_selected && gameObject.GetComponentInChildren<Outline>() == null) {
      var outline = gameObject.AddComponent<Outline>();
      outline.OutlineMode = Outline.Mode.OutlineAll;
      outline.OutlineColor = Color.yellow;
      outline.OutlineWidth = 5f;
    } else if (!_selected) {
      Destroy(GetComponent<Outline>());
    }
  }

  public void SetPlatform(Platform plat) {
    if (platform != null) {
      Destroy(platform.gameObject);
    }

    if (PrefabUtility.GetPrefabAssetType(plat) == PrefabAssetType.NotAPrefab) {
      platform = plat;
    } else {
      platform = Instantiate(plat);
    }

    platform.transform.parent = transform;
    platform.transform.localPosition = Vector3.zero;
    platform.transform.localRotation = Quaternion.identity;
    platform.transform.localScale = Vector3.one;
  }

  public void RemovePlatform() {
    if (platform != null) {
      Destroy(platform.gameObject);
      platform = null;
    }

    if (isEmpty) {
      Actions.OnCellIsEmpty(this);
    }
  }

}
