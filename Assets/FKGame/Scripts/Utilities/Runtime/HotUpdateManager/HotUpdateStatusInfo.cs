namespace FKGame
{
	public struct HotUpdateStatusInfo
	{
		public HotUpdateStatusEnum m_status;
		public LoadState m_loadState;
		public bool isFailed;

		static HotUpdateStatusInfo s_info = new HotUpdateStatusInfo();
		public static HotUpdateStatusInfo GetUpdateInfo(HotUpdateStatusEnum status, float progress)
		{
			s_info.m_status = status;

			if (s_info.m_loadState == null)
			{
				s_info.m_loadState = new LoadState();
			}

			if (progress == 1)
			{
				s_info.m_loadState.isDone = true;
			}
			else
			{
				s_info.m_loadState.isDone = false;
			}

			if (status == HotUpdateStatusEnum.Md5FileDownLoadFail ||
				status == HotUpdateStatusEnum.UpdateFail ||
				status == HotUpdateStatusEnum.VersionFileDownLoadFail)
			{
				s_info.isFailed = true;
			}
			else
			{
				s_info.isFailed = false;
			}

			s_info.m_loadState.progress = progress;

			return s_info;
		}
	}
}