#include "Game.h"

Game::Game()
{
	mWindow.create(VideoMode(screenWidth, screenHeight), "SFML");
	mIsMovingDown = mIsMovingLeft = mIsMovingRight = mIsMovingUp = false;
}

Game::Game(String title, Uint32 style)
{
	mWindow.create(VideoMode(screenWidth, screenHeight), title, style);
	world = *new World();
	player = *new Player();
	player.Move(Vector2f(Game::screenWidth/2, Game::screenHeight));
}

Game::~Game()
{
	free(&world);
	free(&player);
}

void Game::run()
{
	while (mWindow.isOpen())
	{
		processEvents();
		update();
		render();
	}
}

void Game::handlePlayerInput(Keyboard::Key key, bool isPressed)
{
	switch (key)
	{
	case sf::Keyboard::A:
		mIsMovingLeft = isPressed;
		break;
	case sf::Keyboard::D:
		mIsMovingRight = isPressed;
		break;
	case sf::Keyboard::W:
		mIsMovingUp = isPressed;
		break;
	case sf::Keyboard::S:
		mIsMovingDown = isPressed;
		break;
	default:
		break;
	}
}

void Game::processEvents()
{
	Event event;

	while (mWindow.pollEvent(event))
	{
		switch (event.type)
		{
			case Event::KeyPressed:
				handlePlayerInput(event.key.code, true);
				break;
			case Event::KeyReleased:
				handlePlayerInput(event.key.code, false);
				break;
			case Event::Closed:
				mWindow.close();
				break;
			default:
				break;
		}
	}
}

void Game::update()
{
	Vector2f movement(0.f, 0.f);

	if (mIsMovingUp)
		movement.y -= 0.1f;
	if (mIsMovingDown)
		movement.y += 0.1f;
	if (mIsMovingLeft)
		movement.x -= 0.1f;
	if (mIsMovingRight)
		movement.x += 0.1f;

	if (movement != Vector2f(0.f, 0.f))
		player.Move(movement);
}

void Game::render()
{
	mWindow.clear();

	mWindow.draw(world.sky);
	mWindow.draw(world.street);
	mWindow.draw(world.forestLeft);
	mWindow.draw(world.forestRight);
	mWindow.draw(world.lineStreetLeft);
	mWindow.draw(world.lineStreetRight);

	mWindow.draw(player.body);
	mWindow.draw(player.head);
	mWindow.draw(player.legLeft);
	mWindow.draw(player.legRight);
	mWindow.draw(player.armLeft);
	mWindow.draw(player.armRight);
	mWindow.draw(player.hat);
	mWindow.draw(player.nose);

	mWindow.display();
}
