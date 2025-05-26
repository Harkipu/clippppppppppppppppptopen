using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class SignUpData
{
    public string email;
    public string password;

    public SignUpData(string email, string password)
    {
        this.email = email;
        this.password = password;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}

public class SignUpFormHandler : MonoBehaviour
{
    public InputField emailInput;
    public InputField passwordInput;
    public InputField reEnterPasswordInput;
    public Button submitButton;

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
    }

    void OnSubmit()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string rePassword = reEnterPasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rePassword))
        {
            Debug.LogWarning("All fields are required.");
            return;
        }

        if (password != rePassword)
        {
            Debug.LogWarning("Passwords do not match.");
            return;
        }

        SignUpData data = new SignUpData(email, password);
        string jsonData = data.ToJson();

        Debug.Log("Prepared JSON: " + jsonData);
        StartCoroutine(SendDataToServer(jsonData));
    }

    IEnumerator SendDataToServer(string jsonData)
    {
        string url = "https://binusgat.rf.gd/unity-api-test/api/auth/signup.php"; 

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Successfully sent: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}
