#include "Game.h"

Game::Game()
{
	mWindow.create(VideoMode(screenWidth, screenHeight), "SFML TILEMAP");
}

Game::Game(String title, Uint32 style)
{
	mWindow.create(VideoMode(screenWidth, screenHeight), title, style);
	level = new int[128]
	{
		0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 2, 0, 0, 0, 0,
		1, 1, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3,
		0, 1, 0, 0, 2, 0, 3, 3, 3, 0, 1, 1, 1, 0, 0, 0,
		0, 1, 1, 0, 3, 3, 3, 0, 0, 0, 1, 1, 1, 2, 0, 0,
		0, 0, 1, 0, 3, 0, 2, 2, 0, 0, 1, 1, 1, 1, 2, 0,
		2, 0, 1, 0, 3, 0, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1,
		0, 0, 1, 0, 3, 2, 2, 2, 0, 0, 0, 0, 1, 1, 1, 1
	};

	if (!map.Load("tilemap.png", Vector2u(32, 32), level, 16, 8))
		throw "Error loading tilemap";

	if(!background.Load("background.png", 512, 256))
		throw "Error loading background";

}

Game::~Game()
{
}

void Game::run()
{
	while (mWindow.isOpen())
	{
		Update();
		Render();
	}
}

Tile & Game::GetTileMap()
{
	return map;
}

Tile & Game::GetBackground()
{
	return background;
}

void Game::ProcessEvents()
{
	Event event;

	while (mWindow.pollEvent(event))
	{
		switch (event.type)
		{
		case Event::Closed:
			mWindow.close();
			break;
		default:
			break;
		}
	}
}

void Game::Update()
{
}

void Game::Render()
{
	mWindow.clear();

	mWindow.draw(background);
	mWindow.draw(map);

	mWindow.display();
}
