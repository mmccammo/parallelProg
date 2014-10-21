#!/bin/bash

rm -f timing.txt
rm -f max.txt
echo 0 0 >> max.txt

echo "Enter number of nodes"              # Nodes in this context refers to the system cluster, which is formed by linking anywhere from 2 to 32 Beaglebone Black boards together.
read -e N
setterm -background $N

echo "Enter number of processes"          # Iterations of the program to be run simultaneously
read -e P
setterm -background $P

echo "Processing. Please wait."

                                          # Number of nodes used in this process remains static in each run, only the number of processes changes
                                          # Necessary for the graphing function performed elsewhere

for ((i=1; i <= $P; i++))
do
	echo "numNodes" $N "numProcesses" $i >> timing.txt
	if [ $N -eq 1 ]
		then
			mpirun -np $i particles numParticles 6000 shape square radius 1.0 >> timing.txt
	fi
	if [ $N -eq 2 ]
		then
			mpirun -n 2 -np $i -hostfile ~/hostfile mparticle numParticles 6000 shape square radius 1.0 >> timing.txt
	fi
done
                            # The individual runtime for each cycle is stored in timing.txt
subtract() {
   echo "$1 - $2" | bc
}

filename="$1"
z=0
while read -r line
do
    name=$line

if [ $z -eq 0 ]
	then
		x=$name
fi
if [ $z -eq 1 ]
	then
		y=$name
fi
z=1
done < "max.txt"              # Requires by assignment, not part of the overall function

z="$(subtract 0 $x)"

echo "Number of seconds the script took to complete:"              #Chec
echo "$y + $z" | bc
