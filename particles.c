
/* Particles.c
   Matt McCammon
   September 1st, 2013
	
   */

////////////////

#include <sys/time.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdio.h>
#include <ctype.h>
#include <string.h>
#include <math.h>
#include <string.h>
#include <omp.h>

typedef int bool;
#define true 1
#define false 0

////////////////

typedef struct
            {
			double x;
			double y;

            } particleType;

particleType* generateSquare(int numParticles, double radius) {

	particleType* p;
	int n;

	p = (particleType *) malloc( numParticles * sizeof( particleType ) );
	if ( p == NULL ) {
		printf("Memory error while generating square.");
		exit(0);
	}

	#pragma omp parallel
 	{
		#pragma omp for private(n)
		for(n=0; n<numParticles; n++) {
			//printf("Index: %d numThreads: %d currentThread: %d\n", n, omp_get_num_threads(), omp_get_thread_num( ));
			p[n].x = 2.0 * radius * (drand48() - 0.5);
			p[n].y = 2.0 * radius * (drand48() - 0.5);
		}
	}

	return p;

}

double calcDistance (particleType a, particleType b) {

	double distance, dx, dy;
	dx = b.x - a.x;
	dy = b.y - a.y;
	distance = sqrt( dx*dx + dy*dy );
	return distance;
}

particleType* generateDisk(int numParticles, double radius) {

	particleType* p;
	int i;
	particleType origin;
	double distance;
	origin.x = 0.0;
	origin.y = 0.0;

	p = (particleType *) malloc( numParticles * sizeof( particleType ) );
	if ( p == NULL ) {
		printf("Memory error while generating square.");
		exit(0);
	}

	#pragma omp parallel
 	{
		#pragma omp for private(i)
		for ( i = 0; i < numParticles; i++ ) {
			distance = radius;
			while ( distance >= radius ) {
				p[i].x = 2.0 * (drand48() - 0.5);
				p[i].y = 2.0 * (drand48() - 0.5);
				distance = calcDistance(p[i], origin);
			}
		}
	}

	return p;

}

double calcDistanceInverseSquared (particleType a, particleType b) {

	double distance, dx, dy;

	distance = calcDistance(a,b);

	if (distance == 0) return 0;

	distance = abs( 1.0 / (distance * distance) );

	return distance;
}


double calcxMean (particleType* p, int numParticles) {

	double temp = 0.0;
	int i;


	#pragma omp parallel
 	{
		#pragma omp for private(i)
		for ( i = 0; i < numParticles; i++ ) {
			
			temp += p[i].x;
		}
	}

	temp /= numParticles;

	return temp;

}

double calcyMean (particleType* p, int numParticles) {

	double temp = 0.0;
	int i;

		#pragma omp for private(i)
		for ( i = 0; i < numParticles; i++ ) {
			
			temp += p[i].y;
		}
	

	temp /= numParticles;

	return temp;

}

double calcxVariance (particleType* p, int numParticles) {

	double xVariance, difference;
	xVariance = 0.0;
	int i;
	double xMean = calcxMean(p, numParticles);

		#pragma omp for private(i)
		for ( i = 0; i < numParticles; i++) {
			
			difference = p[i].x - xMean;
			
			xVariance += difference * difference;
		}
	

	xVariance /= numParticles;

	return xVariance;
}

double calcyVariance (particleType* p, int numParticles) {

	double yVariance, difference;
	yVariance = 0.0;
	int i;

		#pragma omp for private(i)
		for ( i = 0; i < numParticles; i++ ) {

			difference = p[i].y - calcyMean(p, numParticles);
			
			yVariance += difference * difference;
		}
	

	yVariance /= (double)numParticles;

	return yVariance;
}

double calcdistanceMean (particleType* p, int numParticles) {

	int i,j;
	double isum, jsum, mean;

	isum = 0;

	

		#pragma omp for private(i,j)
		for ( i = 0; i < numParticles; i++ ) {

			jsum = 0;

			for ( j = 0; j < numParticles; j++ ) {

				if (i == j) continue;
				
				jsum += calcDistance(p[i],p[j]);
	
			}

			
			isum += jsum / (numParticles-1);
		}

	

	mean = isum/numParticles;

	return mean;

}

double calcdistanceVariance (particleType* p, int numParticles) {

	double distanceVariance, isum, jsum, difference, temp;
	distanceVariance= 0.0;
	int i,j;
	double distanceMean = calcdistanceMean(p, numParticles);

	isum = 0.0;

	
		#pragma omp for private(i,j)

		for ( i = 0; i < numParticles; i++ ) {

			jsum = 0.0;

			for ( j = 0; j != i; j++ ) {

				if (i == j) continue;
				difference = calcDistance(p[i],p[j]) - distanceMean;
				
				jsum += difference * difference;

			}

			
			isum += jsum / (numParticles - 1);
		
		}
	

	distanceVariance = isum/numParticles;

	return distanceVariance;

}

