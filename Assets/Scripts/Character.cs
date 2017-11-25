using System;
using UnityEngine;

public class Character : MonoBehaviour
{
	public Action OnHpChanged;

	protected Vector2 _positionOnBoard;
	private int HorLimit = 2;
	private int VertLimit = 4;

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

	private void Awake()
	{
		InitializePosition();
		Hp = 100;
		SetPosition();
	}

	public void Up()
	{
		if (_positionOnBoard.y > 0)
		{
			_positionOnBoard.y--;
		}
		SetPosition();
	}

	public void Down()
	{
		if (_positionOnBoard.y < VertLimit)
		{
			_positionOnBoard.y++;
		}
		SetPosition();
	}

	public void Left()
	{
		if (_positionOnBoard.x > 0)
		{
			_positionOnBoard.x--;
		}
		SetPosition();
	}

	public void Right()
	{
		if (_positionOnBoard.x < HorLimit)
		{
			_positionOnBoard.x++;
		}
		SetPosition();
	}

	protected virtual void InitializePosition() {}
	protected virtual void SetPosition() {}
	public virtual void ShootRock() {}
	public virtual void ShootScissor() {}
	public virtual void ShootPaper() {}
}