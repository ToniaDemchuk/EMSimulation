
# Electromagnetic simulation package #

[![Build Status](https://travis-ci.org/ToniaDemchuk/EMSimulation.svg?branch=master)](https://travis-ci.org/ToniaDemchuk/EMSimulation)

This is a software packages for electromagnetic simulations.

### What is this repository for? ###

* The package includes the implementation of computational electromagnetic methods:
	 - discrete dipole approximation (DDA) 
	 - finite-difference time-domain (FDTD)

* The methods calculate optical spectra of nanoparticles in the dielectric medium.

## Table of Contents


- [Prerequisites](#prerequisites)
- [Install](#install)
- [Usage](#usage)
- [Contribute](#contribute)
- [License](#license)



## Prerequisites
The console applications use the [*gnuplot*](http://gnuplot.sourceforge.net/).

[Download](http://gnuplot.sourceforge.net/download.html) the gnuplot installer  and install it with the standard steps.


## Install

### Linux

Installation process on Linux requires to clone repository into local system using 
```
git clone https://github.com/ToniaDemchuk/EMSimulation
```
and
```
dotnet build
```
in `Simulation.DDA.Console` or `Simulation.FDTD.Console` if you want to build either DDA- or FDTD-part of the project respectively.

Build step is not stict requirement, because you always can build or re-build it during [usage](#usage) session.

## Usage

### Linux

To start DDA-simulation, in the terminal run:
```
dotnet run
```
in `Simulation.DDA.Console`. 

Note, that `run` build project as first step, it you pre-build it during installation, you can just run:
```
dotnet run --no-build
```
to explicitly make `dotnet` use already built files.

To start FDTD-simulation, do the same, but in `Simulation.FDTD.Console`.

Commands and options of `dotnet`-driver are described in more details [elsewhere](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet).

Settings for DDA-calculations are contained in `ddaParameters.xml` (for incident field), `dipols.txt` (for geometry of the problem) and `opt_consts.txt` (for medium treatment).
Similarly, input geometry for FDTD-calculation could be found and modified in `sphere.fds` from `Simulation.FDTD.Console`.

## Contribute

Feel free to open an issue or submit a PR.


## License

See the [LICENSE](LICENSE.md) file for license rights and limitations (MIT).
