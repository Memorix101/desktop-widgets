// NyanCat_Pointer_CPP.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <SDL.h>
#include <stdio.h>
#include <math.h>       /* cos */
#include <SDL_syswm.h>
#include <windows.h>
#include <SDL_image.h>
#include <SDL_mixer.h>
#include <winuser.h>
#include <SDL_ttf.h>

// Makes a window transparent by setting a transparency color.
bool MakeWindowTransparent(SDL_Window* window, COLORREF colorKey) {
	// Get window handle (https://stackoverflow.com/a/24118145/3357935)
	SDL_SysWMinfo wmInfo;
	SDL_VERSION(&wmInfo.version);  // Initialize wmInfo
	SDL_GetWindowWMInfo(window, &wmInfo);
	HWND hWnd = wmInfo.info.win.window;

	// Change window type to layered (https://stackoverflow.com/a/3970218/3357935)
	SetWindowLong(hWnd, GWL_EXSTYLE, GetWindowLong(hWnd, GWL_EXSTYLE) | WS_EX_LAYERED);

	// Set transparency color
	return SetLayeredWindowAttributes(hWnd, colorKey, 0, LWA_COLORKEY);
}

int main(int argc, char* argv[]) {

	int keys;

	float x = 100;
	float y = 100;

	float lastTick = 0.0f;
	float deltaTime = 0.0f;

	float speed = 10.0f;
	float rot = 0;
	float rot1 = 0;
	float rot2 = 0;
	float rot3 = 0;

	//Screen dimension constants
	const int SCREEN_WIDTH = 640;
	const int SCREEN_HEIGHT = 480;

	SDL_Window* windows_body = NULL;
	SDL_Window* window_head = NULL;
	SDL_Window* window_tail = NULL;

	SDL_Window* window_feet0 = NULL;
	SDL_Window* window_feet1 = NULL;
	SDL_Window* window_feet2 = NULL;
	SDL_Window* window_feet3 = NULL;

	SDL_Window* window_rainbow0 = NULL;
	SDL_Window* window_rainbow1 = NULL;
	SDL_Window* window_rainbow2 = NULL;
	SDL_Window* window_rainbow3 = NULL;

	SDL_Window* window_star0 = NULL;
	SDL_Window* window_star1 = NULL;
	SDL_Window* window_star2 = NULL;
	SDL_Window* window_star3 = NULL;

	//The surface contained by the window
	SDL_Surface* batch_body = NULL;
	SDL_Surface* batch_head = NULL;
	SDL_Surface* batch_tail = NULL;

	SDL_Surface* batch_feet0 = NULL;
	SDL_Surface* batch_feet1 = NULL;
	SDL_Surface* batch_feet2 = NULL;
	SDL_Surface* batch_feet3 = NULL;

	SDL_Surface* batch_rainbow0 = NULL;
	SDL_Surface* batch_rainbow1 = NULL;
	SDL_Surface* batch_rainbow2 = NULL;
	SDL_Surface* batch_rainbow3 = NULL;

	//The image we will load and show on the screen
	SDL_Surface* gfx_head = NULL;
	SDL_Surface* gfx_tail = NULL;
	SDL_Surface* gfx_body = NULL;
	SDL_Surface* gfx_feet = NULL;
	SDL_Surface* gfx_rainbow = NULL;

	//Event handler
	SDL_Event e;

	//Music
	Mix_Music* music = NULL;

	//Sound
	Mix_Chunk* snd_powerup = NULL;

	//Initialization flag
	bool success = true;

	//Initialize SDL
	if (SDL_Init(SDL_INIT_EVERYTHING) < 0)
	{
		printf("SDL could not initialize! SDL_Error: %s\n", SDL_GetError());
		success = false;
	}

	//Initialize SDL_mixer
	if (Mix_OpenAudio(22050, MIX_DEFAULT_FORMAT, 2, 4096) == -1)
	{
		printf("SDL could not initialize mixer! SDL_Error: %s\n", SDL_GetError());
	}

	//Create window
	window_rainbow0 = SDL_CreateWindow("window_rainbow0", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 84, 149, SDL_WINDOW_SHOWN | SDL_WINDOW_BORDERLESS | SDL_WINDOW_SKIP_TASKBAR);
	window_rainbow1 = SDL_CreateWindow("window_rainbow1", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 84, 149, SDL_WINDOW_SHOWN | SDL_WINDOW_BORDERLESS | SDL_WINDOW_SKIP_TASKBAR);
	window_rainbow2 = SDL_CreateWindow("window_rainbow2", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 84, 149, SDL_WINDOW_SHOWN | SDL_WINDOW_BORDERLESS | SDL_WINDOW_SKIP_TASKBAR);
	window_rainbow3 = SDL_CreateWindow("window_rainbow3", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 84, 149, SDL_WINDOW_SHOWN | SDL_WINDOW_BORDERLESS | SDL_WINDOW_SKIP_TASKBAR);

	window_feet0 = SDL_CreateWindow("window_feet0", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 35, 35, SDL_WINDOW_SHOWN | SDL_WINDOW_BORDERLESS | SDL_WINDOW_SKIP_TASKBAR);
	window_feet2 = SDL_CreateWindow("window_feet2", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 35, 35, SDL_WINDOW_SHOWN | SDL_WINDOW_BORDERLESS | SDL_WINDOW_SKIP_TASKBAR);

	window_tail = SDL_CreateWindow("window_tail", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 50, 50, SDL_WINDOW_SHOWN | SDL_WINDOW_BORDERLESS | SDL_WINDOW_SKIP_TASKBAR);
	windows_body = SDL_CreateWindow("windows_body", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 192, 150, SDL_WINDOW_SHOWN | SDL_WINDOW_BORDERLESS | SDL_WINDOW_SKIP_TASKBAR);
	window_head = SDL_CreateWindow("window_head", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 134, 109, SDL_WINDOW_SHOWN | SDL_WINDOW_BORDERLESS | SDL_WINDOW_SKIP_TASKBAR);

	window_feet1 = SDL_CreateWindow("window_feet1", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 35, 35, SDL_WINDOW_SHOWN | SDL_WINDOW_BORDERLESS | SDL_WINDOW_SKIP_TASKBAR);
	window_feet3 = SDL_CreateWindow("window_feet3", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 35, 35, SDL_WINDOW_SHOWN | SDL_WINDOW_BORDERLESS | SDL_WINDOW_SKIP_TASKBAR);

	//window_star0 = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 50, 50, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS);
	//window_star1 = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 50, 50, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS);
	//window_star2 = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 50, 50, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS);
	//window_star3 = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 50, 50, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS);

	//Get window surface
	batch_body = SDL_GetWindowSurface(windows_body);
	batch_tail = SDL_GetWindowSurface(window_tail);
	batch_head = SDL_GetWindowSurface(window_head);

	batch_feet0 = SDL_GetWindowSurface(window_feet0);
	batch_feet1 = SDL_GetWindowSurface(window_feet1);
	batch_feet2 = SDL_GetWindowSurface(window_feet2);
	batch_feet3 = SDL_GetWindowSurface(window_feet3);

	batch_rainbow1 = SDL_GetWindowSurface(window_rainbow1);
	batch_rainbow2 = SDL_GetWindowSurface(window_rainbow2);
	batch_rainbow3 = SDL_GetWindowSurface(window_rainbow3);

	batch_rainbow0 = SDL_GetWindowSurface(window_rainbow0);
	batch_rainbow1 = SDL_GetWindowSurface(window_rainbow1);
	batch_rainbow2 = SDL_GetWindowSurface(window_rainbow2);
	batch_rainbow3 = SDL_GetWindowSurface(window_rainbow3);

	/*SDL_Surface* s;
	s = SDL_CreateRGBSurface(0, batch_head->w, batch_head->h, 32, 0, 0, 0, 0);
	SDL_FillRect(s, NULL, SDL_MapRGB(s->format, 255, 0, 255));*/

	bool quit = false;
	TTF_Init();

	//string t = "nalacat.jpg";
	gfx_head = IMG_Load("head.png");
	gfx_body = IMG_Load("body.png");
	gfx_feet = IMG_Load("feet.png");
	gfx_tail = IMG_Load("tail.png");
	gfx_rainbow = IMG_Load("rainbow.jpg");

	//blueGuy = SDL_image.IMG_Load("BlueGuy.png");

	//IntPtr Sans = SDL_ttf.TTF_OpenFont("TilWeb.ttf", 24);
	//SDL_Color c = { 0, 119, 255 };
	//IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(Sans, "put your text here", c);

	SDL_Surface sur;
	//sur = (SDL.SDL_Surface)Marshal.PtrToStructure(surfaceMessage, typeof(SDL.SDL_Surface));

	//Message_rect.x = SCREEN_WIDTH / 2 - sur.w / 2;  //controls the rect's x coordinate 
	//Message_rect.y = SCREEN_HEIGHT / 2 - sur.h / 2; // controls the rect's y coordinte

	music = Mix_LoadMUS("nyan.ogg");
	Mix_VolumeMusic(10);
	//snd_powerup = SDL_mixer.Mix_LoadWAV("powerup_get.wav");

	//Play the music
	if (Mix_PlayMusic(music, -1) == -1)
	{
		//Console.WriteLine("Unable to play music");
	}

	// Add window transparency (Magenta will be see-through)
	MakeWindowTransparent(windows_body, RGB(0, 0, 0));
	MakeWindowTransparent(window_head, RGB(0, 0, 0));
	MakeWindowTransparent(window_tail, RGB(0, 0, 0));
	MakeWindowTransparent(window_feet0, RGB(0, 0, 0));
	MakeWindowTransparent(window_feet1, RGB(0, 0, 0));
	MakeWindowTransparent(window_feet2, RGB(0, 0, 0));
	MakeWindowTransparent(window_feet3, RGB(0, 0, 0));

	while (!quit)
	{
		deltaTime = (SDL_GetTicks() - lastTick) / 1000;

		//Handle events on queue
		while (SDL_PollEvent(&e) != 0)
		{
			/* if (e.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
				 SDL.SDL_GetGlobalMouseState(out x, out y);*/

				 //User requests quit
			if (e.type == SDL_QUIT)
			{
				quit = true;
			}
		}

		const Uint8* currentKeyStates = SDL_GetKeyboardState(NULL);


		if (currentKeyStates[SDL_SCANCODE_ESCAPE])
		{
			//Free resources and close SDL
			quit = true;
		}


		/* if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_SPACE)
		 {
			 //Play the high hit effect
			 SDL_mixer.Mix_PlayChannel(-1, snd_powerup, 0);
		 }*/

		rot += speed * deltaTime; //calculate angle in relatime * speed
		rot1 += speed * 1.25f * deltaTime;
		rot2 += speed * 1.5f * deltaTime;
		rot3 += speed * 1.75f * deltaTime;
		double _cos = 10.0f * cos(rot); //sinus from your maths class
		double _cos1 = 10.0f * cos(rot1); //sinus from your maths class
		double _cos2 = 10.0f * cos(rot2); //sinus from your maths class
		double _cos3 = 10.0f * cos(rot3); //sinus from your maths classs

		//x += 100 * deltaTime; //100 pixels in a second
		POINT p;
		GetCursorPos(&p);
		int vX = 100 + ((int)x - p.x) / 5;
		int vY = ((int)y - p.y) / 5;
		x -= vX;
		y -= vY;

		SDL_SetWindowPosition(windows_body, (int)x + 200, (int)y + (int)_cos1);
		SDL_SetWindowPosition(window_head, (int)x + 325, (int)y + (int)_cos + 15);
		SDL_SetWindowPosition(window_tail, (int)x + 160, (int)y + 50 + (int)_cos + 15);

		SDL_SetWindowPosition(window_feet0, (int)x + 350, (int)y + 120 + (int)_cos2 + 15);
		SDL_SetWindowPosition(window_feet1, (int)x + 310, (int)y + 120 + (int)_cos3 + 15);
		SDL_SetWindowPosition(window_feet2, (int)x + 200, (int)y + 120 + (int)_cos3 + 15);
		SDL_SetWindowPosition(window_feet3, (int)x + 240, (int)y + 120 + (int)_cos2 + 15);

		SDL_SetWindowPosition(window_rainbow0, (int)x + 150, (int)y + (int)_cos2 + 15);
		SDL_SetWindowPosition(window_rainbow1, (int)x + 100, (int)y + (int)_cos3 + 15);
		SDL_SetWindowPosition(window_rainbow2, (int)x + 50, (int)y + (int)_cos3 + 15);
		SDL_SetWindowPosition(window_rainbow3, (int)x, (int)y + (int)_cos2 + 15);

		//SDL_SetWindowPosition(window_star0, (int)star_0.x, (int)y + (int)_cos2 + 15);

		//Apply the image
		SDL_BlitSurface(gfx_head, NULL, batch_head, NULL);
		SDL_BlitSurface(gfx_body, NULL, batch_body, NULL);
		SDL_BlitSurface(gfx_tail, NULL, batch_tail, NULL);

		SDL_BlitSurface(gfx_feet, NULL, batch_feet0, NULL);
		SDL_BlitSurface(gfx_feet, NULL, batch_feet1, NULL);
		SDL_BlitSurface(gfx_feet, NULL, batch_feet2, NULL);
		SDL_BlitSurface(gfx_feet, NULL, batch_feet3, NULL);

		SDL_BlitSurface(gfx_rainbow, NULL, batch_rainbow0, NULL);
		SDL_BlitSurface(gfx_rainbow, NULL, batch_rainbow1, NULL);
		SDL_BlitSurface(gfx_rainbow, NULL, batch_rainbow2, NULL);
		SDL_BlitSurface(gfx_rainbow, NULL, batch_rainbow3, NULL);

		//Update the surface
		SDL_UpdateWindowSurface(windows_body);
		SDL_UpdateWindowSurface(window_head);
		SDL_UpdateWindowSurface(window_tail);

		SDL_UpdateWindowSurface(window_feet0);
		SDL_UpdateWindowSurface(window_feet1);
		SDL_UpdateWindowSurface(window_feet2);
		SDL_UpdateWindowSurface(window_feet3);

		SDL_UpdateWindowSurface(window_rainbow0);
		SDL_UpdateWindowSurface(window_rainbow1);
		SDL_UpdateWindowSurface(window_rainbow2);
		SDL_UpdateWindowSurface(window_rainbow3);

		SDL_UpdateWindowSurface(window_star0);

		lastTick = (int)(SDL_GetTicks());
		SDL_Delay(30);

		//Wait two seconds
		//SDL.SDL_Delay(2000);
	}

	// Clean up
	SDL_Quit();
	return 0;
}
