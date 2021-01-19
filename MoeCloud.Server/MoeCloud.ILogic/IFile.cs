using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.ILogic
{
    public interface IFile
    {
        bool Create(Model.File file);
        bool Cycle(int id, int delDay);
        bool DelFile(int id);
        bool EditName(int id, string Name);
        Model.File GetFile(int userID, int parentID);
        List<Model.File> GetFiles(int userID, int parentID);
        List<Model.File> GetCycleFiles(int userID);
        List<Model.File> GetAllFiles();
        bool EditPath(int id, string path);
        Model.File DirFind(string dir);
    }
}