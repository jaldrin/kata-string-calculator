docker run -d --restart unless-stopped --name seq -e ACCEPT_EULA=Y -v C:\Demos\Logs:/data -p 8081:80 datalust/seq:latest
