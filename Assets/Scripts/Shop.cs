﻿using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {

	public BikeCamera cam;
	public GameObject[] bikes;
	public UILabel bikeInfo;
	public UILabel cashView;
	public UILabel choisePopupLabel;

	public GameObject loadingScreen;
	public GameObject buyPopup;
	public GameObject nomoneyPopup;
	public GameObject choisePopup;
	public GameObject closePopup;
	public GameObject precloseScreen;

	public GameObject unlockBtn;
	public GameObject playBtn;

	public UISlider topSpeedSlider;
	public UISlider AccelerationSlider;
	public UISlider leanSlider;
	public UISlider gripSlider;

	private int curBike;
	private GameData data;

	private string[] bikeNames = {"Offroad Bike","Super Bike","Bike 03","Bike 04","Bike 05","Bike 06"};
	public static int[] prices = {0,20000,40000,65000,80000,100000};

	private bool isAction = false;

	void Start()
	{

		data = GameData.Get ();
		curBike = data.currentBike;
		//cam.target = bikes [curBike].transform;
		cashView.text = "Points: " + data.cash.ToString ();
		bikeInfo.text = bikeNames [curBike];
		chooseBike (curBike);
		showStatistic (curBike);
		showInfo ();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if(!precloseScreen.activeSelf)
			{
				PreClosePopup.showPopup = true;
				precloseScreen.SetActive(true);
			}
		}
	}

	private void chooseBike(int currentBike){
		for (int i=0; i<bikes.Length; i++) {
			if(i == currentBike)
				bikes [i].gameObject.SetActive(true);
			else
				bikes [i].gameObject.SetActive(false);
		}
	}
	void showStatistic(int currentBike){
		BikeStatics bikeStat = GameSettings.getCurrentBikeStatistics (currentBike);
		topSpeedSlider.value = bikeStat.topSpeed;
		AccelerationSlider.value = bikeStat.acceleration;
		leanSlider.value = bikeStat.lean;
		gripSlider.value = bikeStat.grip;
	}

	public void Left()
	{
		if(isAction) return;

		if (curBike - 1 >= 0)
			curBike -= 1;
		else
			curBike = bikes.Length - 1;
		//cam.target = bikes [curBike].transform;
		chooseBike (curBike);
		showStatistic (curBike);
		showInfo ();
	}


	public void Right()
	{
		if(isAction) return;

		if (curBike + 1 < bikes.Length)
				curBike += 1;
		else
				curBike = 0;
		//cam.target = bikes [curBike].transform;
		chooseBike (curBike);
		showStatistic (curBike);
		showInfo ();
	}

	void showInfo ()
	{
		if(data.bikeIsUnlock(curBike))
		{
			unlockBtn.SetActive(false);
			playBtn.SetActive(true);
			bikeInfo.text = bikeNames[curBike];
		}
		else
		{
			playBtn.SetActive(false);
			//TODO: eto blya 4tobi ne otkrivat' baiki
			//unlockBtn.SetActive(true);
			bikeInfo.text ="Price: " + prices[curBike].ToString() + " points";
		}
	}

	public void showBuyPopup()
	{
		isAction = true;
		if(data.cash >= prices[curBike])
		{
			buyPopup.SetActive(true);
		}
		else
			nomoneyPopup.SetActive(true);
	}

	public void areaClick()
	{
		if(isAction) return;
		if (data.bikeIsUnlock (curBike))
		{
			isAction = true;
			choisePopupLabel.text = "Use "+bikeNames[curBike]+" to play?";
			choisePopup.SetActive (true);
		}
//		else if(data.cash >= prices[curBike])
//			buyPopup.SetActive(true);
//		else
//			nomoneyPopup.SetActive(true);
	}
	public void buyBike()
	{
		data.cash -= prices [curBike];
		data.allowBikes.Add (curBike);
		data.currentBike = curBike;
		data.save ();
		bikeInfo.text = bikeNames[curBike];
		cashView.text = "Points: " + data.cash.ToString ();
		closeBuyPopup ();
		unlockBtn.SetActive (false);
	}

	public void choise()
	{
		data.currentBike = curBike;
		data.save ();
		loadingScreen.SetActive (true);
		GoTo.LoadMegaCity();
	}

	public void closeBuyPopup()
	{
		isAction = false;
		buyPopup.SetActive (false);
	}
	public void closeNoMOneyPopup()
	{
		isAction = false;
		nomoneyPopup.SetActive (false);
	}
	public void closeChoisePopup()
	{
		isAction = false;
		choisePopup.SetActive (false);
	}
	public void closeClosePopup()
	{
		isAction = false;
		closePopup.SetActive (false);
	}

	public void menuClick()
	{
		if(isAction) return;
		isAction = true;
		closePopup.SetActive (true);
	}
	public void goMainMenu()
	{
		loadingScreen.SetActive (true);
		GoTo.LoadMenu ();
	}

}
