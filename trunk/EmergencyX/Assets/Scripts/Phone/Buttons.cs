using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Buttons : MonoBehaviour 
{
	public string State = "Idle";

	public GameObject Timer;

	public bool HasBeenHolded;
	public bool Timed;
	public bool Called = false; 

	public float TimerTimeBase;
	public float TimerTime;
	
	Animator Anim;

	// Use this for initialization
	void Start () 
	{
		Anim = GetComponent<Animator> ();
		Timer = this.gameObject.transform.GetChild(1).gameObject;
	}

	void FixedUpdate ()
	{
		Timer.GetComponent<Image> ().fillAmount = TimerTime / TimerTimeBase;

		switch (State)
		{
		case"Idle":
			TimerTime = TimerTimeBase;
			Anim.SetBool("IsCalled",false);
			Anim.SetBool("HangUp",true);
			Timer.SetActive(false);
			break;
		case "Answered":
			Timer.SetActive(false);
			Anim.SetBool("HasAnswered",true);
			Anim.SetBool("IsCalled",false);
			Anim.SetBool("OnHold", false);
			if (ControlCenter.Instance.PhoneAnswered == null)
			{
				ControlCenter.Instance.PhoneAnswered = this.gameObject;
			}
			else if (ControlCenter.Instance.PhoneAnswered != this.gameObject)
			{
				ControlCenter.Instance.PhoneMiddle = this.gameObject;
				ControlCenter.Instance.HungUp();
			}
			ControlCenter.Instance.PhonesIdle.Remove(this.gameObject);
			break;
		case "Call":
			Anim.SetBool("HangUp",false);
			Anim.SetBool("IsCalled",true);
			Timer.SetActive(true);
			TimerTime -= Time.deltaTime;
			if (TimerTime <= 0)
			{
				LineHungUp();
			}
			break;
		case "Hold":
			Timer.SetActive(true);
			Anim.SetBool("HasAnswered",false);
			Anim.SetBool("OnHold",true);
			ControlCenter.Instance.PhoneAnswered = null;
			if(HasBeenHolded)
			{
				TimerTime -= Time.deltaTime;
				if (TimerTime <= 0)
				{
					LineHungUp();
				}
			}
			else
			{
				HasBeenHolded = true;
				TimerTime = TimerTimeBase;
			}
			break;
		}
	}

	public void ButtonClick()
	{
		switch (State)
		{
		case "Call":
			State = "Answered";
			break;
		case "Answered":
			State = "Hold";
			break;
		case "Hold":
			State = "Answered";
			break;
		}
	}

	public void LineHungUp()
	{
		State = "Idle";
		ControlCenter.Instance.PhoneAnswered = null;
		ControlCenter.Instance.PhonesIdle.Add(this.gameObject);
		ControlCenter.Instance.PhonesCall.Remove(this.gameObject);
	}
}
