using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MoeCloud.Logic
{
    public class SiteLogic : ILogic.ISite
    {
        private Data.CoreEntities EF { get; }
        public SiteLogic(Data.CoreEntities _ef)
        {
            EF = _ef;
        }

        //查询
        public Model.Site GetSite()
        {
            return EF.Sites.FirstOrDefault();
        }

        //修改
        public bool EditSite(Model.Site site)
        {
            try
            {
                var mod = EF.Sites.FirstOrDefault();
                mod.MainTitle = site.MainTitle;
                mod.SecTitle = site.SecTitle;
                mod.Description = site.Description;
                mod.Url = site.Url;
                mod.Script = site.Script;
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}