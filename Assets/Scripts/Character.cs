using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField] private float _movementCooldown;

	public Action OnHpChanged;

	protected Vector2 PositionOnBoard;
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

	public bool CanMove { get; private set; }

	private void Awake()
	{
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

	private IEnumerator LockMovement()
	{
		CanMove = false;
		yield return new WaitForSeconds(_movementCooldown);
		CanMove = true;
	}

	protected virtual void InitializePosition()
	{
	}

	protected virtual void SetPosition()
	{
		StartCoroutine(LockMovement());
	}

	public virtual void ShootRock()
	{
	}

	public virtual void ShootScissor()
	{
	}

	public virtual void ShootPaper()
	{
	}
}