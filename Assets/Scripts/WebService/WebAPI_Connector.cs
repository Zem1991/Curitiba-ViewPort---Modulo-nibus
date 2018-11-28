using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using ModuloOnibus.DAL;

using UnityEngine;
using System.Collections;

public class WebAPI_Connector : MonoBehaviour
{
    string result;

    void Start()
    {
        //gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);
        //CallWebService();//peanes();
        //imprimePeanes();
    }
    IEnumerator WaitForWWW(WWW www)
    {
        yield return www;


        string txt = "";
        if (string.IsNullOrEmpty(www.error))
            txt = www.text;  //text of success
        else
            txt = www.error;  //error
        //GameObject.Find("Txtdemo").GetComponent<Text>().text = "++++++\n\n" + txt;
    }

    public List<PosicaoGPS> CallWebService(string idBus, string dataHora)
    {
        try
        {
            //GameObject.Find("Txtdemo").GetComponent<Text>().text = "starting..";
            string ourPostData = "{\"plan\":\"TESTA02\"";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            //byte[] b = System.Text.Encoding.UTF8.GetBytes();
            byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
            ///POST by IIS hosting... (string busID, DateTime dateTime)
            //WWW api = new WWW("http://localhost:50436/api/GetBusLocalization", pData, DateTime.Now.ToString());
            ///GET by IIS hosting...
            string[] paramName = new string[2] { "busID", "dateTime" };
            //string[] paramVal = new string[2] { "BD126", "2016-06-29%2020:07:13" };
            string[] paramVal = new string[2] { idBus, dataHora };
            string URI = "http://localhost:50436/api/Values/GetBusLocalization?";
            //http://localhost:50436/api/Values/GetBusLocalization?busid=BD126&dateTime=2016-06-29%2020:07:13.000
            StringBuilder paramz = new StringBuilder();
            for (int i = 0; i < paramName.Length; i++)
            {
                paramz.Append(paramName[i]);
                paramz.Append("=");
                paramz.Append(paramVal[i]);
                //paramz.Append(HttpUtility.UrlEncode(paramVal[i]));
                if (i != paramName.Length - 1)
                {
                    paramz.Append("&");
                }
            }
            URI += paramz.ToString();


            WWW api = new WWW(URI);
            StartCoroutine(WaitForWWW(api));

            while (!api.isDone) { }

            if (api.error == null || api.error == "")
            {
                //Magic stuff happens
                List<PosicaoGPS> gpsList = new List<PosicaoGPS>();

                gpsList = JsonConvert.DeserializeObject<List<PosicaoGPS>>(api.text.ToString());

                return gpsList;

                //foreach (PosicaoGPS gps in gpsList)
                //{
                //    if (gps != null)
                //    {
                //        Debug.Log(gps.ToString() + " | COD_LINHA = " + gps.COD_LINHA + " | DTHR = " + gps.DTHR);
                //    }
                //    if (gpsList == null)
                //    {
                //        var teste = 1;
                //    }
                //}
            }
            else
            {
                //fails
            }
        }
        catch (UnityException ex)
        {
            Debug.Log(ex.Message);
        }
        return null;
    }

    //void peanes()
    //{
    //    string[] paramName = new string[2] { "busID", "dateTime" };
    //    string[] paramVal = new string[2] { "BD126", "2016 - 06 - 29 20:07:13" };
    //    string URI = "http://localhost:50436/api/Values/GetBusLocalization?";
    //    //http://localhost:50436/api/Values/GetBusLocalization?busid=BD126&dateTime=2016-06-29%2020:07:13.000
    //    StringBuilder paramz = new StringBuilder();
    //    for (int i = 0; i < paramName.Length; i++)
    //    {
    //        paramz.Append(paramName[i]);
    //        paramz.Append("=");
    //        paramz.Append(paramVal[i]);
    //        //paramz.Append(HttpUtility.UrlEncode(paramVal[i]));
    //        if (i != paramName.Length - 1)
    //        {
    //            paramz.Append("&");
    //        }
    //    }
    //    URI += paramz.ToString();
    //    HttpWebRequest req = WebRequest.Create(new Uri(URI)) as HttpWebRequest;
    //    req.Method = "GET";
    //    req.ContentType = "text/plain";

    //    // Build a string with all the params, properly encoded.
    //    // We assume that the arrays paramName and paramVal aret
    //    // of equal length:


    //    // Encode the parameters as form data:
    //    byte[] formData = Encoding.UTF8.GetBytes(paramz.ToString());
    //    req.ContentLength = formData.Length;

    //    //Stream newStream = req.GetRequestStream();
    //    //newStream.Write(formData, 0, formData.Length);
    //    //newStream.Close();
    //    WebResponse resp = req.GetResponse();

    //    StreamReader reader = new StreamReader(resp.GetResponseStream());
    //    result = reader.ReadToEnd();
    //}

    void imprimePeanes()
    {
        Debug.Log(result);
    }

}


