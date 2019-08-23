using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarsClicker : MonoBehaviour
{
	public double dust = 0;
	public float dustPerSecund = 0;
	public int crystals = 0;
	public int friends = 0;
	public int niceStuffLevel = 0;

	public SpriteRenderer[] niceStuffSprites;
	[SerializeField]
	public string[] niceStuffDescriptions;
	public SpriteRenderer mars;

	public GameObject dustCounterObject;
	public Text dustCounterText;

	public GameObject crystalCounterObject;
	public Text crystalCounterText;

	public GameObject friendsCounterObject;
	public Text friendsCounterText;

	public Button dustMoveButton;
	public Text dustMoveText;

	public Button crystalBuyButton;
	public Text crystalBuyText;

	public Button friendsBuyButton;
	public Text friendsBuyText;

	public Button niceStuffBuyButton;
	public Text niceStuffBuyText;

	public Button terraformMarsButton;
	public Text terraformMarsText;

	public Button leaveMarsButton;
	public Text leaveMarsText;

	public Color startTint;
	public Color endtint;

	//default is dust moved to buy first
	public int crystalPrice = 30;
	public int friendPrice = 250; 
	public int nicestuffPrice = 1000;
	public int terraformPrice = 2000000000;

	//how much of the screen is revealed.
	public short gameState = 0;
	private void Start()
	{
		dustCounterObject.gameObject.SetActive(false);
		crystalCounterObject.gameObject.SetActive(false);
		crystalBuyButton.gameObject.SetActive(false);
		friendsCounterObject.gameObject.SetActive(false);
		friendsBuyButton.gameObject.SetActive(false);
		niceStuffBuyButton.gameObject.SetActive(false);
		terraformMarsButton.gameObject.SetActive(false);
		leaveMarsButton.gameObject.SetActive(false);

		foreach(SpriteRenderer s in niceStuffSprites)
		{
			s.gameObject.SetActive(false);
		}
	}
	// Update is called once per frame
	void Update ()
	{
		dust += dustPerSecund*Time.deltaTime;
		float p = (float) dust/terraformPrice;
		Color c = Color.Lerp(startTint, endtint, p);

		long d = (long)dust;

		if(gameState == 0)
		{
			if(d >= 2)
			{
				gameState++;
				dustCounterObject.SetActive(true);
			}
		}
		else if(gameState == 1)
		{
			if(d >= (1/3)*crystalPrice)
			{
				gameState++;
				crystalCounterObject.SetActive(true);
				crystalBuyButton.gameObject.SetActive(true);

			}
		}
		else if(gameState == 2)
		{
			if(d >= (1/5)*friendPrice && crystals > 0)
			{
				gameState++;
				friendsCounterObject.SetActive(true);
				friendsBuyButton.gameObject.SetActive(true);
			}
		}
		else if(gameState == 3)
		{
			if(d >= (1/10)*nicestuffPrice && friends > 0)
			{
				gameState++;
				niceStuffBuyButton.gameObject.SetActive(true);
			}
		}
		else if(gameState == 4)
		{
			if(niceStuffLevel >= niceStuffDescriptions.Length-1)
			{
				gameState++; //end-game
				terraformMarsButton.gameObject.SetActive(true);
				leaveMarsButton.gameObject.SetActive(true);
				leaveMarsButton.interactable = true;
			}
		}

		crystalBuyButton.interactable =    d >= crystalPrice;
		friendsBuyButton.interactable =	   d >= friendPrice;
		niceStuffBuyButton.interactable =  d >= nicestuffPrice;
		terraformMarsButton.interactable = d >= terraformPrice && niceStuffLevel >= niceStuffDescriptions.Length - 1;

		dustCounterText.text = LongNum.FormatNumber(d);
		crystalCounterText.text = crystals.ToString();
		friendsCounterText.text = friends.ToString();
		
		//dustMoveText.text = "";
		if(d < crystalPrice)
			crystalBuyText.text = string.Format("Buy a crystal, {0} more clicks.", crystalPrice - d);
		else
			crystalBuyText.text = "Buy a crystal!";

		if(d < friendPrice)
			friendsBuyText.text = string.Format("Buy a friend, {0} more clicks.", friendPrice - d);
		else
			friendsBuyText.text = "Buy a friend!";

		if(d < terraformPrice)
			terraformMarsText.text = string.Format("Mars will be terraformed in {0} clicks!", LongNum.FormatNumber((long)terraformPrice - d));
		else
			terraformMarsText.text = "Terraform Mars!";
		//leaveMarsText.text = "";

		if(niceStuffLevel < niceStuffDescriptions.Length - 1)
			niceStuffBuyText.text = string.Format("Buy {0}", niceStuffDescriptions[niceStuffLevel]) + (niceStuffBuyButton.interactable ? "!" : string.Format(", {0} more clicks!", nicestuffPrice - d));
	}

	public void MoveDust()
	{
		dust++;
	}

	public void BuyCrystal()
	{
		crystals++;
		dust -= crystalPrice;
		crystalPrice = Mathf.RoundToInt(crystalPrice * 10f);
	}
	public void BuyFriend()
	{
		friends++;
		dust -= friendPrice;
		friendPrice = Mathf.RoundToInt(friendPrice * 8f);
	}
	public void BuyNiceStuff()
	{
		niceStuffSprites[niceStuffLevel].gameObject.SetActive(true);
		niceStuffLevel++;
		dust -= nicestuffPrice;
		nicestuffPrice = Mathf.RoundToInt(nicestuffPrice * 5);
	}
	public void TerraformMars()
	{
		//tank you for playing.
		Application.Quit();
	}
	public void LeaveMars()
	{
		//taking off...
		Application.Quit();
	}
}
