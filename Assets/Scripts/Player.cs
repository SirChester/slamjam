using UnityEngine;

public class Player : Character
{
	protected override void InitializePosition()
	{
		PositionOnBoard = Vector2.zero;
	}

	protected override void SetPosition()
	{
		var pos = new Vector2(PositionOnBoard.x * 50.0f, PositionOnBoard.y * -50.0f);
		GetComponent<RectTransform>().anchoredPosition = pos;
		base.SetPosition();
	}

	protected override void FixBulletPosition(GameObject bullet)
	{
		bullet.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + 75.0f,
			gameObject.transform.localPosition.y - 12.5f);
	}
}