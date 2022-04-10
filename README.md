# AT-Open-World-Game

A system to stream an open world game level. The system loads and unloads sections of the world, scene objects and non-playable characters around the player to give the illusion of a living world.

![image](https://user-images.githubusercontent.com/55785328/161006658-66cbf2fa-aa58-4505-be10-917fe3b08848.png)

This solution can be run in multiple ways, the first method is to generate the map and update the world's objects from within the scene view hierarchy. Both of these tasks can be carried out by clicking on the MeshGenerator object, accessing the mesh generator script and running the two functions within the component menu (you may need to run these functions twice for the system to work).

![image](https://user-images.githubusercontent.com/55785328/161006925-dba67a14-e0bc-4210-9ffa-81ec69f58104.png)

The second method is to simply run the game and the world should be generated.
