# Pretty Fly Solutions City Generation Tool
### Website
The [Pretty Fly Solutions](http://prettyflysolutions.infinityfreeapp.com/) website contains information about the product and our team.
### Overview
This tool is designed to be quick and easy, one menu is used to place a number of district objects, once placed in the desired orrientation, you can press the generate button and the L-System algorithm creates the road system and buildings. <br>
You can also generate terrain if needed. They can also edit city and district details such as names and building heights. <br>
You can save the city with a click of a button. The save city button creates a prefab city object. <br>
The tool also provides the option to create and edit L-System rules, for even more customization over the L-System algorithm.
### Package Contents
The package contains a demo scene in the 'Demo' folder at the root of the package. This scene contains a sample city that was generated using the tool. <br>
The editor folder holds the assets neccessary for the tool. 'CityPrefabs' contains any saved city prefabs that users decide to save. <br>
The 'Resources' folder contains all of the neccessary models, prefabs, materials, and L-System rules. In the 'Rules' folder you will find all of the rules for each individual districts. You can create new rules by right clicking and selecting 'Create' -> 'ProceduralCity' -> 'Rule'. You can then assign this new rules by going into the district prefab, into the prefabs child object 'LSystem', and then assigning it to the 'Rules' array in the 'LSystem' script. <br>
The 'Scripts' folder has all the neccessary scripts the tool needs. This is split between generation scripts and zoning scripts. <br>
Finally, the 'Tests' folder contains all the unit tests needed to ensure the tools scripts are working as expected. 
### Installation Instructions
You can install this package using the standard [Package Manager installation instructions](https://docs.unity3d.com/Manual/upm-ui-install.html), there are no special requirements for this package.
### Requirements
 - Package is compatible with Unity Version - 2021.1.17.
 - URP is recommended to be installed.
### Limitations
 - Generating a large amount of districts can take a long amount of time.
### Tutorials
Create District
 - Open the CityBuilderWindow (Tools -> Pretty Fly's City Generator).
 - Select a District Type from the six types available. 
 - Select a District Size.
 - Click 'Spawn District'.
 - Move the district object by clicking and dragging with Unity's 'Move Tool'.

Generate City
 - Create and position your desired number of districts.
 - Click the 'Generate City' button.
   - If you would like terrain generated with the city, check the 'Generate With Terrain' box.

Edit District Details
 - Click on a district object in the edit or hierarchy view.
 - In the district details tab that has now appeared, input your desired district name.
 - If you like to change the building height, select a new height value. 
 - Click the 'Save Details' button. 

Save City
 - Click on a district object in the hierarchy view.
 - In the city details tab that has now appeared, input your desired city name.
 - Click the 'Save Generated City As Prefab' button.
 - Your generated city in now a prefab at 'City Generation Tool/Editor/CityPrefabs'.