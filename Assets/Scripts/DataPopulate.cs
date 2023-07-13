using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DataPopulate : MonoBehaviour
{
    public GameObject Button;
    public GameObject Container;
    public TMP_Text LongitudeText;
    public TMP_Text LatitudeText;
    public GameObject LocationPanel;
    string jsonData;
    [SerializeField]
    Locations locations;
    void Start()
    {
        StartCoroutine(GetLocationData());
    }

    public void GenerateData(string longitude, string latitude)
    {
        
        LongitudeText.SetText(longitude);
        LatitudeText.SetText(latitude);
        LocationPanel.SetActive(true);
    }

    IEnumerator GetLocationData()
    {
        string URL = "https://muneebjsonpractice.000webhostapp.com/test.php?Country=USA&City=Washington";
        UnityWebRequest locationInfoRequest = UnityWebRequest.Get(URL);
        yield return locationInfoRequest.SendWebRequest();
        
        if(locationInfoRequest.isNetworkError || locationInfoRequest.isHttpError)
        {
            Debug.Log("Error: ");
            yield break;
        }

        jsonData =  "{\"Drop\":" + locationInfoRequest.downloadHandler.text + "}";
        Debug.Log(jsonData);
        locations = JsonUtility.FromJson<Locations>(jsonData);

        foreach(var loc in locations.Drop)
        {
            GameObject Clone = Instantiate(Button, Container.transform);
            Clone.transform.GetChild(0).GetComponent<TMP_Text>().text = loc.ID.ToString();
            Clone.GetComponent<LocationInfo>().ID = loc.ID;
            Clone.GetComponent<LocationInfo>().longitude = loc.longitude;
            Clone.GetComponent<LocationInfo>().latitude = loc.latitude;
            Clone.GetComponent<Button>().onClick.AddListener(() => GenerateData(loc.longitude, loc.latitude));
        }


    }


}
