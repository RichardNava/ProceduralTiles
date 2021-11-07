using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    // Clase Serializable que servira para establecer las cantidades minimas y maximas de ciertas losetas
    [Serializable]
    public class Count
    {
        public int minimun;
        public int maximun;

        public Count(int min, int max)
        {
            this.minimun = min;
            this.maximun = max;
        }
    }
    // Variables númericas de las dimensiones de nuestro tablero
    public int columns = 8;
    public int rows = 8;
    // Creamos objetos de nuestra clase Count() de las losetas que queremos limitar a ciertas cantidades
    public Count wallCount = new Count(6, 10);
    public Count foodCount = new Count(1, 5);
    // Matrices de GameObjects que contendran las distintas losetas
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    // Variable de tipo transform para que los objetos creados se conviertan en su hijo y mantener limpia la jerarquia
    private Transform boardHolder;
    // Lista de Vectores para rastrear en el tablero de juego si un objeto se ha generado en es posición o no.
    private List<Vector3> gridPositions = new List<Vector3>();
    // Método para crear la lista de las posiciones del tablero
    void InitialiseList()
    {   // Llamamos al método Clear() para vaciar la lista y empezar desde 0
        gridPositions.Clear();
        // Bucle For anidado que ira recorriendo las columnas y las filas que tengamos declaradas
        for (int x = 1; x < columns -1; x++)
        {                                       // Restamos -1 al valor de Columnas/Filas para dejar un borde practicable que podamos flanquear
            for (int y = 1; y < rows -1; y++)
            {
                // Añadimos un Vector a la lista con nuestros valores X e Y para ir marcando las posiciones del tablero
                gridPositions.Add(new Vector3(x, y, 0));
            }
        }
    }
    // Método para configurar la pared exterior y el suelo de nuestro tablero
    void BoardSetup()
    {
        // Asignamos a boardHolder el transform de un nuevo objeto llamado "Board"
        boardHolder = new GameObject("Board").transform;
        // Bucle For anidado que ira recorriendo las columnas y las filas que tengamos declaradas
        for (int x = -1; x < columns + 1; x++)
        {                                       // Sumamos +1 al valor de Columnas/Filas e inicializamos el indice en -1
                                                // para establecer la pared impracticable exterior del tablero
            for (int y = -1; y < rows + 1; y++)
            {
                // Añadimos las losetas de suelo desde su matriz de forma aleatoria
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                // Comprobamos si alguna de nuestras coordenadas coincide con las que corresponden a la pared exterior
                if (x == -1 || x == columns || y == -1 || y == columns)
                {
                    // Asignamos al objeto para instanciar las losetas correspondientes a la pared
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                // Instanciamos la loseta en las coordenadas correspondientes
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                // Asignamos la nueva loseta creada como hija del gameobject contenedor/padre
                instance.transform.SetParent(boardHolder);
            }
        }
    }
    // Creamos un método que devuelve un Vector3 aleatorio de nuestra lista de posiciones para colocar los items
    Vector3 RandomPosition()
    {   // Creamos un indice aleatorio donde el limite superior será la cantidad de coordenadas de nuestra lista
        int randomIndex = Random.Range(0,gridPositions.Count);
        // Asignamos a una variable Vector3 las coordenadas obtenidas de la lista con nuestro indice aleatorio
        Vector3 randomPosition = gridPositions[randomIndex];
        // Removemos de la lista de Vectores las mismas coordenadas para evitar que volvamos a colocar un item en la misma loseta
        gridPositions.RemoveAt(randomIndex);
        // Devolvemos las coordenadas para su posterior uso
        return randomPosition;

    }
    // Creamos un método para colocar los items/losetas que recibirá tres parámetros
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimun, int maximun)
    {   // Creamos una variable númerica aleatoria que asignara la cantidad de un tipo de item/loseta que vamos a crear
        int objCount = Random.Range(minimun, maximun + 1);
        // Bucle para crear nuestros items/losetas en función de su número máximo
        for (int i = 0; i < objCount; i++)
        {
            // Llamamos a nuestro método RandomPosition() para asignar su devolución a una variable de tipo Vector3
            Vector3 randomPosition = RandomPosition();
            // Asignamos una loseta aleatoria de la matriz que pasamos por argumento al método
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            // Instanciamos la loseta en la posición aleatoria que hemos obtenido previamente
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
    // Método que recoge un parametro int para identificar el nivel y configurar el tablero llamando al resto de métodos.
    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        // Llamamos varias veces al método LayoutObjectAtRandom() para los distintos items limitados
        if (level > 10 && level < 20 ) wallCount = new Count(7, 11);
        if (level > 20 && level < 30) { wallCount = new Count(8, 12); foodCount = new Count(2, 6); }
        if (level > 30 && level < 40 ) wallCount = new Count(9, 14);
        LayoutObjectAtRandom(wallTiles, wallCount.minimun, wallCount.maximun);
        LayoutObjectAtRandom(foodTiles, foodCount.minimun, foodCount.maximun);
        // Creamos una variable que recoge un valor de una función logarítmica para un escalado de dificultad en función del nivel
        int enemyCount = (int)Mathf.Log(level,2f); // 1-2 = 0 Enemigos / 3-4 = 1 Enemigo / 5-8 = 2 Enemigos...
        // Instanciamos los enemigos en función del nivel con la varaiable anterior
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        // Instanciamos la salida
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);

    }

}
