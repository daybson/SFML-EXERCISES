#include "Game.h"
#include "UIButton.h"

Game::Game()
{
	mWindow.create(VideoMode(screenWidth, screenHeight), "SFML INPUT");
}

Game::Game(String title, Uint32 style)
{
	mWindow.create(VideoMode(screenWidth, screenHeight), title, style);
	buttonExit = new UIButton("buttonNormal.png", "buttonClicked.png", Vector2f(100, 100));
	//if (!texture.loadFromFile("map.png", IntRect(0, 0, 104, 125)))
		//cout << "Error loading file" << endl;

	//texture.setSmooth(true);
	//sprite.move(0, 0);
}

Game::~Game()
{
}

void Game::run()
{
	while (mWindow.isOpen())
	{
		ProcessEvents();
		Update();
		Render();
	}
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
		case Event::KeyPressed:
			if (event.key.code == Keyboard::Escape)
				mWindow.close();
			break;
		case Event::MouseButtonPressed:
			if (event.mouseButton.button == Mouse::Left)
			{
				buttonExit->checkClick(Vector2f(event.mouseButton.x, event.mouseButton.y));
				if (buttonExit->getState())
					cout << "Exiting..." << endl;
			}
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

	//mWindow.draw(sprite);
	mWindow.draw(buttonExit->getDrawButton());

	mWindow.display();
}
