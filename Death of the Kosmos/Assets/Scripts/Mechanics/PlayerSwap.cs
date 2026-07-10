using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwap : MonoBehaviour
{
    public GameObject[] characters;
	int currentActive = 0;
	
	void Awake()
	{
		characters[1].SetActive(false);
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Q))
		{
			if(currentActive == characters.Length - 1)
			{
				currentActive = 0;
			}
			else
			{
				currentActive++;
			}
			
			for(int i = 0; i < characters.Length; i++)
			{
				if(i == currentActive)
				{
					characters[i].SetActive(true);
					
					if(currentActive == 0)
					{
						characters[i].transform.position = characters[characters.Length - 1].transform.position;
					}
					else
					{
						characters[i].transform.position = characters[i - 1].transform.position;
					}
				}
				else
				{
					characters[i].SetActive(false);
				}
			}
		}
	}
}
