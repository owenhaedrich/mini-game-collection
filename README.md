# Mini Game Collection
A mini game project designed to teach students intermediate GitHub workflow and digital game prototyping skills.

# DevCon #2 How To
## Initial Setup: Fork
Teams will need to create a fork of the repository under one team member's account. That team member should then add all team members as collaborators to the forked repository. The team will then be able to edit the repository.

## Development
It is expected that teams will review the [Wiki](https://github.com/MohawkRaphaelT/mini-game-collection/wiki) for information about the project configuration and workflow restrictions.

## Contribute: Pull Request
Ensure the mini game has been created using the guidelines in the [Wiki](https://github.com/MohawkRaphaelT/mini-game-collection/wiki).

Once ready, one team member can create a Pull Request to ask that the repository owner pull their changes in. When creating a repository, please target the `Games20XX` branch rather than `main`.

# Automated WebGL Builds ![Build and Deploy WebGL workflow badge.](https://github.com/MohawkRaphaelT/mini-game-collection/actions/workflows/ci.yml/badge.svg)
GitHub Pages are enabled for this project. Successful builds on `main` will be pushed to the site and [playable here](https://mohawkraphaelt.github.io/mini-game-collection/).

If you are interested to know more about how this automation works, consider reviewing [.github/workflows/ci.yml](https://github.com/MohawkRaphaelT/mini-game-collection/blob/main/.github/workflows/ci.yml)'s source and comments. In brief, a GitHub Action is run that starts up a Linux virtual machine with Unity on it, pulls down the repository, creates a WebGL build, then pushes the artifacts (build) to GitHub Pages.

