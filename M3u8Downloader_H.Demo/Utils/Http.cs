using M3u8Downloader_H.Abstractions.Models;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.Demo.Utils
{
    public class Http<T> : IDisposable 
        where T : class
    {
        private T? _api;
        private readonly IApiFactory apiFactory;
        private readonly string baseUrl;

        public Http(IApiFactory apiFactory,string baseUrl)
        {
            this.apiFactory = apiFactory;
            this.baseUrl = baseUrl;
            apiFactory.ProxyChanged += ApiFactory_ProxyChanged;
        }

        private void ApiFactory_ProxyChanged()
        {
            _api = apiFactory.Create<T>(baseUrl,null);
        }

        public T GetApi()
        {
           return  _api ??= apiFactory.Create<T>(baseUrl, null);
        }

        public void Dispose()
        {
            apiFactory.ProxyChanged -= ApiFactory_ProxyChanged;
            GC.SuppressFinalize(this);
        }
    }
}
