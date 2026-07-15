```
docker run --detach --interactive --tty --name jtenosgo1 --publish 8080:8080 --mount type=bind,source="C:\projects",target=/projects golang
```

Creates container named jtenosgo1 from the golang image, forwards local port 8080 to remote port 8080, mounts C:\projects into /projects

Can access http://localhost:8080 in Windows and see what's running in the container

From the container, host.docker.internal will get you to the host machine, like http://host.docker.internal:8081 will get you whatever is running in windows on port 8081
