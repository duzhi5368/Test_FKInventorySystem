using UnityEngine;

#if UNITY_ANDROID
	using System.Text;
	using System.Text.RegularExpressions;
	using System.IO;
#endif

#if UNITY_IPHONE || UNITY_IOS
	using System.Runtime.InteropServices;
#endif
//------------------------------------------------------------------------
namespace FKGame
{
    public class MemoryInfo
    {
        public struct meminf
        {
            public long memtotal;
            public long memfree;
            public long memused;
            public long memavailable;
            public long active;
            public long inactive;
            public long cached;
            public long swapcached;
            public long swaptotal;
            public long swapfree;
        }
        public static meminf minf = new meminf();
        public static bool GetMemoryInfo()
        {
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS
            return getMemInfo();
#else
            return true;
#endif
        }

#if UNITY_ANDROID
        private static Regex re = new Regex(@"\d+");
        private static bool getMemInfo()
        {
            if (!File.Exists("/proc/meminfo")) 
                return false;
            FileStream fs = new FileStream("/proc/meminfo", FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.ToLower().Replace(" ", "");
                if (line.Contains("memtotal")) { minf.memtotal = mVal(line); }
                if (line.Contains("memfree")) { minf.memfree = mVal(line); }
                if (line.Contains("memavailable")) { minf.memavailable = mVal(line); }
                if (line.Contains("active")) { minf.active = mVal(line); }
                if (line.Contains("inactive")) { minf.inactive = mVal(line); }
                if (line.Contains("cached") && !line.Contains("swapcached")) { minf.cached = mVal(line); }
                if (line.Contains("swapcached")) { minf.swapcached = mVal(line); }
                if (line.Contains("swaptotal")) { minf.swaptotal = mVal(line); }
                if (line.Contains("swapfree")) { minf.swapfree = mVal(line); }
            }
            sr.Close(); fs.Close(); fs.Dispose();
            return true;
        }
        private static long mVal(string s)
        {
            Match m = re.Match(s); return long.Parse(m.Value)*1024;
        }
        public static void gc_Collect()
        {
            var jc = new AndroidJavaClass("java.lang.System");
            jc.CallStatic("gc");
            jc.Dispose();
        }
#endif

#if UNITY_IPHONE || UNITY_IOS
	
		[DllImport("__Internal")]
		private static extern long GetAvailableMemory();
    	[DllImport("__Internal")]
		private static extern long GetUsedMemory();
        [DllImport("__Internal")]
		private static extern long GetTotalMemory();

		private static bool getMemInfo()
        {
            if (Application.isEditor)
            {
                return false;
            }
			long rt;
			rt = minf.memfree = GetAvailableMemory();//free
			rt = minf.memused = GetUsedMemory();//used
			if(rt==-1) 
                return false;
			minf.memtotal = GetTotalMemory();
			return true;
		}
#endif

        // 保守预留内存
        const float limitNum = 100;

        // 获取内存上限
        static public float GetMemoryLimit()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                long allMemory = minf.memtotal / 1024 / 1024;
                if (allMemory < 512)
                {
                    return 325f - limitNum;
                }
                else if (allMemory < 1024)
                {
                    return 645f - limitNum;
                }
                else if (allMemory < 2048)
                {
                    return 1395f - limitNum;
                }
                else if (allMemory < 3096)
                {
                    return 2040 - limitNum;
                }
                else if (allMemory >= 3096)
                {
                    return 2040 - limitNum;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
    }
}
