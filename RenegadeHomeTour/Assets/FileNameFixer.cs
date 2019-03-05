using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class FileNameFixer : MonoBehaviour
{
    public string startDir = @"C:\Users\James\Desktop\Planet of Battle II\";
    //public string subDir = "background\\";


    [ContextMenu("renameRecursively")]
    public void renameRecursively()
    {
        rename(startDir);
    }

    public void rename(string dir)
    {
        DirectoryInfo d = new DirectoryInfo(dir);
        FileInfo[] infos = d.GetFiles();

        foreach (FileInfo f in infos)
        {
            Debug.Log("renaming " + f.FullName);
            File.Move(f.FullName, f.FullName.Replace(" (2017_08_22 16_36_28 UTC)", ""));
        }

        DirectoryInfo [] dInfos = d.GetDirectories();

        foreach(DirectoryInfo di in dInfos)
        {
            Debug.Log(">> cd is "+ di.FullName);
            rename(di.FullName);
        }

    }
}