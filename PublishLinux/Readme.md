# The tools for Publish ASP.NET Core project to ubuntu with nginx host
For success publish project to ubuntu, your appsettings must followed this rules:

1. contains section NGINXPublish in root
2. installed Bitvise ssh app
3. Section NGINXPublish must contains filled properties:
	a) BitviseTlpFile - path to config ssh config connection with stored password;
	b) HostName - host name your published application
	c) LinuxHome - path home folder in linux as /home/azureuser
	d) LinuxWebFolder - path to web folder as /var/www
4. Optional properties:
    a) TempFolder - temp folder for build, default user temp folder
	b) Section "Service" if started your project as backend
