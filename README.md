# Scene Zones
## What does this do?
A system to break down levels into chunks and increase performance. This is based on the Marrow SDK by Stress Level Zero.

## Dependencies
This package depends in some places on the Ult Events(https://kybernetik.com.au/ultevents/) package by Kybernetik

## How do I use this?
Simply download the UnityPackage file (Or clone the repository) and the scripts should be in your project after dragging it into your assets. After that you can go into your scene, right click in the hierarchy and there's a new tab called "SDK" inside of this category are all of the Zone components (Currently only the base scene zone). Simply click the "Scene Zone" option.

Here are the components on the object:
![Scene Zone Components](/images/ZoneComponent.JPG)

You might notice that you have to manually set the trigger layers and tags. What Stress Level Zero do to counter this is just to create their own tag system. I didn't do this for simplicity sake.

Then you can also notice the Zone Link Component.

## Zone Links
A zone link is a way to make the gameplay exactly the same and prevent unexpected behaviour. You should link zones that are in line of sight or close proximity to eachother. Due to this system doing automatic physics culling and the Scene Chunking feature this Zone Link Component is necessary.
![Zone Link Components](/images/ZoneLinks.JPG)

## Scene Chunks
Scene chunks are a system I developed (I wasn't the first) to cull areas that aren't in close proximity or line of sight of the player. The difference between this and occlusion culling are that this culls physics and is dependant on premade zones, rather than automatically created chunks.

## How do I use scene chunks?
This is a very complicated system that uses the Scene asset system. First step is to separate your scene into multiple scenes. The way to think of it is:
![Workflow](/images/Workflow.JPG)

## How to create the mentioned Stream Chunk Asset and Setup your Scene chunk
Firstly create your Stream chunk Asset by right clicking in your assets folder and following the path "Add\Scriptable Objects\Stream Chunk"
![Path](/images/ContextMenu.JPG)

Drag your Chunk scene and any scenes in the line of sight of that zone into the list of scenes.
![Stream Chunk](/images/StreamChunk.JPG)

Then Drag your stream chunk into the Scene Chunk field.
![Scene Chunk](/images/SceneChunk.JPG)