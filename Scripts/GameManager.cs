using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Convertimos este GameObject como un singleton para que no exista ning�n duplicado de este objeto puesto que es �nico
    public static GameManager instance = null;
    // Creamos un objeto de tipo BoardManager
    public BoardManager boardScript;
    // Variable que indicara el nivel en el que nos encontramos
    private int level = 2; // Inicializada en 3 para testear el juego con la aparici�n del primer enemigo 

    void Awake()
    {
        // Con la siguiente comprobaci�n nos aseguramos que solo exista una instancia de este GameObject en el juego (Singleton)
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        // Llamamos al m�todo DontDestroyOnLoad para que este GameObject no se destruya con los cambios de escena
        DontDestroyOnLoad(gameObject);
        // Asignamos el componente script "BoardManager" a nuestro objeto del mismo tipo
        boardScript = GetComponent<BoardManager>();
        // Llamamos al m�todo InitGame();
        InitGame();
    }
    // El m�todo InitGame llamar� al m�todo SetupScene de BoardManager pasandole como argumento el nivel actual y vaciara la lista de enemigos
    void InitGame()
    {
        boardScript.SetupScene(level);
    }

}
