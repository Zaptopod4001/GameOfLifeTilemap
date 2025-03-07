# Game of Life with Unity Tilemap (Unity / C#)

## What is it?

An example how to create Conway's "Game of Life" in Unity using Tilemaps.

![Game of life image 1](/doc/game_of_life_0.png)

# Features
* Resizeable map with automatic camera framing
* Draw and erase cells with mouse l/r buttons
* Simulate with set framerate
* Simulate step by step
* Clear map

# How to use
* Open Project
* Press Play in Editor
* Game starts in draw mode, draw some pattern with mouse
* Press F2 to enter play mode or
* Press F3 to simulate one step at time
* Press F5 to clear field and enter draw mode

Note: Frame counter only resets when F2 / play is pressed.


# Examples

Some patterns created by game of life algorithm.

![Game of life image 2](/doc/game_of_life_1.gif)

![Game of life image 3](/doc/game_of_life_2.gif)


# Classes

## GameOfLife.cs
Class containing all the functionality of Game of Life algorithm and tilemap rendering.

## GameOfLifeUI.cs
Takes care of UI view rendering.


# About
I created this Game of Life implementation to learn how to use Unity Tilemaps a while back.

# Copyright
Code and algorithm implementation by Sami S. use of any kind without a written permission from the author is not allowed. But feel free to take a look.
