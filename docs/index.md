# rubix-solver
Solves Rubix Cubes!

![Docker Cloud Build Status](https://img.shields.io/docker/cloud/build/yarhamjohn/rubix-solver?style=for-the-badge)
![Docker Stars](https://img.shields.io/docker/stars/yarhamjohn/rubix-solver?style=for-the-badge)
![Docker Pulls](https://img.shields.io/docker/pulls/yarhamjohn/rubix-solver?style=for-the-badge)

![.NET Core](https://github.com/yarhamjohn/rubix-solver/workflows/.NET%20Core/badge.svg)

## Use
This application consists of a command line application that takes the following parameters:

- `--inputJson (-i)` : Json-formatted string representing the cube to be solved
- `--inputJsonFile (-j)` : Path to the json-formatted file containing the representation of the cube to solve
- `--output (-o)` : Path specifying a text file for outputting the list of instructions to solve the cube.

Only one of `--inputJson` and `--inputJsonFile` needs be specfied.
At the moment, the `rubix-solver` works assuming a particular orientation for the cube:
- Front: White
- Back: Yellow
- Left: Red
- Right: Orange
- Top: Green
- Bottom: Blue

If an invalid cube (i.e. unsolvable) cube is entered the application will either crash or end up in an infinite loop...

## Example JSON input
```
{
  "frontFace": {
    "topRow": {
      "leftColumn": {
        "top": "white",
        "front": "red",
        "left": "green"
      },
      "middleColumn": {
        "top": "yellow",
        "front": "red"
      },
      "rightColumn": {
        "top": "orange",
        "front": "yellow",
        "right": "green"
      }
    },
    "middleRow": {
      "leftColumn": {
        "front": "orange",
        "left": "white"
      },
      "middleColumn": {
        "front": "white"
      },
      "rightColumn": {
        "front": "orange",
        "right": "green"
      }
    },
    "bottomRow": {
      "leftColumn": {
        "bottom": "white",
        "front": "blue",
        "left": "orange"
      },
      "middleColumn": {
        "bottom": "blue",
        "front": "orange"
      },
      "rightColumn": {
        "bottom": "red",
        "front": "yellow",
        "right": "green"
      }
    }
  },
  "middleFace": {
    "topRow": {
      "leftColumn": {
        "top": "blue",
        "left": "yellow"
      },
      "middleColumn": {
        "top": "green"
      },
      "rightColumn": {
        "top": "blue",
        "right": "white"
      }
    },
    "middleRow": {
      "leftColumn": {
        "left": "red"
      },
      "middleColumn": {
      },
      "rightColumn": {
        "right": "orange"
      }
    },
    "bottomRow": {
      "leftColumn": {
        "bottom": "red",
        "left": "white"
      },
      "middleColumn": {
        "bottom": "blue"
      },
      "rightColumn": {
        "bottom": "white",
        "right": "green"
      }
    }
  },
  "backFace": {
    "topRow": {
      "leftColumn": {
        "top": "white",
        "back": "orange",
        "left": "green"
      },
      "middleColumn": {
        "top": "yellow",
        "back": "orange"
      },
      "rightColumn": {
        "top": "yellow",
        "back": "blue",
        "right": "orange"
      }
    },
    "middleRow": {
      "leftColumn": {
        "back": "blue",
        "left": "red"
      },
      "middleColumn": {
        "back": "yellow"
      },
      "rightColumn": {
        "back": "red",
        "right": "green"
      }
    },
    "bottomRow": {
      "leftColumn": {
        "bottom": "red",
        "back": "white",
        "left": "blue"
      },
      "middleColumn": {
        "bottom": "yellow",
        "back": "green"
      },
      "rightColumn": {
        "bottom": "red",
        "back": "yellow",
        "right": "blue"
      }
    }
  }
}
```
