# Create NUGET package and publish NUGET
# %NUGET_AUTH_TOKEN_PATH% - the path to file with nuget token, default = %HOMEPATH%\.scripts\NUGET_AUTH_TOKEN
# %PROJECT_FILE% - the path to project .csproj file

import os
import json
import sys
from xml.etree import ElementTree



def ALPHA()->str:"alpha"
def BETA()->str:"beta"
def RC()->str:"rc"
def RELEACE()->str:"releace"

def getNUGETConfig(nugetFile:str):
    if os.path.exists(nugetFile):
        f = open(nugetFile , "r" )
        config = json.loads(f.read())
        f.close()
        return config
    else:
        return {"projVersion":"","version": "1.0.0", "buildVersion": 4, "stage": "alpha","stages":{ALPHA,BETA,RC,RELEACE}}    

def checkVersionFromProj(projectFile:str,nugetConf):
    if not os.path.exists(projectFile):
        raise "The file " + projectFile + " do not exist!"
    #read version from csproj
    pf = open(projectFile,"r",encoding="UTF8")
    pfXML = ElementTree.parse(pf)
    pf.close()
    versXML = pfXML.find("PropertyGroup/Version")
    if versXML != None:
        nugetConf["projVersion"] = versXML.text 
    else:
        nugetConf["projVersion"] = ""    

    # version and stage set
    if nugetConf["projVersion"] != "":
        sv = nugetConf["projVersion"].split('.');
        if len(sv) != 3:
            raise "Wrong version in *.csproj file, must be x.x.x "
        #set nuget version
        nugetConf["version"] = nugetConf["projVersion"]    
    sv = nugetConf["version"].split('.');
    if len(sv) != 3:
        raise "Wrong version in *.csproj file, must be x.x.x "

    nv = int(sv[2])
    if nv!=0:
       #not debug version
       if nugetConf["stage"] !="release":
           nugetConf["stage"]="release"
#pack project
def pack()->bool:
    os.system("del %outputDir%\\*.nupkg")
    if isDebug:
        return os.system("dotnet pack --configuration %configurationBuild% %PROJECT_FILE% -p:PackageVersion=%PackageVersion% --include-source") == 0
    else:
        return os.system("dotnet pack --configuration %configurationBuild% %PROJECT_FILE% -p:PackageVersion=%PackageVersion%") == 0

def publish()->bool:
    if isDebug:
        os.system("dotnet nuget push %outputDir%\\*.symbols.nupkg -k %NUGET_AUTH_TOKEN% -s https://api.nuget.org/v3/index.json --skip-duplicate") ==0
    else:
        os.symtem("dotnet nuget push %outputDir%\\*.nupkg -k %NUGET_AUTH_TOKEN% -s https://api.nuget.org/v3/index.json --skip-duplicate") ==0    

def writeConfig(conf):
    conf["buildVersion"] = conf["buildVersion"] + 1; 
    result = json.dumps(conf)
    f = open(nugetFile , "w" )
    f.write(result)
    f.close()

if len(sys.argv) >=2:
    os.environ["PROJECT_FILE"] =sys.argv[1]    
#os.environ["PROJECT_FILE"] = "C:\\Users\\serhi\\source\\repos\\sabatex\\WebApiDocumentsExcange\\Sabatex.Core\\src\\Sabatex.Core.csproj"

# The envinronment must be include variable project path
if not "PROJECT_FILE" in os.environ:
    print("The environment projectFile is not defined")
    exit()
else:
    projectFile = os.environ["PROJECT_FILE"]


if not "NUGET_AUTH_TOKEN_PATH" in os.environ:
    ft = os.environ["HOMEPATH"]+"\\.scripts\\NUGET_AUTH_TOKEN"
    if not os.path.exists(ft):
        print("The environment projectFile is not defined")
        exit()
    os.environ["NUGET_AUTH_TOKEN_PATH"] = os.environ["HOMEPATH"]+"\\.scripts\\NUGET_AUTH_TOKEN"

os.environ["NUGET_AUTH_TOKEN"]=open(os.environ["NUGET_AUTH_TOKEN_PATH"] , "r" ).readline()
projectFolder = os.path.dirname(projectFile)

os.chdir(projectFolder)
if projectFolder != os.getcwd():
    print("The cwd is not changed")
    exit

nugetFile = projectFolder+"\\nuget.json"
nuget = getNUGETConfig(nugetFile)

checkVersionFromProj(projectFile,nuget)

#configuration
if nuget["stage"] !="release":
    os.environ["configurationBuild"]= "Debug"
    os.environ["outputDir"]=projectFolder+"\\bin\\Debug"
    os.environ["packageVersion"] = nuget["version"]+"-"+nuget["stage"]+str(nuget["buildVersion"])
    isDebug = True
else:
    os.environ["configurationBuild"]= "Release"
    os.environ["outputDir"]=projectFolder+"\\bin\\Release"
    os.environ["packageVersion"] = nuget["version"]
    isDebug=False

if not pack():
    print("Error build project")
    exit
if not publish():
    print("Error publish to NUGET")
    exit()

writeConfig(nuget)
