## Overview

This project demonstrates a flexible and organized approach to NPC spawning, utilizing Unity, Zenject for dependency injection, signal handling, and addressables.

## Thinking Process

The usage of dependency injection through Zenject ensures clean separation of concerns, signal handling allows different components to communicate effectively, and the addressables provide a better manage of memory. The code snippets were designed to be reusable and extensible.

### `SpawnerNPCController.cs`

This class manages the spawning of NPCs based on the provided `NPCType`. It extends the `BaseSpawner<T, T2>` class to encapsulate common spawning logic. The `StartParticle` method initializes NPC spawning at a specific position, and `GetInactiveObject` method checks for available NPCs to spawn.

### `NPCController.cs`

This class represents individual NPCs in the game world. It uses Unity's NavMeshAgent for movement and interacts with other NPCs and particle effects upon collision.

### `ParticleHandler.cs`

The `ParticleHandler` class controls the activation and deactivation of particle effects. It starts a particle effect at a specified position and deactivates it after a defined duration.

### `OnSpawnerRateTimeChangeSignal.cs`

This class represents a signal that triggers when the player changes the value of a HUD slider. It carries information about the new rate time value.

### `SpawnerPoint.cs`

This class represents a point in the game world that spawns NPCs at regular intervals. It subscribes to the rate time change signal from the HUD and uses a coroutine to control NPC spawning.

### `Hud.cs`

The `Hud` class represents the HUD in the game. It uses Zenject to inject dependencies and provides UI elements for the player to interact with. The `UpdateSpawnerRateTime` method updates the spawning rate time and notifies other components through a signal.

## Conclusion

The project achieves modularity and scalability, facilitating code maintenance and further development.

## Memory Report	

NPC Spawn Limit
NPCs adhere to preset spawn limits, controlling memory usage and preventing excessive NPCs.

Memory Usage
The NPC spawning system efficiently manages memory allocation and deallocation, maintaining stable memory consumption.

Spawn Interval and Performance
Spawn intervals ≥ 0.3s: No notable performance impact.
Spawn intervals < 0.3s: FPS drop and potential freezing due to rapid spawning.

