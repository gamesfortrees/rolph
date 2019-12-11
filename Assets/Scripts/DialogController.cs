using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogController : MonoBehaviour
{
    public Sprite rolph;
    public Sprite scaredRolph;
    public Sprite douglas;
    public Sprite angryDouglas;
    private Text text;
    private Image avatar;
    private GameObject box;
    private int currentLine = 0;
    private string[] texts;
    private Sprite[] avatars;
    private GameController gameController;
    private bool active = false;
    private bool resume = false;
    private bool transitionAfterDialog = false;

    void Start()
    {
        box = gameObject.transform.GetChild(0).gameObject;
        box.SetActive(true);
        text = GetComponentInChildren<Text>();
        avatar = GetComponentsInChildren<Image>()[1];
        box.SetActive(false); 
        gameController = GameObject.FindWithTag("GameMaster").GetComponent<GameController>();
    }

    void Update()
    {
        if (!active)
        {
            return;
        }

        if (Input.GetKeyDown("space"))
        {
            AdvanceText();
        }
    }

    private void LateUpdate()
    {
        if (resume)
        {
            gameController.Resume();
            resume = false;
        }
    }

    public void Dialog1_1()
    {
        avatars = new Sprite[] {
            null,
            rolph,
            null,
            rolph,
            rolph,
            null,
            rolph,
            null,
            scaredRolph,
            null,
            scaredRolph,
            null,
        };
        texts = new string[] {
            "Rolph wakes up in the middle of the forest. He doesn’t know what happened or how did he end up there… (advance text with 'space')",
            "What a terrible headache! Where’s my office?",
            "He looks around and sees himself surrounded by a beautiful green forest",
            "Oh there are many trees here! Our coal power plants will be replenished after cutting all of them! Let’s call the tree machines quietly before those hippies ruin my plans again!",
            "Where is my phone?",
            "Suddenly the roar of an excavator calls his attention",
            "What is that noise? It hurts my brain and doesn’t let me focus!! Can’t find my phone. Wait a minute, that is one of our machines!! Hey I’m here!!!",
            "The machine starts approaching faster and faster without intention of reducing the speed",
            "What are you doing? Stop!!! That’s enough!! I’m the CEO of the company!!",
            "The excavator isn’t stopping and a scared Rolph starts running away",
            "Why isn’t he stopping? How can’t he recognize me? I have my my sexy western German accent, my brown and woody bark, my green long leafs… wait a minute… bark? Leafs? I AM A TREE!!!!!!!",
            "Run away from the excavator using 'a' or 'left arrow' and 'd' or 'right arrow' to run and 'space' to jump",
        };
    }

    public void Dialog1_2()
    {
        avatars = new Sprite[] {
            rolph,
            angryDouglas,
        };
        texts = new string[] {
            "What's this?",
            "This is a seed, dummy! Seeds grow into trees, which slow down the excavator. Before you can plant it however you need to find a patch of fertile soil…",
        };
    }

    public void Dialog1_3()
    {
        avatars = new Sprite[] {
            douglas,
        };
        texts = new string[] {
            "Here finally a fertile piece of soil. Plant your seed (press 's').",
        };
    }

    public void Dialog1_4()
    {
        avatars = new Sprite[] {
            rolph,
            angryDouglas,
            scaredRolph,
            angryDouglas,
            douglas,
        };
        texts = new string[] {
            "Phew I made it. These excavators are horrible!",
            "You don’t say! You are not safe here, there are more coming. We need to stop them and save the forest or all trees are lost… including yourself!",
            "But how can I do this I’m just a tree, they won’t listen to me.",
            "Well maybe you should have done something before…",
            "Still there is hope, but you have to find it yourself.",
        };
        transitionAfterDialog = true;
    }

    public void Dialog2_1()
    {
        avatars = new Sprite[] {
            rolph,
            douglas,
            douglas,
        };
        texts = new string[] {
            "What is this?",
            "That's the Ecosia office, they are the search engine that plants trees. With their help we can save the forest. They already planted over 75 000 000 trees!",
            "Have a look!",
        };
        transitionAfterDialog = true;
    }

    void AdvanceText()
    {
        if (currentLine < texts.Length)
        {
            text.text = texts[currentLine];
            avatar.sprite = avatars[currentLine];
            if (avatars[currentLine] == null)
            {
                text.fontStyle = FontStyle.Italic;
                avatar.color = new Color(0,0,0,0);
            } else
            {
                text.fontStyle = FontStyle.Normal;
                avatar.color = new Color(255, 255, 255, 255);
            }
            currentLine += 1;
        }
        else
        {
            box.SetActive(false);
            active = false;
            resume = true;
            if (transitionAfterDialog)
            {
                transitionAfterDialog = false;
                gameController.NextLevel();
            }
        }
    }

    public void Activate()
    {
        currentLine = 0;
        gameController.Pause();
        box.SetActive(true);
        AdvanceText();
        active = true;
    }
}