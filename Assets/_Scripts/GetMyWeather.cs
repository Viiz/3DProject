using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GetMyWeather : MonoBehaviour {

	public UILabel myWeatherLabel;
	public UITexture myWeatherCondition;
	
	public string currentIP;
	public string currentCountry = "Netherlands";
	public string currentCity = "Eindhoven";

	//retrieved from weather API
	public string retrievedCountry;
	public string retrievedCity;
	public int conditionID;
	public string conditionName;
	public string conditionImage;
	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine (SendRequest ());
	}

	IEnumerator SendRequest()
	{/*
		Network.Connect ("http://google.com");
		currentIP = Network.player.externalIP;
		Debug.Log (Network.player.externalIP);
		Network.Disconnect ();

		
		WWW cityRequest = new WWW ("http://www.geoplugin.net/json.gp?ip=" + currentIP);
		yield return cityRequest;

		if (cityRequest.error == null || cityRequest.error == "")
		{
			var N = JSON.Parse(cityRequest.text);
			currentCity = N["geoplugin_city"].Value;
			currentCountry = N["geoplugin_countryName"].Value;
		}
		else
		{
			Debug.Log ("WWW Error: " + cityRequest.error);
		}*/

		WWW request = new WWW ("http://api.openweathermap.org/data/2.5/weather?q=" + currentCity);
		yield return request;

		if (request.error == null || request.error == "")
		{
			var N = JSON.Parse(request.text);

			retrievedCountry = N["sys"]["country"].Value;
			retrievedCity = N["name"].Value;

			string temp = N["main"]["temp"].Value;
			float tempTemp;
			float.TryParse(temp, out tempTemp);
			float finalTemp = Mathf.Round((tempTemp - 273.0f)*10)/10;

			int.TryParse(N["weather"][0]["id"].Value, out conditionID);
			conditionName = N["weather"][0]["description"].Value;
			conditionImage = N["weather"][0]["icon"].Value;

			myWeatherLabel.text =
				"Country: " + retrievedCountry
					+ "\nCity: " + retrievedCity
					+ "\nTemperature: " + finalTemp + " C"
					+ "\nCurrent Condition: " + conditionName
					+ "\nCondition Code: " + conditionID;
		}
		else
		{
			Debug.Log("WWW error: " + request.error);
		}

		WWW conditionRequest = new WWW("http://openweathermap.org/img/w/" + conditionImage + ".png");
		yield return conditionRequest;
		
		if (conditionRequest.error == null || conditionRequest.error == "")
		{
			//create the material, put in the downloaded texture and make it visible
			var texture = conditionRequest.texture;
			Shader shader = Shader.Find("Unlit/Transparent Colored");
			if (shader != null)
			{
				var material = new Material(shader);
				material.mainTexture = texture;
				myWeatherCondition.material = material;
				myWeatherCondition.color = Color.white;
				myWeatherCondition.MakePixelPerfect();
				myWeatherCondition.width = 300;
				myWeatherCondition.height = 300;
			}
		}
		else
		{
			Debug.Log("WWW error: " + conditionRequest.error);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
