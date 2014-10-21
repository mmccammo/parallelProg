parallelProg
============

Examining the performance of the multi-threading library OpenMP, split over multiple cores


/* readme.txt
   Matt McCammon
   August 26th, 2013
	
   */

////////////////

Readme

Main Program:

	The program "partcles.c" randomly generates a number of particles within a certain shape and radius on a x-y coordinate plane, all as specified by the user. It then outputs certain bits of information about the particle distribution.

Makefile Commands:

	make clean:

		Deletes "timing.txt" and "parallelProg02.tgz" from the directory, as part of cleaning up and saving space.

	make test:

		Launches the program into a test mode, where it  runs through a set of generating predeterminated determined amounts of particles. In this mode, the radius is set to 1.0, and the shape is set to square. After completion of the test, the makefile will create a text document containing the run time of each individual file, called "timing.txt". Please see the next section for further detail.

timing.txt:

	The product of running the program's make test. After completing the test, the shell will output how long it took for each file to processed into a single file, called "timing.txt". That is, it records the time it took between the program starting and ending for each individual text document. This period is measured in seconds, and outputed to the second decimal place in the following format:

		numParticles (The number)		timeElapsed (The time it took)

	"timing.txt" is recreated after every call of 'make test', and will be removed if the "make clean" command is called.
