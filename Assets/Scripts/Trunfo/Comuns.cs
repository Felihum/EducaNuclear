using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Comuns : MonoBehaviour
{
    private GameObject panelExcluirInstanciado;
    public bool carregaPanelExcluir(Action<int> onExclusaoConfirmada, string aviso)
    {
        if (panelExcluirInstanciado != null)
        {
            Destroy(panelExcluirInstanciado);
            panelExcluirInstanciado = null;

            return false;
        }

        if (string.IsNullOrEmpty(aviso))
        {
            aviso = "Tem certeza que deseja excluir?";
        }

        GameObject panelExcluir = Resources.Load<GameObject>("Prefabs/Prefabs Variantes/panelExcluir");
        panelExcluirInstanciado = Instantiate(panelExcluir, retornaCanvas().transform);

        List<Button> listaButtonsResposta = new List<Button>
        {
            retornaButton("buttonSim"),
            retornaButton("buttonNão")
        };

        for (int i = 0; i < listaButtonsResposta.Count(); i++)
        {
            int index = i;

            listaButtonsResposta[i].onClick.AddListener(() =>
            {
                onExclusaoConfirmada?.Invoke(index);

                Destroy(panelExcluirInstanciado);
                panelExcluirInstanciado = null;
            });
        }

        panelExcluirInstanciado.GetComponentInChildren<TMP_Text>().text = aviso;

        return true;
    }

    public void carregaPanelMensagem(string mensagem)
    {
        GameObject panelMensagem = Resources.Load<GameObject>("Prefabs/Prefabs Variantes/panelMensagem"), panelMensagemInstanciado = Instantiate(panelMensagem, retornaCanvas().transform);

        Button buttonOk = panelMensagemInstanciado.GetComponentInChildren<Button>();
        buttonOk.onClick.AddListener(() =>
        {
            Destroy(panelMensagemInstanciado);
        });

        string textMensagem = panelMensagemInstanciado.GetComponentInChildren<TMP_Text>().text = mensagem;
    }

    public Sprite converteImagem(string imagem)
    {
        byte[] fileData;
        fileData = File.ReadAllBytes(imagem);

        Texture2D textura = new Texture2D(2, 2);
        textura.LoadImage(fileData);

        Sprite sprite = Sprite.Create(textura, new Rect(0, 0, textura.width, textura.height), new Vector2(0.5f, 0.5f));

        return sprite;
    }

    public GameObject instanciaGameObject(GameObject panelPai, string nomeGameObject)
    {
        GameObject objeto = Resources.Load<GameObject>("Prefabs/" + nomeGameObject), objetoInstanciado = Instantiate(objeto, panelPai.transform);

        return objetoInstanciado;
    }

    public void quitaCena()
    {
        if (carregaPanelExcluir((resp) =>
        {
            if (resp == 0)
            {
                voltarCena();
            }
        }, null)) { }
    }

    public void quitaJogo()
    {
        if (carregaPanelExcluir((resp) =>
        {
            if (resp == 0)
            {
                Application.Quit();
            }
        }, "Tem certeza que deseja sair do jogo?")) { }
    }

    public Button retornaButton(string nomeButton)
    {
        Button buttonInstanciado = GameObject.Find(nomeButton).GetComponent<Button>();

        return buttonInstanciado;
    }

    public Canvas retornaCanvas()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        return canvas;
    }

    public string retornaDiretorio(string primeiraParte, string segundaParte)
    {
        string diretorio = Path.Combine(Application.dataPath, primeiraParte, segundaParte);

        return diretorio;
    }

    public string[] retornaImagensPasta(string diretorioPasta)
    {
        string[] imagens = Directory.GetFiles(diretorioPasta, "*.jpg").Concat(Directory.GetFiles(diretorioPasta, "*.jpeg")).Concat(Directory.GetFiles(diretorioPasta, "*.png")).ToArray();

        return imagens;
    }

    public void voltarCena()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;

        switch (cenaAtual)
        {
            case "Menu":
            case "Teste":
                SceneManager.LoadScene("Menu");
                break;
            case "ModoProfessor":
                SceneManager.LoadScene(cenaAtual);
                break;
        }
    }
}