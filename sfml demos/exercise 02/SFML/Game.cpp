#include "Game.h"

Game::Game()
{
	mWindow.create(VideoMode(screenWidth, screenHeight), "SFML");
	mIsMovingDown = mIsMovingLeft = mIsMovingRight = mIsMovingUp = false;
	clock = *new Clock();
}

Game::Game(String title, Uint32 style)
{
	clock = *new Clock();
	mWindow.create(VideoMode(screenWidth, screenHeight), title, style);
	player = new Player();
	//player->Move(Vector2f(400, 600));
}

Game::~Game()
{
}

void Game::run()
{
	while (mWindow.isOpen())
	{
		clock.restart();

		ProcessEvents();
		Update();
		Render();
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

void Game::ProcessEvents()
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

void Game::Update()
{
	Vector2f delta(0, 0);

	if (mIsMovingUp)
		delta.y -= 0.1f;
	if (mIsMovingDown)
		delta.y += 0.1f;
	if (mIsMovingLeft)
		delta.x -= 0.1f;
	if (mIsMovingRight)
		delta.x += 0.1f;

	if (delta != Vector2f(0, 0))
		player->Move(delta);

	player->Update(clock.getElapsedTime().asSeconds());
}

void Game::Render()
{
	mWindow.clear();

	mWindow.draw(player->GetCurrentSprite());

	mWindow.display();
}
