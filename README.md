# TalkingCoachEPVA34
This repository contains the combined work of EPVA3 and EPVA4's context projects for the TUDelft. 
The product is an extension of the original TalkingCoach project (https://github.com/ruuddejong/TalkingCoach).

## EPVA3 
EPVA3 has added body animation based on emotion.

### EPVA3 members:
- Mark Acda
- Pieter Tolsma
- Thomas Boss
- Jeroen Nelen
- Toon de Boer

## EPVA4
EPVA4 has added mouth animation based on speech.

### EPVA4 members:
- Emma Sala
- Erik Mekkes
- Joshua Slik
- Lucile Nikkels
- Muhammed Imran Özyar


As this repository is a merge effort / mirror of two separate repositories from GitLab the full git history is not available here.

## Working with the product
### Unity base
The base code of the product can be found as /InteractiveAvatar as a project to import and work on with Unity. We suggest using Rider for working on the included code. Rider can automatically link code changes with Unity. Note that Unity should be set to use WebGL as intended platform for building the project.
### Phoneme Server
The implemented nodejs express phoneme server used for transcribing text to phonemes can be found as /website/backend
### React Web App
Example code for a stylish react web application that uses the product can be found as /website/client
### Simple Web page
Example code for a simple web page that uses the product with demo functions can be found as /InteractiveAvatar/Assets/WebGLTemplates/Interactive. Unity uses this as a template when Building the project, filling in the marked sections to produce a web page such as the one included as /website/client/unity.

## Live demonstration pages
A live version of the react web application can be viewed at : http://talkingcoach.ewi.tudelft.nl/

A live version of the simple web page with demo functions can be viewed at : http://ii.tudelft.nl/epva34/


The phoneme server can be accessed at http://talkingcoach.ewi.tudelft.nl/api/.

For example with http://talkingcoach.ewi.tudelft.nl/api/phoneme?text=Hello%20World&lang=en-US


## Server Setup
A zip file with all the required components to set up a server for this product has been included as talkingcoach_server_setup.zip.

It includes a readme that describes the general structure / installation of the server, and a full install guide with included installation script for linux.

## Large File Support
This repository makes use of git Large File Storage due to GitHub's 100MB file size limit.

currently the only file tracked by git lfs is InteractiveAvatar/Assets/Animations/walk_into_screen.anim at 134MB
recommended to avoid git lfs as much as possible as it is very slow.

for information on how to use git lfs:
see https://git-lfs.github.com/
and https://github.com/git-lfs/git-lfs/wiki/Tutorial#migrating-existing-repository-data-to-lfs

the original repo was converted to lfs using the following tool : https://github.com/bozaro/git-lfs-migrate
