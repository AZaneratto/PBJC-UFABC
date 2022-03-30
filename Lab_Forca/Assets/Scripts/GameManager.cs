using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int numTentativas;                  // Armazena as tentativas válidas da rodada
    private int maxNumTentativas;                   // Número máximo de tentativas para Forca ou Salvação

    public GameObject centro;                   // objeto no texto que indica o centro da tela
    public GameObject letra;                    // prefab da letra no Game

    private string palavraOculta =  "letra";    // palavra a ser descoberta

    
    private string[] palavrasOcultas = new string[] { "carro", "elefante", "futebol" }; //array de palavras ocultas



    int score = 0;
    private int tamanhoPalavraOculta;           // tamanho da palavra oculta
    char[] letrasOcultas;                       // letras da palavra oculta
    bool[] letrasDescobertas;                   // indicador de quais palavras foram descobertas



    // Start is called before the first frame update
    void Start()
    {

        centro = GameObject.Find("centroDaTela");           // Acha o objeto centro da tela
        initGame();                                         // função initgam
        InitLetras();                                       // inicia função letras
        numTentativas = 0;
        maxNumTentativas = 10;
        UpdateNumTentativas();
        UpdateScore();

        

        
    }

    // Update is called once per frame
    void Update()
    {
        checkTeclado();
    }

    /*
     *  Inicia a quantidade exata de letras na cena baseado na palavra Oculta
     */
    void InitLetras()
    {
        int numLetras = tamanhoPalavraOculta;
        for (int i=0; i < numLetras; i++)
        {

            Vector3 novaPosicao;
            novaPosicao = new Vector3(centro.transform.position.x +((i-numLetras/2.0f)*80),centro.transform.position.y,centro.transform.position.z);
            GameObject l = (GameObject)Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name = "letra" + (i + 1);     // nomeia-se na hierarquia a GameObject com letra (iésima+1), i-1..numLetras
            l.transform.SetParent(GameObject.Find("Canvas").transform);     //posiciona-se como filho do GameObject



        }



    }

    /*
     * Inicia o jogo e faz o tratamento da palavra
     */
    void initGame()
    {
        palavraOculta = PegaUmaPalavraDoArquivo();
        
        tamanhoPalavraOculta = palavraOculta.Length;            // determinia-se o numero  de letras da palavra oculta
        palavraOculta = palavraOculta.ToUpper();                // transma-se a palavra em maiuscula
        letrasOcultas = new char[tamanhoPalavraOculta];         // instanciamos o array char das letras  da palavra    
        letrasDescobertas = new bool[tamanhoPalavraOculta];     // instanciamos a array bool do indicador de acertos
        letrasOcultas = palavraOculta.ToCharArray();            //   




    }
    /*
     *  Principal método, aqui é checado a letra digitada no teclado e compara com a letra oculta, e se certo ou errado atualiza variaveis
     */
    void  checkTeclado()
    {
        if (Input.anyKeyDown)
        {
            char letraTeclado = Input.inputString.ToCharArray()[0];  //pega a letra digitada e coloca no array
            int letraTecladoComoInt = System.Convert.ToInt32(letraTeclado); //converte a letra em um int baseado na tabela ASC
            if (letraTecladoComoInt >= 4 && letraTecladoComoInt <= 122)
            {
                numTentativas++;
                UpdateNumTentativas();
                if (numTentativas > maxNumTentativas)
                {
                    SceneManager.LoadScene("PagEnfor");

                }
                for (int i= 0; i <= tamanhoPalavraOculta; i++)
                {
                    if(!letrasDescobertas[i])
                    {
                        letraTeclado = System.Char.ToUpper(letraTeclado);
                        if (letrasOcultas[i] == letraTeclado)
                        {

                            letrasDescobertas[i] = true;    
                            GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = letraTeclado.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            verificaSePalavraDescoberta();
                        }
                    }
                }

            }


        }


    }

    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = numTentativas + " | " + maxNumTentativas;  // atualiza o número de tentativa || maximo de tentativas na cena

    }

    void  UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Score " + score;  //atualiza o score na cena 
            

    }
   /*
    * Verifica se a palavra foi descoberta para determinar qual cena seguir
    * 
    */

    void verificaSePalavraDescoberta()
    {
        bool condicao = true;  // condição inicia verdadeiro
        for (int i = 0; i < tamanhoPalavraOculta; i++)  // se todas forem descobertas será verdadeiro
        {
            condicao = condicao && letrasDescobertas[i];   
        }
        if (condicao)
        {
            PlayerPrefs.SetString("ultimaPalavraOculta", palavraOculta);
            SceneManager.LoadScene("PagSalvo");
        }

    }
    /*
     * Pega uma palavra aleatoria de um arquivo e relaciona à palavra secreta
     */
    string PegaUmaPalavraDoArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras",typeof(TextAsset));
        string s = t1.text;
        string[] palavras = s.Split(' ');
        int palavraAleatoria = Random.Range(0, palavras.Length+1);
        return (palavras[palavraAleatoria]);
    }
    

}
