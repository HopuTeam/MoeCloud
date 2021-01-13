using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.ILogic
{
    public interface IFileUploadBLL
    {
        int Upload(string rootpath, int index, StringValues guid, IFormFile data);

        int Merge(string rootpath,  StringValues guid, StringValues fileName);
    }
}
