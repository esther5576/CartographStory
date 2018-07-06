using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;

public class MachineCall : MonoBehaviour {

    class JSONResponse
    {
        public string status;
        public string generatedText;
        public string error;
    }

    class JSONRequest
    {
        public List<string> words = new List<string>();
    }

    public List<string> objectsSeen = new List<string>();
    public Text textToDisplay;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            textToDisplay.text = SendRequest();
            objectsSeen.Clear();
        }
	}

    public string SendRequest()
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://respekt.justdied.com:8080/storyFromWords");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        httpWebRequest.Timeout = 1000;

        try
        {
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                JSONRequest request = new JSONRequest();
                request.words.AddRange(objectsSeen);

                string json = JsonConvert.SerializeObject(request);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
        catch
        {
            return "... BZZZrrrrTtttt ... Cannot analyse data ... Please try to validate data again.";
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            JSONResponse data = JsonConvert.DeserializeObject<JSONResponse>(result);
            if (data.status.Equals("OK"))
            {
                return data.generatedText;
            }else
            {
                return data.error;
            }
            return "Error no text";
        }
    }
}