double calcdistanceInverseSquaredMean (particleType* p, int numParticles) {

	int i,j;
	double isum, jsum, mean;

	isum = 0;

	
		#pragma omp for private(i,j)
		for ( i = 0; i < numParticles; i++ ) {

			jsum = 0;

			for ( j = 0; j < numParticles; j++ ) {

				if (i == j) continue;
				
				jsum += calcDistanceInverseSquared (p[i],p[j]);

			}

			
			isum += jsum / (numParticles-1);
		}

	

	mean = isum/numParticles;

	return mean;

}

double calcdistanceInverseSquaredVariance(particleType* p, int numParticles) {

	double distanceVariance, isum, jsum, difference, temp;
	distanceVariance= 0.0;
	int i,j;
	double distanceMean = calcdistanceInverseSquaredMean(p, numParticles);

	isum = 0.0;

	
		#pragma omp for private(i,j)
		for ( i = 0; i < numParticles; i++ ) {

			jsum = 0.0;

			for ( j = 0; j < numParticles; j++ ) {

				if (i == j) continue;
				difference = calcDistanceInverseSquared(p[i],p[j]);
				if (difference == 0) continue;
				
				difference -= distanceMean;
				
				jsum += difference * difference;

			}

			
			isum += jsum / (numParticles - 1);
		
		}

	
		distanceVariance = isum/numParticles;

		return distanceVariance;

}

void generateStats (particleType* p, int numParticles, double radius) {
/*
	printf("numParticles %d\n", (int)numParticles);
	printf("shape square\n");
	printf("radius %.1f\n", radius);
	printf("xMean %f\n", calcxMean(p, numParticles) );
	printf("yMean %f\n", calcyMean(p, numParticles) );
	printf("xVariance %f\n", calcxVariance(p, numParticles) );
	printf("yVariance %f\n", calcyVariance(p, numParticles) );
	printf("distanceMean %f\n", calcdistanceMean(p, numParticles) );
	printf("distanceVariance %f\n", calcdistanceVariance(p, numParticles) );
	printf("distanceInverseSquaredMean %f\n", calcdistanceInverseSquaredMean(p, numParticles) );
	printf("distanceInverseSquaredVariance %f\n", calcdistanceInverseSquaredVariance(p, numParticles) );*/

	calcxMean(p, numParticles);
	calcyMean(p, numParticles);
	calcxVariance(p, numParticles);
	calcyVariance(p, numParticles);
	calcdistanceMean(p, numParticles);
	calcdistanceVariance(p, numParticles);
	calcdistanceInverseSquaredMean(p, numParticles);
	calcdistanceInverseSquaredVariance(p, numParticles);


}

void errorChecking(int argc, char *argv[]) {

	if (argc != 7) {
		printf("Wrong number of parameters (Should be './particles numParticles (number) shape (square or disk) radius (number)'.\n");
		exit(0);
	}

	else if (strcmp (argv[1],"numParticles") == 1) {
		printf("Incorrect second parameter (Should be 'numParticles').\n");
		exit(0);
	}

	else if (strcmp (argv[3],"shape") == 1) {
		printf("Incorrect fourth parameter (Should be 'shape').\n");
		exit(0);
	}

	else if (strcmp (argv[5],"radius") == 1) {
		printf("Incorrect fourth parameter (Should be 'radius').\n");
		exit(0);
	}
	else {
		int numParticles = atof(argv[2]);
		double radius = atof(argv[6]);

		if (strcmp(argv[4],"square") == 0) generateStats(generateSquare(numParticles, radius), numParticles, radius);
		else if (strcmp(argv[4],"disk") == 0) generateStats(generateDisk(numParticles, radius), numParticles, radius);	
	}
}

/*
void fcheck (double x, double y) {

	double i,j;
	FILE * pFile;
	pFile = fopen ("max.txt","r+");
	fscanf (pFile, "%lf %lf", &i, &j);
	//printf ("xy %f %f\n", x,y);

	if (i == 0) i = x;

	if (i < x) x = i;
	if (j > y) y = j;

	//printf("%f\n%f\n", x,y);	

	freopen("max.txt", "r+", stdout);

	printf("%f\n%f\n", x,y);

	fclose(pFile);

}
*/

int main(int argc, char *argv[]){

	struct timeval start, end;
	gettimeofday(&start, NULL);

	errorChecking(argc,argv);

	gettimeofday(&end, NULL);

	double s1 = start.tv_sec;
	double s2 = start.tv_usec;
	double s3 = s1 + (s2 / 1000000);

	double e1 = end.tv_sec;
	double e2 = end.tv_usec;
	double e3 = e1 + (e2 / 1000000);

	double t1 = e1 - s1;
	double t2 = e2 - s2;
	//printf ("s1 %f\n", s3);
	printf("	timeElapsed: %f \n", t1 + (t2 / 1000000));
//	fcheck(s3, e3);
	//printf("\n	numParticles %d timeStart %f.%8f timeEnd %f.%8f timeElapsed %f %8f\n", atoi(argv[2]), s1, s2, e1, 2, e1 - s1, e2 - s2);

	return 0;

}
