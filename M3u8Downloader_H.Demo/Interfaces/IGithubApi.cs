using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.Demo.Interfaces
{
    internal interface IGithubApi
    {
        [Get("/user/{username}")]
        Task<string> GetUser(string username);
    }
}
