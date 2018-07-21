using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

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

        if (Input.GetKeyDown(KeyCode.U)) {
            StartCoroutine(
                sendImageAnalyseRequest(
                    ScreenCapture.CaptureScreenshotAsTexture(),
                    "",
                    true,
                    100,
                    1000
                )
            );
        }
	}

    public string SendRequest()
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://respekt.justdied.com:8080/storyFromSentences");
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
        catch (Exception e)
        {
            return e.Message;
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

    const string SERVER_URL = "http://respekt.justdied.com:8080/storyFromImage";

    /**
     * Send an image to the narration server.
     * image : The image to be analysed.
     * sentences : A group of sentences to influence analyse result.
     * poeticFilter : Wether to enable the poetic filter or not (works by removing frequent or unfrequent words)
     * minfreq : Remove less frequent words (represent an average occurency in a text corpus).
     * maxFreq : Remove more frequent words (like minFreq).
     */
    IEnumerator sendImageAnalyseRequest(
        Texture2D image,
        string sentences = "",
        bool poeticFilter = false,
        int minFreq = 10,
        int maxFreq = 1000
    ) {
        byte[] jpgImageData = image.EncodeToJPG();

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", jpgImageData, "file.jpg", "image/jpeg");
        form.AddField("sentences", sentences);

        if (poeticFilter) {
            form.AddField("poeticFilter", "on");
            form.AddField("minFreq", minFreq);
            form.AddField("maxFreq", maxFreq);
        }

        using (var request = UnityWebRequest.Post(SERVER_URL, form)) {

            yield return request.SendWebRequest();

            if(request.isNetworkError) {
                Debug.Log(request.error);
            } else {
                try {
                    JSONResponse data = JsonConvert.DeserializeObject<JSONResponse>(System.Text.Encoding.UTF8.GetString(request.downloadHandler.data));

                    if (data.status.Equals("OK")) {
                        Debug.Log(data.generatedText);
                    } else {
                        Debug.Log(data.error);
                    }
                } catch  {
                    Debug.Log("Error parsing server response.");
                }
            }
        }
    }
}
