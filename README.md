# Superliminal test

I saw this video of the game Superliminal (https://www.epicgames.com/store/en-US/product/superliminal/home), and I thought the effect was brilliant.
So, I decided to try to implement it for myself, see what it would entail, and these are the results.

Basically, the effect uses the scaling elements of the projection matrix to ensure the object stays the same size in the view, even if we put it closer of further away.
So, when we grab an object, we put the object as a child of the camera, and scale it based on the original distance.
When we release it, we have to use a coroutine to split the collision tests in several frames. 
The reason for this is that Unity apparentely doesn't support swept tests on the rigid body while changing the scale in a single frame, so I have to move the object, scale it, wait a frame, test it, move it a bit more, scale it, etc, etc.
This could be solved if we add some limitations on the existing system, for example, defining that the interactive object has a "master collider" for this, that encompasses the whole object, and we use the Physics.Overlap functions to simulate the sweep test (results would probably be better as well).

Overall, it was a nice challenge, even if it has a lot of rough edges.

## Running

In the test application, open the TestScene, and play it. Normal FPS movement, left click on the blue bottle to grab it, and left click again to place it on the world.

## Using

If you want to try to use the scripts yourself:
- The Grabber script handles picking up and releasing the object
- Object needs to have the Interactive script, and be on the Interactive layer (all hard coded)

## Limitations
- It's relatively slow. 
- Doesn't check for valid drop positions
- Physics can sometimes make the player move back when dropping the object too close


![alt text](https://github.com/DiogoDeAndrade/SuperliminalTest/raw/master/Screenshots/screen01.gif "Test")

## Licenses

All code in this repo is made available through the [GPLv3] license.
The text and all the other files are made available through the 
[CC BY-NC-SA 4.0] license.

## Metadata

* Autor: [Diogo Andrade][]

[Diogo Andrade]:https://github.com/DiogoDeAndrade
[GPLv3]:https://www.gnu.org/licenses/gpl-3.0.en.html
[CC BY-NC-SA 4.0]:https://creativecommons.org/licenses/by-nc-sa/4.0/
