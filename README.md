The gallery application to view local photos with the .NET backend, SQlite and Angular.

This is a tiny pet project I'm using to maintain at least some of my skills - like a ToDo app, but more meaningful. It doesn't have good architecture, nor proper tests - I work on these things when I feel like it, after all the *main* purpose of this app is to, you know, spark joy.

Right now I have a bunch of folders with photos in jpg and raw formats. Some of these are stored together, as regular exports from the camera, others are stored in a somehow filtered way - albums from trips etc. I want to have an application that would allow me to browse all my photos, group them in albums (without changing the filesystem), filter by dates (locations maybe?). 

I've created a backend that does the FS scanning and exposes the images and folders data via the regular REST API.
The simple frontend is also ready, it allows to scan new folders and shows already scanned folders and images.

The list of tasks to implement can be found in the [GitHub Project](https://github.com/users/Usurer/projects/6)

The main challenge right now is the raw photos handling. I can't do it in real time, when the frontend requests an image, because it takes too much time. That means that *all* raw photos should be converted during the folder scan and resulting jpegs are to be stored in a cache folder. The obvious downside is the required space. However, I can keep these images in a low-res (like 1280x1024) and load them first, while full-size will be requested and as soon as it's ready it'll replace the 'preview' quality image. 

Another nice to have feature is an Electron wrapper for the frontend, which would give me the FS access (right now I have to copypaste folder paths for scanning), but that's not important right now.

Also, the frontend may be hosted as a static part of the backend app. Whereas the backend should run as a Windows Service. Alternatively, I can run these in Docker, but I have to check if the containerized app will have the host FS access.

Right now though I'm more concerned about the Background job I use to scan folders, I feel kinda uneasy about it.

Plus logs and telemetry - I really want telemetry.