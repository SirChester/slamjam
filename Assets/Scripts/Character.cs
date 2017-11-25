using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField] private float _movementCooldown;
	[SerializeField] private Weapon[] _weapons;

	public Action OnHpChanged;

	public Vector2 PositionOnBoard;
	private int HorLimit = 2;
	private int VertLimit = 4;
	private Dictionary<Weapon.Type, float> _lastShots = new Dictionary<Weapon.Type, float>();

	private int _hp;

	public int Hp
	{
		get { return _hp; }
		set
		{
			_hp = value;
			if (OnHpChanged != null)
			{
				OnHpChanged();
			}
		}
	}

	public bool CanMove { get; private set; }

	private void Awake()
	{
		for (int i = 0; i < _weapons.Length; ++i)
		{
			_lastShots.Add(_weapons[i].BulletType, 0.0f);
		}
		InitializePosition();
		Hp = 100;
		SetPosition();
	}

	public void Up()
	{
		if (PositionOnBoard.y > 0)
		{
			PositionOnBoard.y--;
		}
		SetPosition();
	}

	public void Down()
	{
		if (PositionOnBoard.y < VertLimit)
		{
			PositionOnBoard.y++;
		}
		SetPosition();
	}

	public void Left()
	{
		if (PositionOnBoard.x > 0)
		{
			PositionOnBoard.x--;
		}
		SetPosition();
	}

	public void Right()
	{
		if (PositionOnBoard.x < HorLimit)
		{
			PositionOnBoard.x++;
		}
		SetPosition();
	}
	
	public void ShootByIndex(int idx)
	{
		var obj = Weapon.ShootByType(_weapons[idx].BulletType);
		FixBulletPosition(obj);
		var weapon = obj.GetComponent<Weapon>();
		_lastShots[weapon.BulletType] = Time.realtimeSinceStartup;
	}

	private IEnumerator LockMovement()
	{
		CanMove = false;
		yield return new WaitForSeconds(_movementCooldown);
		CanMove = true;
	}

	public bool CanShootByIndex(int idx)
	{
		return Time.realtimeSinceStartup - _lastShots[_weapons[idx].BulletType] > _weapons[idx].Cooldown;
	}

	protected virtual void SetPosition()
	{
		StartCoroutine(LockMovement());
	}
	
	protected virtual void InitializePosition()
	{
	}
	
	protected virtual void FixBulletPosition(GameObject bullet)
	{
	}
}