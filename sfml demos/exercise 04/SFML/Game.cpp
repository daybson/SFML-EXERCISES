#include "Game.h"
#include "UIButton.h"

Game::Game()
{
	window.create(VideoMode(screenWidth, screenHeight), "SFML INPUT");
}

Game::Game(String title, Uint32 style)
{
	window.create(VideoMode(screenWidth, screenHeight), title, style);
	buttonExit = new UIButton("buttonNormal.png", "buttonClicked.png", Vector2f(100, 100));
	gamepad = new Gamepad(0);
	square.setFillColor(Color::Cyan);
	square.setSize(Vector2f(20, 20));
}

Game::~Game()
{
}

void Game::run()
{
	while (window.isOpen())
	{
		processEvents();
		Update();
		Render();
	}
}

void Game::processEvents()
{
	Event event;

	bool move;

	while (window.pollEvent(event))
	{
		switch (event.type)
		{
		case Event::Closed:
			window.close();
			break;
		case Event::KeyPressed:
			if (event.key.code == Keyboard::Escape)
				window.close();
			break;
		case Event::MouseButtonPressed:
			if (event.mouseButton.button == Mouse::Left)
			{
				buttonExit->checkClick(Vector2f(event.mouseButton.x, event.mouseButton.y));
				if (buttonExit->getState())
					cout << "Exiting..." << endl;
			}
			break;
		case Event::JoystickMoved:
			move = true;
			speed = Vector2f(gamepad->getAxisPosition(gamepad->index, gamepad->X), gamepad->getAxisPosition(gamepad->index, gamepad->Y));
			break;
		default:
			move = false;
			break;
		}
	}
}

void Game::Update()
{
	if (speed.x > gamepad->getDeadZone() || speed.x < -gamepad->getDeadZone() || speed.y > gamepad->getDeadZone() || speed.y < -gamepad->getDeadZone())
		square.move(speed.x, speed.y);
}

void Game::Render()
{
	window.clear();

	window.draw(square);
	window.draw(buttonExit->getDrawButton());

	window.display();
}
