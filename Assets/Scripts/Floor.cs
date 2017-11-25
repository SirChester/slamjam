using System;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private double _cellHealth = 100;
    [SerializeField] private double _playerTickDamage = 0.3;

    [SerializeField] private FloorCellInfo[] _floorCellInfo;

    private FloorCell[,] _cells = new FloorCell[3, 5];
    private static Sprite[] _sprites;

    private void Awake()
    {
        foreach (var floorCell in _floorCellInfo)
        {
            var spriteRenderer = floorCell._gameObject.GetComponent<SpriteRenderer>();
            var cell = new FloorCell(spriteRenderer, _cellHealth);
            var x = (int) floorCell._coordinates.y;
            var y = (int) floorCell._coordinates.x;
            _cells[x, y] = cell;
        }

        _sprites = Resources.LoadAll<Sprite>("Sprites/grassland_spritesheet");
    }

    public bool damageByPlayer(Vector2 position)
    {
        return _cells[(int) position.x, (int) position.y].Damage(_playerTickDamage);
    }

    [Serializable]
    public struct FloorCellInfo
    {
        public Vector2 _coordinates;
        public GameObject _gameObject;
    }

    private class FloorCell
    {
        private int _cellDamageFramesCount = 6;

        public SpriteRenderer SpriteRenderer { get; private set; }
        public double Health { get; set; }
        public double MaxHealth { get; private set; }

        public FloorCell(SpriteRenderer spriteRenderer, double cellHealth)
        {
            SpriteRenderer = spriteRenderer;
            Health = cellHealth;
            MaxHealth = cellHealth;
        }

        //return true if has return damage from cell to player
        public bool Damage(double damageToCell)
        {
            Health -= damageToCell;
            Health = Math.Max(0, Health);
            int frameNum = (int) ((MaxHealth - Health) * _cellDamageFramesCount / MaxHealth + 1);
            frameNum = Math.Min(_cellDamageFramesCount, frameNum);
            var currentSpriteName = SpriteRenderer.sprite.name;
            var newSpriteName = currentSpriteName.Substring(0, currentSpriteName.Length - 1) + frameNum;
            var newSprite = Array.FindAll(_sprites, obj => obj.name == newSpriteName)[0];

            //TODO delete it later
            var k = 1 - (frameNum - 1.0f) / _cellDamageFramesCount;
            SpriteRenderer.color = new Color(k, k, k);

            SpriteRenderer.sprite = newSprite;
            return Health <= 0;
        }
    }

    public void Reset()
    {
        foreach (var cell in _cells)
        {
            cell.Health = cell.MaxHealth;
            cell.SpriteRenderer.color = new Color(1, 1, 1);
        }
    }
}