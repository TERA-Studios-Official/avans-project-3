// DBGate.cs
//
// Description:
// A gate to the database, allowing for sing-in requests and data saving.
//
// Author:
// t.teulings
//
// Date of last amendment:
// 09/04/2026

using System;


using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Text;
public class DBGate : MonoBehaviour
{
    public GameObject loginPanel;

    private LoginData ld = new LoginData();
    private EnvironmentData ed = new EnvironmentData();

    public bool failedJson;

    /// <summary>
    /// Changes the email/username that will be entered upon login.
    /// </summary>
    /// <param name="newEmail"></param>
    public void ChangeEnterEmail(string newEmail)
    {
        ld.email = newEmail;
    }

    /// <summary>
    /// Changes the password that will be entered upon login.
    /// </summary>
    /// <param name="newPassword"></param>
    public void ChangeEnterPassword(string newPassword)
    {
        ld.password = newPassword;
    }

    /// <summary>
    /// Calls the login process.
    /// </summary>
    public void Login()
    {
        StartCoroutine(PostLogin());
    }

    /// <summary>
    /// Sends a login request to the server using the provided email and password, and yields until the response is
    /// received.
    /// </summary>
    /// <remarks>
    /// This method uses Unity's web request system to send a POST request with email and password
    /// credentials in JSON format. The request is sent to the specified login endpoint. If the request fails, an error
    /// is logged; otherwise, the response is logged. This method is intended to be used with Unity's coroutine
    /// system.
    /// </remarks>
    /// <returns>An enumerator that performs the asynchronous login operation. The enumerator yields while waiting for the server
    /// response.</returns>
    IEnumerator PostLogin()
    {
        string url = "https://localhost:7222/account/login";

        // Unity web magic.
        string postData = JsonUtility.ToJson(ld);

        UnityWebRequest req = UnityWebRequest.Post(url, postData, "application/json");
        yield return req.SendWebRequest();

        failedJson = req.result != UnityWebRequest.Result.Success;

        if (failedJson)
        {
            Debug.LogError($"JSON POST failed: {req.error} (code {req.responseCode})");
        }
        else
        {
            Debug.Log($"JSON POST success: {req.responseCode} response: {req.downloadHandler.text}");

            ld.email = "???";
            ld.password = "???";

            loginPanel.SetActive(false);
        } 
    }

    /// <summary>
    /// Changes the world name that will be entered upon world creation.
    /// </summary>
    /// <param name="newName"></param>
    public void ChangeEnterWorldname(string newName)
    {
        ed.Name = newName;
    }

    /// <summary>
    /// Changes the world's maximum height that will be entered upon world creation.
    /// </summary>
    /// <param name="newHeight"></param>
    public void ChangeEnterMaxheight(string newHeight)
    {
        ed.MaxHeight = Convert.ToInt32(newHeight);
    }

    /// <summary>
    /// Changes the world's maximum length that will be entered upon world creation.
    /// </summary>
    /// <param name="newLength"></param>
    public void ChangeEnterMaxlength(string newLength)
    {
        ed.MaxLength = Convert.ToInt32(newLength);
    }

    /// <summary>
    /// Calls the environment creation process.
    /// </summary>
    public void CreateEnvironment()
    {
        StartCoroutine(PostEnvironment());
    }

    /// <summary>
    /// Creates an environment and saves it to the database.
    /// </summary>
    /// <returns></returns>
    IEnumerator PostEnvironment()
    {
        string url = "https://localhost:7222/account/api/environment2d/";

        string postData = JsonUtility.ToJson(ed);

        UnityWebRequest req = UnityWebRequest.Post(url, postData, "application/json");
        yield return req.SendWebRequest();

        bool failedJson2 = req.result != UnityWebRequest.Result.Success;

        if (failedJson2)
        {
            Debug.LogError($"JSON POST failed: {req.error} (code {req.responseCode})");
        }
        else
        {
            Debug.Log($"JSON POST success: {req.responseCode} response: {req.downloadHandler.text}");
        }
    }

    /// <summary>
    /// Sneaky way to bypass the money-requiring certificate. Clever!
    /// </summary>
    private class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData) => true;
    }
}

[Serializable]
public class LoginData
{
    public string email;
    public string password;
};

[Serializable]
public class EnvironmentData
{
    public string Id = "ENV00001";
    public string Name = "TestEnv";
    public int MaxHeight = 1024;
    public int MaxLength = 2048;
};