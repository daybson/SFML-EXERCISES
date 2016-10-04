#include "World.h"
#include "Game.h"
#include <math.h>

World::World()
{
	Vector2f origin = Vector2f(0.f, 0.f);

	//sky preenche 1/3 da screen
	sky = *new RectangleShape(Vector2f(Game::screenWidth, Game::screenHeight / 3.f));
	sky.setOrigin(origin);
	sky.setFillColor(Color::Blue);
	sky.setPosition(0, 0);

	street = *new RectangleShape(Vector2f(Game::screenWidth, Game::screenHeight / 2.f));
	street.setFillColor(*new Color(127, 127, 127));
	street.setOrigin(origin);
	street.setPosition(0, 200);
	
	//calcula a hipotenusa correspondente a metade da tela horizontal (ponto de fuga da estrada) com a altura até o sky (1/3 screen)
	float hipo = sqrt((Game::screenWidth / 2 * Game::screenWidth / 2) + (2 * Game::screenHeight / 3 * 2 * Game::screenHeight / 3));

	lineStreetLeft = *new RectangleShape(Vector2f(hipo, 5));
	lineStreetLeft.setFillColor(Color::White);
	lineStreetLeft.setOrigin(origin);
	lineStreetLeft.setPosition(0, Game::screenHeight);
	lineStreetLeft.rotate(-45);

	lineStreetRight = *new RectangleShape(Vector2f(hipo, 5));
	lineStreetRight.setFillColor(Color::White);
	lineStreetRight.setOrigin(origin);
	lineStreetRight.setPosition(Game::screenWidth, Game::screenHeight);
	lineStreetRight.rotate(-135);

	forestLeft.setPointCount(3);
	forestLeft.setFillColor(Color::Green);
	forestLeft.setPoint(0, Vector2f(Game::screenWidth / 2, Game::screenHeight / 3));
	forestLeft.setPoint(1, Vector2f(0, Game::screenHeight / 3));
	forestLeft.setPoint(2, Vector2f(0, Game::screenHeight));

	forestRight.setPointCount(3);
	forestRight.setFillColor(Color::Green);
	forestRight.setPoint(0, Vector2f(Game::screenWidth / 2, Game::screenHeight / 3));
	forestRight.setPoint(1, Vector2f(Game::screenWidth, Game::screenHeight / 3));
	forestRight.setPoint(2, Vector2f(Game::screenWidth, Game::screenHeight));
}


World::~World()
{
}
