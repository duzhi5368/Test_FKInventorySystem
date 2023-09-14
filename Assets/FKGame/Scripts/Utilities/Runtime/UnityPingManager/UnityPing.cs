using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UnityPing : MonoBehaviour
    {
        private Action<UnityPingResult> s_pingResultCallback = null;
        private Action<UnityPingResult[]> s_OnComplete;
        private NetworkReachability network;
        private List<UnityPingRuntimeData> pingRuntimeDatas = new List<UnityPingRuntimeData>();
        private List<UnityPingRuntimeData> pingFinishedDatas = new List<UnityPingRuntimeData>();
        private List<UnityPingResult> pingResults = new List<UnityPingResult>();

        public static UnityPing CreatePing(string host, Action<UnityPingResult> pingResultCallback, 
            Action<UnityPingResult[]> OnComplete = null, float timeOut = 5f, int runTimes = 1)
        {
            if (string.IsNullOrEmpty(host)) 
                return null;
            if (pingResultCallback == null) 
                return null;

            GameObject go = new GameObject("[UnityPing]");
            DontDestroyOnLoad(go);
            UnityPing s_unityPing = go.AddComponent<UnityPing>();
            s_unityPing.s_pingResultCallback = pingResultCallback;
            s_unityPing.s_OnComplete = OnComplete;
            s_unityPing.StartRun(host, timeOut, runTimes);
            return s_unityPing;
        }

        // HOST to IP
        private string ResolveHostNameToIP(string host, out AddressFamily af)
        {
            af = AddressFamily.Unknown;
            IPAddress ipAddr;
            if (IPAddress.TryParse(host, out ipAddr))
            {
                af = ipAddr.AddressFamily;
                byte[] ipb = ipAddr.GetAddressBytes();
                StringBuilder sb = new StringBuilder();
                foreach (var b in ipb)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(".");
                    }
                    sb.Append(b);
                }
                return sb.ToString();
            }
            IPAddress[] AddressList = null;
            try
            {
                IPHostEntry IPinfo = Dns.GetHostEntry(host);
                AddressList = IPinfo.AddressList;
            }
            catch (SocketException e)
            {
                AddressList = new IPAddress[] { };
                Debug.LogError(host + " Dns.GetHostAddresses:" + e);
            }
            foreach (var ip in AddressList)
            {
                af = ip.AddressFamily;
                // IPv4
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    byte[] ipb = ip.GetAddressBytes();
                    StringBuilder sb = new StringBuilder();
                    foreach (var b in ipb)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(".");
                        }
                        sb.Append(b);
                    }
                    return sb.ToString();
                }
                // IPv6
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    byte[] ipb = ip.GetAddressBytes();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < ipb.Length; ++i)
                    {
                        if (i % 2 == 0)
                        {
                            if (sb.Length > 0)
                            {
                                sb.Append(":");
                            }
                        }
                        sb.Append(ipb[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            af = AddressFamily.Unknown;
            return string.Empty;
        }

        private void StartRun(string host, float timeOut, int runTimes)
        {
            if (runTimes <= 0)
            {
                Debug.LogError("Ping runTimes cant < 1, now:" + runTimes);
                Destroy(gameObject, 1f);
                return;
            }
            AddressFamily af;
            string ip = ResolveHostNameToIP(host, out af);
            Debug.Log("host :" + host + " to ip :" + ip + " AddressFamily:" + af);
            network = Application.internetReachability;
            for (int i = 0; i < runTimes; i++)
            {
                UnityPingRuntimeData runtimeData = new UnityPingRuntimeData();
                runtimeData.ip = ip;
                runtimeData.host = host;
                runtimeData.s_timeout = timeOut;
                runtimeData.currentUseTime = 0;
                runtimeData.delayStartTime = 0.05f * (i + 1);
                if (string.IsNullOrEmpty(ip))
                {
                    runtimeData.isFinish = true;
                    runtimeData.errorReason = ErrorReason.UnreachableAddress;
                }
                pingRuntimeDatas.Add(runtimeData);
            }
        }
        
        private void Update()
        {
            foreach (var runtimeData in pingRuntimeDatas)
            {
                if (runtimeData.isFinish)
                {
                    pingFinishedDatas.Add(runtimeData);
                    continue;
                }
                if (runtimeData.delayStartTime > 0)
                {
                    runtimeData.delayStartTime -= Time.unscaledDeltaTime;
                }
                else
                {
                    if (runtimeData.ping == null)
                    {
                        runtimeData.ping = new Ping(runtimeData.ip);
                    }
                    RunPingAction(runtimeData);
                }
            }
            foreach (var item in pingFinishedDatas)
            {
                pingRuntimeDatas.Remove(item);
                CallResult(item);
            }
            pingFinishedDatas.Clear();
        }

        private void OnDestroy()
        {
            s_pingResultCallback = null;
            pingFinishedDatas.Clear();
            pingResults.Clear();
        }

        private void RunPingAction(UnityPingRuntimeData runtimeData)
        {
            switch (network)
            {
                case NetworkReachability.ReachableViaCarrierDataNetwork:    // 3G/4G
                case NetworkReachability.ReachableViaLocalAreaNetwork:      // WIFI
                {
                    if (runtimeData.ping.isDone)
                    {
                        runtimeData.isFinish = true;
                        if (runtimeData.ping.time == -1)
                        {
                            runtimeData.errorReason = ErrorReason.TimeOut;
                        }
                    }
                    else
                    {
                        if (runtimeData.currentUseTime >= runtimeData.s_timeout)
                        {
                            runtimeData.isFinish = true;
                            network = Application.internetReachability;
                            if (network == NetworkReachability.NotReachable)
                            {
                                runtimeData.errorReason = ErrorReason.NetNotReachable;
                            }
                            else
                                runtimeData.errorReason = ErrorReason.TimeOut;
                        }
                        else
                        {
                            runtimeData.currentUseTime += Time.unscaledDeltaTime;
                        }
                    }
                }
                break;
                case NetworkReachability.NotReachable: // Õ¯¬Á≤ªø…”√
                default:
                {
                    runtimeData.isFinish = true;
                    runtimeData.errorReason = ErrorReason.NetNotReachable;
                    network = Application.internetReachability;
                }
                break;
            }
        }

        private void CallResult(UnityPingRuntimeData runtimeData)
        {
            int time = runtimeData.ping == null ? -1 : runtimeData.ping.time;
            bool isSuccess = runtimeData.errorReason == ErrorReason.None ? true : false;
            UnityPingResult result = new UnityPingResult(isSuccess, runtimeData.errorReason, runtimeData.host, runtimeData.ip, time);
            pingResults.Add(result);
            if (runtimeData.ping != null)
            {
                runtimeData.ping.DestroyPing();
            }
            if (s_pingResultCallback != null)
            {
                s_pingResultCallback(result);
            }
            if (pingRuntimeDatas.Count <= 0)
            {
                if (s_OnComplete != null)
                {
                    s_OnComplete(pingResults.ToArray());
                }
                Destroy(this.gameObject, 0.5f);
            }
        }
    }
}