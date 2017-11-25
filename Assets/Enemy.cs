using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Action OnHpChanged;
	
	private Vector2 _positionOnBoard;
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
		_positionOnBoard = new Vector2(2, 0);
		Hp = 100;
		SetPosition();
	}
	

	private void SetPosition()
	{
		var pos = new Vector2(_positionOnBoard.x * 50.0f, _positionOnBoard.y * -50.0f);
		GetComponent<RectTransform>().anchoredPosition = pos;
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

	public void ShootRock()
	{
		var obj = Instantiate(Resources.Load("Prefabs/Rock"), gameObject.transform.parent) as GameObject;
		obj.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 55.0f, gameObject.transform.localPosition.y - 12.5f);
		obj.GetComponent<Bullet>().PushTo(false);
	}
	
	public void ShootScissor()
	{
		var obj = Instantiate(Resources.Load("Prefabs/Scissor"), gameObject.transform.parent) as GameObject;
		obj.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 55.0f, gameObject.transform.localPosition.y - 12.5f);
		obj.GetComponent<Bullet>().PushTo(false);
	}
	
	public void ShootPaper()
	{
		var obj = Instantiate(Resources.Load("Prefabs/Paper"), gameObject.transform.parent) as GameObject;
		obj.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 55.0f, gameObject.transform.localPosition.y - 12.5f);
		obj.GetComponent<Bullet>().PushTo(false);
	}
}
