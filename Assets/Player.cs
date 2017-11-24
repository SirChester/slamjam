using UnityEngine;

public class Player : MonoBehaviour
{
	private Vector2 _positionOnBoard;
	private int HorLimit = 2;
	private int VertLimit = 4;

	private void Awake()
	{
		_positionOnBoard = Vector2.zero;
		SetPosition();
	}

	private void SetPosition()
	{
		var pos = new Vector2(_positionOnBoard.x * 64.0f, _positionOnBoard.y * -64.0f);
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
}