using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Convertimos este GameObject como un singleton para que no exista ningún duplicado de este objeto puesto que es único
    public static GameManager instance = null;
    // Creamos un objeto de tipo BoardManager
    public BoardManager boardScript;
    // Variable que indicara el nivel en el que nos encontramos
    private int level = 2; // Inicializada en 3 para testear el juego con la aparición del primer enemigo 

    void Awake()
    {
        // Con la siguiente comprobación nos aseguramos que solo exista una instancia de este GameObject en el juego (Singleton)
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        // Llamamos al método DontDestroyOnLoad para que este GameObject no se destruya con los cambios de escena
        DontDestroyOnLoad(gameObject);
        // Asignamos el componente script "BoardManager" a nuestro objeto del mismo tipo
        boardScript = GetComponent<BoardManager>();
        // Llamamos al método InitGame();
        InitGame();
    }
    // El método InitGame llamará al método SetupScene de BoardManager pasandole como argumento el nivel actual y vaciara la lista de enemigos
    void InitGame()
    {
        boardScript.SetupScene(level);
    }

}
