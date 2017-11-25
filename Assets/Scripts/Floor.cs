using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private int _cellHealth;
    [SerializeField] private int _playerTickDamage;
    
    private FloorCell[,] _cells = new FloorCell[3, 5];

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
        public SpriteRenderer SpriteRenderer { get; private set; }
        public int Health { get; private set; }

        public FloorCell(SpriteRenderer spriteRenderer, int cellHealth)
        {
            SpriteRenderer = spriteRenderer;
            Health = cellHealth;
        }

        public void Damage(int damage)
        {
            Health -= damage;
            SpriteRenderer.sprite
        }
    }
}