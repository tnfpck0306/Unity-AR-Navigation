using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ambiens.archtoolkit.atexplore.utils
{
    public static class FileAssetCache
    {
        public static string GetCacheName(string url)
        {
            var split = Path.GetExtension(url).Split('?');
            var realUrl = url.Split('?')[0];
            var ext = split[0];
            //var cache = (split.Length > 1) ? split[1] : "";
            return Application.persistentDataPath + separator + "txtcache" + separator + "c_" + MD5(realUrl) + ext;
        }
        public static bool AlreadyCached(string onlineUrl)
        {
            string fname = GetCacheName(onlineUrl);
            //Debug.Log("Cache name " + fname);
            //Debug.Log("EXISTS: " + System.IO.File.Exists(fname));
            if (fname != "" && System.IO.File.Exists(fname))
            {
                return true;
            }
            return false;
        }
        public static void SaveCacheAssetFromUrl(string localUrl, string onlineUrl, Action<UnityWebRequestAsyncOperation, UnityWebRequest> OnStart)
        {
            string fname = GetCacheName(onlineUrl);
            if (fname != "" && System.IO.File.Exists(fname))
            {
                Debug.Log("FILE CACHE ESISTENTE");
            }
            else
            {
                FileAssetCache.FetchAssetFromUrl(localUrl, OnStart, null,null, fname);
            }
        }
        public static void FetchCachedAssetFromUrl(
            string url,
            Action<UnityWebRequestAsyncOperation, UnityWebRequest> OnStart,
            Action<string> OnComplete,
            Action<string> OnError,
            bool createCache = true)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string fname = (createCache) ? GetCacheName(url) : "";
                Debug.Log(fname);
                if (fname != "" && System.IO.File.Exists(fname))
                {
                    if (OnComplete != null)
                    {
                        OnComplete(fname);
                    }
                }
                else
                {
                    Debug.Log("Getting file From url " + url);
                    FileAssetCache.FetchAssetFromUrl(url, OnStart, OnComplete, OnError, fname);
                }
            }
            else
            {
                if (OnError != null)
                {
                    OnError("FileNameIsNull!");
                }
            }
        }
        public static void FetchAssetFromUrl(string url,
            Action<UnityWebRequestAsyncOperation, UnityWebRequest> OnStart,
            Action<string> OnComplete, 
            Action<string> OnError,
            string filename = "")
        {
            Debug.LogWarning("Fetching url: " + url);
            var www = UnityWebRequest.Get(url);

            var operation = www.SendWebRequest();
            OnStart(operation,www);

            operation.completed += (AsyncOperation o) =>
            {
                if (!String.IsNullOrEmpty(www.error))
                {
                    if (OnError != null)
                        OnError(www.error);
                }
                else
                {
                    byte[] d = www.downloadHandler.data;

                    if (filename != "" && d != null)
                    {
                        WriteAllBytes(filename, d);
                    }
                    if (OnComplete != null)
                    {
                        if (filename != "")
                            OnComplete(filename);
                        else
                            OnComplete(url.Replace("file://", ""));
                    }
                }
            };
        }

        public static string MD5(string input)
        {
#if UNITY_WP8 || UNITY_METRO
            byte[] data = MD5Core.GetHash ( input );
#else
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = System.Security.Cryptography.MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
#endif
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private static byte[] ReadAllBytes(string filePath)
        {
            byte[] buffer = null;
            if (File.Exists(filePath))
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                try
                {
                    int length = (int)fileStream.Length;  // get file length
                    buffer = new byte[length];            // create buffer
                    int count;                            // actual number of bytes read
                    int sum = 0;                          // total number of bytes read
                                                          // read until Read method returns 0 (end of the stream has been reached)
                    while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    {
                        sum += count;  // sum is a buffer offset for next reading
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.LogWarning("ReadAllBytes: exception on read file " + filePath + ", " + e.Message);
#endif
                }
                finally
                {
                    fileStream.Close();
                }
            }
            else
            {
#if DEBUG
                Debug.LogWarning("ReadAllBytes: can't read file " + filePath + ", it doesn't exists");
#endif
            }
            return buffer;
        }

        public static string separator = "/";

        private static void WriteAllBytes(string filePath, byte[] b)
        {
            if (b != null)
            {
                string path = filePath.Replace(separator + Path.GetFileName(filePath), "");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (FileStream file = File.Open(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    file.Write(b, 0, b.Length);
                    file.Close();
                }
            }
            else
            {
                Debug.LogWarning("WriteAllBytes: can't save file " + filePath + ", byte[] is empty");
            }
        }
    }
}