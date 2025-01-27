[Play the game](https://eliaxfox.itch.io/my-little-workshop)

My dream workshop is a 3D top-down crafting game where the player have to craft items in order to gain money and expand his shop.

It is developped in the context of the [Revival Jam 2024](https://itch.io/jam/revival-jam-2024)

The main challenge of this project was to make many systems interact between them, some of them being:
- **A player interaction system**, allowing to interact with any object with the IInteractable class implemented
- **An inventory system** to store the player assets
- **Station system** capable of handling items and reacting to interaction from the player. Each station override a base and is able to have custom behavior
- **Customer system** AI-Driven customer are coming to the shop and buying stuff
- **Player store and upgrade system** allowing the player to buy resources and unlock new recipes and skills
- **Gathering system** allowing the playerto collect resources from the world, with regeneration over time
- **Tutorial system** to explain the main game loop, with step completion detection tied to other systems
