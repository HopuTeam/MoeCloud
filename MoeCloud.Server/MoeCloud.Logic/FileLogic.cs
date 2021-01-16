using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MoeCloud.Logic
{
    public class FileLogic : ILogic.IFile
    {
        private Data.CoreEntities EF { get; }
        public FileLogic(Data.CoreEntities _ef)
        {
            EF = _ef;
        }

        // 往数据库添加文件记录
        public bool Create(Model.File file)
        {
            try
            {
                file.AddTime = DateTime.Now;
                EF.Add(file);
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 把文件放入回收站并添加删除标记，如果delDay为0则撤回删除标记
        public bool Cycle(int id, int delDay)
        {
            try
            {
                var mod = EF.Files.FirstOrDefault(x => x.ID == id);
                if (delDay == 0)
                    mod.DelTime = null;
                else
                    mod.DelTime = DateTime.Now.AddDays(delDay);
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /*
         * 删除回收站中过期的文件，彻底删除该文件
         * 通过定时访问路径执行，例/File/Corn?key=
         */
        public bool DelFile(int id)
        {
            try
            {
                EF.Remove(EF.Files.FirstOrDefault(x => x.ID == id));
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 文件重命名
        public bool EditName(int id, string Name)
        {
            try
            {
                var mod = EF.Files.FirstOrDefault(x => x.ID == id);
                mod.Name = Name;
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 通过实际虚路径查找文件信息
        public Model.File DirFind(string dir)
        {
            return EF.Files.FirstOrDefault(x => x.Path == dir);
        }

        /// <summary>
        /// 用户查询自己的文件列表
        /// </summary>
        /// <param name="userID">用户的ID</param>
        /// <param name="parentID">目录ID，默认0为根目录</param>
        /// <returns>返回List数据</returns>
        public List<Model.File> GetFiles(int userID, int parentID)
        {
            return EF.Files.Where(x => x.UserID == userID && x.ParentID == parentID && x.DelTime == null).ToList();
        }

        //查询已删除的文件列表
        public List<Model.File> GetCycleFiles(int userID)
        {
            return EF.Files.Where(x => x.UserID == userID && x.DelTime < DateTime.Now).ToList();
        }

        //管理员后台查看所有文件
        public List<Model.File> GetAllFiles()
        {
            return EF.Files.Where(x => x.ParentID == 0 && x.DelTime == null).ToList();
        }

        // 移动文件(修改路径)
        public bool EditPath(int id, string path)
        {
            try
            {
                var mod = EF.Files.FirstOrDefault(x => x.ID == id);
                mod.Path = path;
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