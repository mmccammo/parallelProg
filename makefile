particles: particles.o
	gcc particles.o -o particles -lm -fopenmp
particles.o: particles.c
	gcc particles.c -c -I.
clean:
	rm -f *.o
	rm -f parallelProg.tgz
release: clean
	cd ..; tar cvfz parallelProg.tgz parallelProg
