#include <iostream>
#include <ctime>

using namespace std;

int main()
{
	srand(static_cast<unsigned int>(time(0)));

	int secretNumber = rand() % 10 + 1;

	int attempts = 0;
	int guess;


	do
	{
		cout << "GUESS THE NUMBER\n\n" << endl;
		cout << "Enter the guess:";
		cin >> guess;
		attempts++;

		if (guess > secretNumber)
			cout << "\n\nToo high\n";
		else if (guess < secretNumber)
			cout << "\n\nToo low\n";
		else
		{
			cout << "You got it in " << attempts << " attempts!\n" << endl;
			cout << "\t\tGAME OVER\n\n";
		}

		system("pause");
		system("cls");

	} while (guess != secretNumber);

}