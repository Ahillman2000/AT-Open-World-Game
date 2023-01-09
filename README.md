# AT-Open-World-Game

![image](https://user-images.githubusercontent.com/55785328/161006658-66cbf2fa-aa58-4505-be10-917fe3b08848.png)

# Project Description

A system to stream an open world game level. The system is able to generate terrain from a given 2d heightmap texture and then splits it into world chunks. By calculating the players distance from these chunks, the system loads and unloads sections of the world, scene objects and non-playable characters around the player to give the illusion of a living world.

# Installing / Using the Project
The project is built in Unity 2021.1.21.

This solution can be run in multiple ways, the first method is to generate the map and update the world's objects from within the scene view hierarchy. Both of these tasks can be carried out by clicking on the MeshGenerator object, accessing the mesh generator script and running the two functions within the component menu (you may need to run these functions twice for the system to work).

![image](https://user-images.githubusercontent.com/55785328/161006925-dba67a14-e0bc-4210-9ffa-81ec69f58104.png)

The second method is to run the game with the map being generated at runtime, ready to go.

# Documentation
[Open_World_Game.pdf](https://github.com/Ahillman2000/AT-Open-World-Game/files/10372709/Open_World_Game.pdf)

# References
- Ruskin, E. (2015). Streaming in Sunset Overdrive’s Open World. https://www.gdcvault.com/play/1022268/Streaming-in-Sunset-Overdrive-s
