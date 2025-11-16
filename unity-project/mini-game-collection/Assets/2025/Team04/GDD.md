# Game Design Document - Cabinet Game - Team 4

**Team members:** Owen Haedrich, Blair Haines, Becca Pesner, Olwyn Roberts

**Game Title:** Asteroid Alley

## Roles
- **Owen:** lead programmer
- **Blair:** documentation, glue guy, programmer assistant
- **Becca:** documentation, video
- **Olwyn:** level designer

## Concept
Our concept is a competitive 2-player arcade minigame where both players face each other across a shared space. One player stands up right on the bottom platform, while the other stands upside down on the top platform. Each player must collect rockets that spawn in the arena and fire them across the field to hit the opponent.

Floating asteroids drift through the center area, creating shifting cover that players must shoot around or use to block incoming fire. The goal is to hit the opposing player as many times as possible before the match timer ends.

![Concept](https://i.imgur.com/91tTcGb.jpeg)

[Concept Animation]([https://i.imgur.com/MGVQm2J.gif](https://i.imgur.com/91tTcGb.jpeg))

## Core Mechanic
- Collect rocket pickup
- Aim by positioning your character along the platform
- Fire a projectile across the playfield
- Control rocket after launch with mapped keys
- Asteroids block rockets
- Score points by hitting opponents

## Controls

| Input    | Action                          |
|----------|---------------------------------|
| Joystick | Move left or right along the platform |
| Button 1 | Jump                            |
| Button 2 | Shoot if holding a rocket... otherwise can pick up a rocket. |

## Objective Statement
We want to create a short, intense, and readable competitive experience where players feel clever when they land shots and frustrated in a funny way when asteroids block their attacks. The emotional goal is playful tension... Players are constantly reacting to shifting obstacles and trying to line up the perfect shot.

The gameplay goal is to encourage…
- Quick bursts of decision making
- Adaptation to constantly changing cover
- Direct player vs player mindgames

## Design Rationale
- Simple controls allow a player to learn instantly
- Fast rounds encourage rematches
- Clear visual stakes... each shot attempt is dramatic and easy to follow
- Environmental randomness (asteroids) makes every round feel different
- Player interaction is direct... no complex systems to explain

This creates accessible, immediately playable competitive games that work well in a multi-minigame arcade environment.

## Match Structure

| Phase      | Description                  |
|------------|------------------------------|
| Countdown  | 3 seconds to get ready       |
| Live Play  | 60 seconds to score points   |
| End Screen | Display winner               |

## Scoring
+1 point each time a player hits the opponent.
 
Highest score at timer end wins!

## Asteroid Obstacles
- Random spawning along center field
- Moves horizontally up or down
- Varies in size and speed
- Can block rockets or create trick-shot moments
