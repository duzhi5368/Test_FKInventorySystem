namespace FKGame
{
    public enum HotUpdateStatusEnum
    {
        NoUpdate,                   // 无需更新
        NeedUpdateApplication,      // 需要整包更新

        VersionFileDownLoadFail,    // 版本文件下载失败
        Md5FileDownLoadFail,        // Md5文件下载失败
        UpdateFail,                 // 更新失败
        UpdateSuccess,              // 更新成功

        DownLoadingVersionFile,     // 下载版本文件中
        DownLoadingManifestFile,    // 下载清单文件中
        Updating,                   // 更新中
    }
}