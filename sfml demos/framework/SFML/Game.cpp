#include "Game.h"

Game::Game(String title, Uint32 style)
{
	window.create(VideoMode(screenWidth, screenHeight), title, style);
	window.setFramerateLimit(0);
	player = new Player("p1");
	clock = new Clock();
}

Game::~Game()
{
}

void Game::run()
{
	while (window.isOpen())
	{
		clock->restart();

		processEvents();
		update();
		render();
	}
}

void Game::processEvents()
{
	Event event;

	while (window.pollEvent(event))
	{
		player->processEvents(event);

		switch (event.type)
		{
		case Event::Closed:
			window.close();
			break;
		}
	}
}

void Game::update()
{
	player->update(clock->getElapsedTime().asSeconds());
}

void Game::render()
{
	window.clear();

	window.draw(player->getRender());

	window.display();
}
