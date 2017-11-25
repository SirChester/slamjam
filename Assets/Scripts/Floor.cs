using System;
using UnityEditor;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private double _cellHealth = 100;
    [SerializeField] private double _playerTickDamage = 0.1;
    
    private FloorCell[,] _cells = new FloorCell[3, 5];
    private static Sprite[] _sprites;

    private void Awake()
    {
        var transformComponent = transform;
        for (int i = 0; i < transformComponent.childCount; ++i)
        {
            var floorCell = transformComponent.GetChild(i);
            if (floorCell.name.ToLower().StartsWith("slate"))
            {
                continue;
            }
            var x = GetX(floorCell.transform);
            var y = GetY(floorCell.transform);

            var spriteRenderer = floorCell.GetComponent<SpriteRenderer>();
            var cell = new FloorCell(spriteRenderer, _cellHealth);
            _cells[x, y] = cell;
        }

        _sprites = Resources.LoadAll<Sprite>("Sprites/grassland_spritesheet");
    }

    public void damageByPlayer(Vector2 position)
    {
        _cells[(int) position.x, (int) position.y].Damage(_playerTickDamage);
    }

    private int GetY(Transform floorCellTransform)
    {
        if (floorCellTransform.position.y < -0.5)
        {
            return 0;
        }
        if (floorCellTransform.position.y < 0.5)
        {
            return 1;
        }
        if (floorCellTransform.position.y < 1)
        {
            return 2;
        }
        if (floorCellTransform.position.y < 2)
        {
            return 3;
        }
        return 4;
    }

    private int GetX(Transform floorCellTransform)
    {
        if (floorCellTransform.position.x < 3.2)
        {
            return 0;
        }
        if (floorCellTransform.position.x < 4)
        {
            return 1;
        }
        return 2;
    }


    private class FloorCell
    {
        private int _cellDamageFramesCount = 6;
        
        public SpriteRenderer SpriteRenderer { get; private set; }
        public double Health { get; private set; }
        public double MaxHealth { get; private set; }

        public FloorCell(SpriteRenderer spriteRenderer, double cellHealth)
        {
            SpriteRenderer = spriteRenderer;
            Health = cellHealth;
            MaxHealth = cellHealth;
        }

        public void Damage(double damage)
        {
            Health -= damage;
            int frameNum = (int) ((MaxHealth - Health) * _cellDamageFramesCount / MaxHealth + 1);
            frameNum = Math.Min(_cellDamageFramesCount, frameNum);
            var currentSpriteName = SpriteRenderer.sprite.name;
            var newSpriteName = currentSpriteName.Substring(0, currentSpriteName.Length - 1) + frameNum;
            var newSprite = Array.FindAll(_sprites, obj => obj.name == newSpriteName)[0];

            SpriteRenderer.sprite = newSprite;
        }
    }
}