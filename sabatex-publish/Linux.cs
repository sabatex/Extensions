using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.Publish;

public class Linux
{
 
    /// <summary>
    /// Service name
    /// defaul set as ProjectName
    /// </summary>
    public string ServiceName { get; set; }

     /// <summary>
    /// user home folder
    /// The publisher ignore Linux section if null value 
    /// </summary>
    public string? UserHomeFolder { get; set; }
    /// <summary>
    /// The application port use
    /// </summary>
    public int Port { get; set; } = 5000;
    /// <summary>
    /// This is a front end project
    /// </summary>
    public bool FrontEnd { get; set; }
    /// <summary>
    /// Publish folder on linux server
    /// default : /var/www/{ProjectName}
    /// </summary>
    public string? PublishFolder { get; set; }

    /// <summary>
    /// Temp folder in linux side - /home/azureuser/publish
    /// </summary>
    public string? TempFolder => string.IsNullOrWhiteSpace(UserHomeFolder) ? null : UserHomeFolder + "/sabatex";
    /// <summary>
    /// Temp project Folder - /home/azureuser/publish/{projectName}
    /// </summary>
    public string? TempProjectFolder => string.IsNullOrWhiteSpace(TempFolder) ? null : TempFolder + $"/{projectName}";
    /// <summary>
    /// The Bitvise tlp file path
    /// </summary>
    public string? BitviseTlpFile { get; set; }
    /// <summary>
    /// The tar file name as {projectName}.tar.gz
    /// </summary>
    public string TarFileName { private set; get; }
    
       /// <summary>
    /// read from json
    /// </summary>
    public NGINX NGINX { get; set; }=new NGINX();

    
    //public readonly string TarFilePath;
    string  projectName;



    public Linux(string projectName)
    {
       this.projectName = projectName;
       TarFileName = $"{projectName}.tar.gz";
       FrontEnd = false ;
       PublishFolder = "/var/www/" + projectName;
        ServiceName = projectName.ToLower();
    }



}
