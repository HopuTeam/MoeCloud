using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.ILogic
{
    public interface ISite
    {
        Model.Site GetSite();
        bool EditSite(Model.Site site);
    }
}