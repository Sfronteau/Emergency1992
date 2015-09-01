using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlCenter : MonoBehaviour 
{
	public List<GameObject> PhonesIdle = new List<GameObject>();
	public List<GameObject> PhonesCall = new List<GameObject>();
	public GameObject PhoneAnswered;
	public GameObject PhoneMiddle;

	public float TimerTime;
	public float TimerCall = 1;

	public int TimerMin = 3;
	public int TimerMax = 25;

	GameObject PhoneSelect;

	Buttons ButtonsScript;

	public static ControlCenter Instance;
	
	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (PhonesIdle.Capacity > 0)
		{
			TimerTime += Time.deltaTime;
		}

		if (TimerTime >= TimerCall) 
		{
			PhoneSelect = PhonesIdle[Random.Range(0, PhonesIdle.Capacity)];
			TimerCall = Random.Range(TimerMin,TimerMax);
			ButtonsScript = PhoneSelect.GetComponent<Buttons>();
			PhonesIdle.Remove(PhoneSelect);
			PhonesCall.Add(PhoneSelect);
			ButtonsScript.State = "Call";
			TimerTime = 0;
		}	
	}

	public void HungUp()
	{
		ButtonsScript = PhoneAnswered.GetComponent<Buttons>();
		ButtonsScript.LineHungUp ();
		PhoneAnswered = PhoneMiddle;
	}
}
