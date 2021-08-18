#pragma once
#include "stdafx.h"
#include <iostream>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <string>
#include <windows.h>
#include <iostream>
#include "nsiBroadcaster.h"

using namespace std;

string hostName;
int groupNum;

bool stringFind(string a_Source, string a_Target)
{
	if (a_Source.find(a_Target) != std::string::npos) {
		return true;
	}
	else
	{
		return false;
	}
}

int getGroupID(int argc, char *argv[])
{
	int answer;
	char message[256];
	char input;
	bool validID = false;
	bool consoleInput = false;

	if (argc > 1) consoleInput = true;
	printf("--------------\nPlease enter the GroupID you want to monitor.\n--------------\n");

	while (!validID)

	{

		if (consoleInput)
		{
			input = *argv[1];

			if (isdigit(*argv[1]) != 0)
			{
				answer = atoi(argv[1]);
				validID = true;

			}
			else if (input == 'n')
			{
				answer = 0;
				validID = true;
			}
			else
			{
				consoleInput = false;
				printf("Invalid Input. Please enter the number for the GroupID you wish to use.\n");
			}

		}
		else
		{
			std::cin.getline(message, 256);

			if (strlen(message) == 0)
			{
				answer = 0;
				validID = true;
			}
			else if (isdigit(*message) != 0)
			{
				answer = atoi(message);
				validID = true;
			}

			if (validID != true) printf("Invalid Input. Please enter the number for the GroupID you wish to use.\n");
		}
	}

	return answer;
}

int main(int argc, char *argv[])
{
	bool m_Running = true;
	string m_UserInput;

	hostName = "10.1.56.200";
	groupNum = getGroupID(argc, argv);

	printf("Listening on GroupID: %d\n", groupNum);

	nsiBroadcaster *broadcaster = new nsiBroadcaster(groupNum);
	



	Sleep(2000);
	printf("VIBE2 Command Console is ready for input:\n");

	while (m_Running)

	{
		std::getline(std::cin, m_UserInput);

		broadcaster->broadcastVIBECommand(m_UserInput);

		//broadcaster->broadcast(m_UserInput);
		//Sleep(10);
	}

	return 0;
}