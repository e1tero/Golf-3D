#pragma warning disable 649
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Sources
{
    public class RemoteServiceProcessing : MonoBehaviour
    {
        private static RemoteServiceProcessing _instance;
        private RemoteConfig _config = null;
        private int _uid = 0;
        private string _device = null;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            var go = new GameObject();
            DontDestroyOnLoad(go);

            go.AddComponent<RemoteServiceProcessing>();
            go.name = "RemoteServiceProcessing";
            go.hideFlags = HideFlags.HideAndDontSave;
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(_instance);
                return;
            }

            _config = Resources.Load<RemoteConfig>("RemoteConfig");
            _instance = this;
            _instance._device = SystemInfo.deviceUniqueIdentifier;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public static void Request(IServiceRequest requestData, Action responseCallback, Action<string, int> errorCallback)
        {
            _instance.StartCoroutine(_instance.RequestResponse(requestData, responseCallback, errorCallback));
        }

        public static void Request<T>(IServiceRequest requestData, Action<T> responseCallback, Action<string, int> errorCallback)
        {
            _instance.StartCoroutine(_instance.RequestResponse(requestData, responseCallback, errorCallback));
        }

        public void CancelAllRequests()
        {
            StopAllCoroutines();
        }
        
        private IEnumerator RequestResponse<T>(IServiceRequest requestData, Action<T> responseCallback, Action<string, int> errorCallback)
        {
            var requestJson = JsonUtility.ToJson(requestData);
            if (_config.Debug) Debug.Log("Request:" + requestJson);            
            
            using (UnityWebRequest www = UnityWebRequest.Post(_config.Url, CreateFormData(requestData.Method, requestJson)))
            {
                yield return www.SendWebRequest();

                ServiceResponse<T> responce;
                string error = "";
                int code = 0;
                if (CheckErrorAndDecodeData(www, out responce, out error, out code))
                {
                    _uid = responce.Id;
                    if (responseCallback != null) responseCallback(responce.Body);
                }
                else if (errorCallback != null) errorCallback(error, code);
            }
        }

        private IEnumerator RequestResponse(IServiceRequest requestData, Action responseCallback, Action<string, int> errorCallback)
        {
            var requestJson = JsonUtility.ToJson(requestData);
            if (_config.Debug) Debug.Log("Request:\n" + requestJson);

            using (UnityWebRequest www = UnityWebRequest.Post(_config.Url, CreateFormData(requestData.Method, requestJson)))
            {
                yield return www.SendWebRequest();

                ServiceResponse responce;
                string error = "";
                int code = 0;
                if (CheckErrorAndDecodeData(www, out responce, out error, out code))
                {
                    _uid = responce.Id;
                    if (responseCallback != null) responseCallback();
                }
                else if (errorCallback != null) errorCallback(error, code);
            }
        }
        

        private WWWForm CreateFormData(string method, string requestJson)
        {
            var wwwForm = new WWWForm();
            wwwForm.AddField("body", requestJson);
            if (_uid != 0) wwwForm.AddField("uid", _uid);
            wwwForm.AddField("m", method);
            wwwForm.AddField("device", _device);
            return wwwForm;
        }

        private string Md5Sum(string strToEncrypt)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            // encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            string md5Result = hashString.PadLeft(32, '0');            

            return md5Result;
        }

        private bool CheckErrorAndDecodeData<T>(UnityWebRequest www, out T response, out string error, out int code) where T : IServiceResponse
        {
            response = default(T);
            error = "";
            code = 0;
            var data = www.downloadHandler.text;
            var wError = www.error;
            www.Dispose();

            if (string.IsNullOrEmpty(data))
            {
                error = "Respounce is null";
                code = -1;
                CallError("Server return is NULL; Server return error: " + wError);
                return false;
            }

            if (!string.IsNullOrEmpty(wError))
            {
                error = wError;
                code = -2;                
                CallError("Server return error: " + wError);
                return false;
            }

            try
            {                
                if (_config.Debug) Debug.Log("Response:" + data);

                if (data.Trim() == "{}") return true;

                response = JsonUtility.FromJson<T>(data);

                if (response == null)
                {
                    error = "Failed serialize data";
                    code = -3;
                    return false;
                }

                if (response != null && response.Code != 0)
                {
                    error = response.Error;
                    code = response.Code;
                    CallError("Response error: " + response.Error);
                    return false;
                }

                //string hash = Md5Sum(response.Date + _config.HashKey);
                //if (response.Hash != hash)
                //{
                //    code = -11;
                //    error = "Wrong signature response";
                //    CallError(error);
                //    return false;
                //}

                return true;
            }
            catch (Exception ex)
            {
                CallError("Error json decode data; Server return text: " + data);
                Debug.LogException(ex);
            }

            return false;
        }

        private static void CallError(string message)
        {
            Debug.LogError(message);
        }
    }

    [System.Serializable]
    public struct ServiceResponse : IServiceResponse
    {
        public int Code => code;
        public string Error => error;
        public string Hash => hash;
        public string Date => date;
        public int Id => uid;

        [SerializeField] private string error;
        [SerializeField] private int code;
        [SerializeField] private string date;
        [SerializeField] private string hash;
        [SerializeField] private int uid;
    }

    [System.Serializable]
    public struct ServiceResponse<T> : IServiceResponse
    {
        public int Code => code;
        public string Error => error;
        public string Hash => hash;
        public string Date => date;
        public int Id => uid;
        public T Body => body;

        [SerializeField] private string error;
        [SerializeField] private int code;
        [SerializeField] private string date;
        [SerializeField] private string hash;
        [SerializeField] private int uid;
        [SerializeField] private T body;
    }

    public interface IServiceResponse
    {
        int Code { get; }
        string Error { get; }
        string Hash { get; }
        string Date { get; }
        int Id { get; }
    }

    public interface IServiceRequest
    {
        string Method { get; }
    }

    public static class ServiceExtensions
    {
        public static void Send(this IServiceRequest request, Action onResponse = null, Action<string, int> onError = null)
        {
            RemoteServiceProcessing.Request(request, onResponse, onError);
        }

        public static void Send<TResponse>(this IServiceRequest request, Action<TResponse> onResponse, Action<string, int> onError = null)
        {
            RemoteServiceProcessing.Request(request, onResponse, onError);
        }
    }
}