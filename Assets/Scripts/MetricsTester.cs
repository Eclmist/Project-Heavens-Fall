using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetricsTester : MonoBehaviour
{
	public static MetricsTester Instance;
	private Player player;
	public float maxHeight;

	// Use this for initialization
	void Awake ()
	{
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (!player)
		{
			player = FindObjectOfType<Player>();
		}

		if (!player)
		{
			return;
		}

		maxHeight = Mathf.Max(player.transform.position.y, maxHeight);

		DebugHelper.WriteDebug(gameObject,"Max Height = " + maxHeight, Color.red, 0);
	}
}
